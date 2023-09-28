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
        public frmMyDlg()
        {
            InitializeComponent();
            CheckAPIKey();
        }

        private void CheckAPIKey()
        {
            bool enabledForm = Main.configManager.GetConfigValue("api_key").Length > 0;
            string noApiKeyString = "No API key found!";

            if (!enabledForm)
            {
                textGPTResponse.Text = string.Empty;
                textGPTResponse.Hide();
                lblResponse.Hide();
                textBox1.Text = noApiKeyString;
            }
            else
            {
                if(textBox1.Text == noApiKeyString)
                {
                    textBox1.Text = string.Empty;
                }

                if (textGPTResponse.Text.Length > 0)
                {
                    lblResponse.Show();
                    textGPTResponse.Show();
                }
            }

            textBox1.Enabled = enabledForm;
            refactorButton.Enabled = enabledForm;
            clearButton.Enabled = enabledForm;
        }

        private void frmMyDlg_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the pressed key is the "Return" key
            if (e.KeyCode == Keys.Return)
            {
                // Activate the button click event or call your function here
                refactorButton.PerformClick();
            }
        }


        public string[] GetGPTResponse(string prompt)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create("https://api.openai.com/v1/chat/completions");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + Main.configManager.GetConfigValue("api_key"));
            request.UseDefaultCredentials = false;

            var messages = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "role", "system" }, { "content", Main.GetInstructions() } },
                new Dictionary<string, string> { { "role", "user" }, { "content", prompt } }
            };

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
                    try
                    {
                        Dictionary<string, object> answers = serializer.Deserialize<Dictionary<string, object>>(message["content"].ToString());
                        return new string[] { answers["code"].ToString(), answers["reason"].ToString() };
                    }
                    catch(Exception e)
                    {
                        return new string[] { string.Empty, e.Message + "\r\nCannot refactor, but my response is:\r\n" + message["content"].ToString() };
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exception
                return new string[] { string.Empty, e.Message };
            }
        }


        private void clearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textGPTResponse.Text = string.Empty;
            lblResponse.Hide();
            textGPTResponse.Hide();
        }

        private void refactorButton_Click(object sender, EventArgs e)
        {
            textGPTResponse.Text = string.Empty;

            var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            string prompt = textBox1.Text + "\r\n" + scintilla.GetSelText();

            string[] response = GetGPTResponse(prompt);

            if(response[0].Length > 0)
                scintilla.ReplaceSel(response[0]);

            lblResponse.Show();
            textGPTResponse.Show();
            textGPTResponse.Text = response[1];
        }

        private void frmMyDlg_GetFocus(object sender, EventArgs e)
        {
            CheckAPIKey();
        }
    }
}
