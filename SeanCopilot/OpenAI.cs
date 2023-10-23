using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public class OpenAI
    {
        private List<Dictionary<string, string>> conversationHistory;

        public OpenAI()
        {
            ClearHistory();
        }

        private static Dictionary<string, string> ChatMessage(string role, string message, string functionName = "")
        {
            var msg = new Dictionary<string, string> { { "role", role }, { "content", message } };

            if (functionName.Length > 0)
                msg.Add("name", functionName);

            return msg;
        }

        public int GetHistoryLength()
        {
            return this.conversationHistory.Count;
        }

        public void ClearHistory()
        {
            conversationHistory = new List<Dictionary<string, string>>();
            conversationHistory.Add(ChatMessage("system", Main.GetInstructions()));
        }

        private List<object> CreateMessageList(string prompt, bool useHistory)
        {
            var messages = new List<object>();

            if (useHistory)
            {
                foreach (Dictionary<string, string> message in conversationHistory)
                    messages.Add(message);
            }
            else
            {
                messages.Add(ChatMessage("system", Main.GetInstructions()));
            }

            messages.Add(ChatMessage("user", prompt));
            return messages;
        }

        public static string[] GetGPTModels()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create("https://api.openai.com/v1/models");
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + Main.configManager.GetConfigValue("api_key"));
            request.UseDefaultCredentials = false;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            HashSet<string> models = new HashSet<string>();

            try
            {
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Dictionary<string, object> parsedResponse = serializer.Deserialize<Dictionary<string, object>>(result);
                    var data = (System.Collections.ArrayList)parsedResponse["data"];
                    foreach (object model in data)
                    {
                        var fields = (Dictionary<string, object>)model;
                        models.Add(Convert.ToString(fields["id"]));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error fetching models from OpenAI: " + e.Message);
            }

            return models.OrderBy(s => s).ToArray();
        }


        public Task<string[]> RunGPTFunction(string prompt, bool useHistory)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create("https://api.openai.com/v1/chat/completions");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + Main.configManager.GetConfigValue("api_key"));
            request.UseDefaultCredentials = false;

            var messages = CreateMessageList(prompt, useHistory);
            var functions = GetFunctions();

            var payload = new Dictionary<string, object>
            {
                { "model", Main.configManager.GetConfigValue("gpt_model") },
                { "messages", messages },
                { "functions", functions },
                //{ "function_call", "provide_code_to_npp_user" },
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string postData = serializer.Serialize(payload);
            string responseData = string.Empty;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            try
            {
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseData = streamReader.ReadToEnd();
                    Dictionary<string, object> parsedResponse = serializer.Deserialize<Dictionary<string, object>>(responseData);
                    var choices = (System.Collections.ArrayList)parsedResponse["choices"];
                    Dictionary<string, object> choice = (Dictionary<string, object>)choices[0];
                    Dictionary<string, object> message = (Dictionary<string, object>)choice["message"];

                    if (message.ContainsKey("function_call"))
                    {
                        return Task.FromResult(InterpretFunctionCall(serializer, message["function_call"], useHistory));
                    }
                    else
                    {
                        return Task.FromResult(InterpretChatResponse(serializer, message["content"], useHistory));
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exception
                return Task.FromResult(new string[] { string.Empty, "*Error: " + e.Message + "\r\n*Request: " + postData + "\r\n*Response: " + responseData });
            }
        }

        private string[] InterpretFunctionCall(JavaScriptSerializer serializer, object function, bool useHistory)
        {
            try
            {
                Dictionary<string, object> functionCall = (Dictionary<string, object>)function;
                string functionArguments = functionCall["arguments"].ToString();

                if (useHistory)
                {
                    this.conversationHistory.Add(ChatMessage("function", function.ToString(), Convert.ToString(functionCall["name"])));
                }

                try
                {
                    Dictionary<string, object> arguments = HandleDodgyJson(serializer, functionArguments);
                    return new string[] { arguments["code"].ToString(), arguments["explanation"].ToString() };
                }
                catch (Exception e)
                {
                    return new string[] { string.Empty, e.Message + "\r\n" + functionArguments };
                }
            }
            catch (Exception e)
            {
                return new string[] { string.Empty, e.Message + "\r\n*Function call: " + function.ToString() };
            }
        }

        private string[] InterpretChatResponse(JavaScriptSerializer serializer, object response, bool useHistory)
        {
            if (useHistory)
            {
                this.conversationHistory.Add(ChatMessage("assistant", response.ToString()));
            }

            try
            {
                try
                {
                    Dictionary<string, object> answers = HandleDodgyJson(serializer, response);
                    return new string[] { answers["code"].ToString(), answers["explanation"].ToString() };
                }
                catch(Exception)
                {
                    return ParseChatResponse(response);
                }
            }
            catch (Exception e)
            {
                return new string[] { string.Empty, "*Error: " + e.Message + "\r\nResponse: " + response.ToString() };
            }
        }

        private static string[] ParseChatResponse(object response)
        {
            StringBuilder code = new StringBuilder();
            StringBuilder explanation = new StringBuilder();
            if(response.ToString().ToLower().Contains("code:"))
            {
                // Regular expression pattern to match the code and explanation
                string pattern = @"(?<=code\:\s)[\s\S]*?(?=\sexplanation\:)|(?<=explanation\:\s)[\s\S]*";

                // Use regex to extract the code and explanation from the response
                MatchCollection matches = Regex.Matches(response.ToString(), pattern, RegexOptions.IgnoreCase);

                if (matches.Count >= 2)
                {
                    code.Append(matches[0].Value);
                    explanation.Append(matches[1].Value.Trim());
                }
                else
                {
                    explanation.Append(response.ToString());
                }
            }
            else if (response.ToString().Contains("```"))
            {
                string[] responseParts = response.ToString().Split(new string[] { "```" }, StringSplitOptions.None);
                for (int i = 0; i < responseParts.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        explanation.AppendLine(responseParts[i]);
                    }
                    else
                    {
                        code.AppendLine(responseParts[i]);
                    }
                }
            }
            else
            {
                explanation.Append(response.ToString());
            }

            return new string[] { code.ToString(), explanation.ToString() };
        }

        private static Dictionary<string, object> HandleDodgyJson(JavaScriptSerializer serializer, object response)
        {
            string cleanedResponse = response.ToString();

            // Remove the C# formatting characters
            if (cleanedResponse.Contains("@\""))
            {
                cleanedResponse = cleanedResponse.Replace("@\"", "\"")
                    .Replace("\"\"", "\"")
                    .Replace("\\\"", "\"");
            }

            return serializer.Deserialize<Dictionary<string, object>>(cleanedResponse);
        }

        private static List<object> GetFunctions()
        {
            return new List<object> {
                new Dictionary<string, object> {
                    { "name", "provide_code_to_npp_user" },
                    { "description", "Give a code solution to the Notepad++ user, with an explanation/reason" },
                    { "parameters", new Dictionary<string, object> {
                            { "type", "object" },
                            { "properties", new Dictionary<string, object> {
                                    { "code", new Dictionary<string, object> {
                                            {  "type", "string" },
                                            { "description", "The generated code which should be provided back to the user. Generated code should be accurate, efficient, optimised for readability and properly indented." }
                                        }
                                    },
                                    { "explanation", new Dictionary<string, object> {
                                            {  "type", "string" },
                                            { "description", "An explanation of how the code provided solves the problem, in concise language that is easy to understand." }
                                        }
                                    }
                                }
                            },
                            { "required", new string[]{ "code", "explanation" } }
                        }
                    },
                }
            };
        }
    }
}