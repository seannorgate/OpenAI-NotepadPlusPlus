using System;
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
            Main.configManager.SetConfigValue("gpt_model", txtGptModel.Text);
        }

        private void btnConfigurationReset_Click(object sender, EventArgs e)
        {
            LoadConfigurations();
        }

        private void LoadConfigurations()
        {
            txtApiKey.Text = Main.configManager.GetConfigValue("api_key", placeholders: false);
            txtGptModel.Text = Main.configManager.GetConfigValue("gpt_model", placeholders: false);
        }

        private void LoadInstructions()
        {
            txtInstructions.Text = Main.GetInstructions();
        }
    }
}
