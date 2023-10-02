using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        public Task<string[]> GetGPTResponse(string prompt, bool useHistory)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create("https://api.openai.com/v1/chat/completions");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + Main.configManager.GetConfigValue("api_key"));
            request.UseDefaultCredentials = false;

            List<Dictionary<string, string>> messages = CreateMessageList(prompt, useHistory);

            var payload = new Dictionary<string, object>
            {
                { "model", Main.configManager.GetConfigValue("gpt_model") },
                { "messages", messages }
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string postData = serializer.Serialize(payload);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            try
            {
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Dictionary<string, object> parsedResponse = serializer.Deserialize<Dictionary<string, object>>(result);
                    var choices = (System.Collections.ArrayList)parsedResponse["choices"];
                    Dictionary<string, object> choice = (Dictionary<string, object>)choices[0];
                    Dictionary<string, object> message = (Dictionary<string, object>)choice["message"];

                    string response = message["content"].ToString();
                    if (useHistory)
                    {
                        this.conversationHistory.Add(ChatMessage("assistant", response));
                    }

                    try
                    {
                        Dictionary<string, object> answers = serializer.Deserialize<Dictionary<string, object>>(response);
                        return Task.FromResult(new string[] { answers["code"].ToString(), answers["reason"].ToString() });
                    }
                    catch (Exception e)
                    {
                        return Task.FromResult(new string[] { string.Empty, e.Message + "\r\nCannot refactor, but my response is:\r\n" + message["content"].ToString() });
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exception
                return Task.FromResult(new string[] { string.Empty, e.Message });
            }
        }

        private static Dictionary<string, string> ChatMessage(string role, string message)
        {
            return new Dictionary<string, string> { { "role", role }, { "content", message } };
        }

        public int GetHistoryLength()
        {
            return (this.conversationHistory.Count - 1) / 2;
        }

        public void ClearHistory()
        {
            conversationHistory = new List<Dictionary<string, string>>();
            conversationHistory.Add(ChatMessage("system", Main.GetInstructions()));
        }

        private List<Dictionary<string, string>> CreateMessageList(string prompt, bool useHistory)
        {
            var messages = new List<Dictionary<string, string>>();

            if (useHistory)
            {
                messages = this.conversationHistory;
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
    }
}