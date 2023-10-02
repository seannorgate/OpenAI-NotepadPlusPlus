using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            cmbGptModel.Items.AddRange(OpenAI.GetGPTModels());
        }

        private void linkBilling_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkBilling.LinkVisited = true;
            Process.Start("https://platform.openai.com/account/billing/overview");
        }
    }
}
