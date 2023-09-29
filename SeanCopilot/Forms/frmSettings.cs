using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            LoadInstructions();
            LoadConfigurations();
        }

        private void btnInstructionSave_Click(object sender, EventArgs e)
        {
            Main.SetInstructions(txtInstructions.Text);
        }

        private void btnInstructionReset_Click(object sender, EventArgs e)
        {
            LoadInstructions();
        }

        private void btnConfigurationSave_Click(object sender, EventArgs e)
        {
            Main.configManager.SetConfigValue("api_key", txtApiKey.Text);
            Main.configManager.SetConfigValue("gpt_model", Convert.ToString(cmbGptModel.SelectedItem));
        }

        private void btnConfigurationReset_Click(object sender, EventArgs e)
        {
            LoadConfigurations();
        }

        private void LoadConfigurations()
        {
            txtApiKey.Text = Main.configManager.GetConfigValue("api_key", placeholders: false);

            RefreshGPTModelOptions();
            cmbGptModel.SelectedItem = Main.configManager.GetConfigValue("gpt_model", placeholders: false);
        }

        private void LoadInstructions()
        {
            txtInstructions.Text = Main.GetInstructions();
        }

        private void RefreshGPTModelOptions()
        {
            cmbGptModel.Items.Clear();
            cmbGptModel.Items.AddRange(GetGPTModels());
        }


        public string[] GetGPTModels()
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
                    foreach(object model in data)
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
