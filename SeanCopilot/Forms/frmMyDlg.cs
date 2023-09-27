using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;

namespace Kbg.NppPluginNET
{

    public partial class frmMyDlg : Form
    {
        WebClient webClient;

        public frmMyDlg()
        {
            InitializeComponent();

            // create web client
            webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("Authorization", "Bearer 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");

            // Set the security protocol
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

        }

        public string GetGPTResponse(string prompt)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.openai.com/v1/chat/completions");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer YOUR_OPENAI_API_KEY_HERE");

            var messages = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "role", "system" }, { "content", "You are a helpful assistant." } },
                new Dictionary<string, string> { { "role", "user" }, { "content", "Hello!" } }
            };

                    var payload = new Dictionary<string, object>
            {
                { "model", "gpt-3.5-turbo" },
                { "messages", messages }
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string postData = serializer.Serialize(payload);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Dictionary<string, object> parsedResponse = serializer.Deserialize<Dictionary<string, object>>(result);
                object[] choices = (object[])parsedResponse["choices"];
                Dictionary<string, object> choice = (Dictionary<string, object>)choices[0];
                Dictionary<string, object> message = (Dictionary<string, object>)choice["message"];
                return message["content"].ToString();
            }
        }

    private void clearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void refactorButton_Click(object sender, EventArgs e)
        {
            var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            string prompt = textBox1.Text + "\r\n" + scintilla.GetSelText();

            textGPTResponse.Text = GetGPTResponse(prompt);
        }
    }
}
