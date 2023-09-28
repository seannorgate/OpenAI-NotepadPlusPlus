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
        private string systemInstruction;

        public frmMyDlg()
        {
            InitializeComponent();

            try
            {
                if (File.Exists(Main.INSTRUCTION_FILEPATH))
                {
                    systemInstruction = File.ReadAllText(Main.INSTRUCTION_FILEPATH);
                }
                else
                {
                    MessageBox.Show("INI file not found in " + Main.INSTRUCTION_FILEPATH);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void frmMyDlg_Load(object sender, EventArgs e)
        {
            // Attach the KeyDown event to the form
            this.KeyDown += new KeyEventHandler(frmMyDlg_KeyDown);
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
            request.Headers.Add("Authorization", "Bearer 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");
            request.UseDefaultCredentials = false;

            var messages = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "role", "system" }, { "content", systemInstruction } },
                new Dictionary<string, string> { { "role", "user" }, { "content", prompt } }
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
            textBox1.Text = "";
            textGPTResponse.Text = systemInstruction;
        }

        private void refactorButton_Click(object sender, EventArgs e)
        {
            var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            string prompt = textBox1.Text + "\r\n" + scintilla.GetSelText();

            string[] response = GetGPTResponse(prompt);

            if(response[0].Length > 0)
                scintilla.ReplaceSel(response[0]);
            
            textGPTResponse.Text = response[1];
        }
    }
}
