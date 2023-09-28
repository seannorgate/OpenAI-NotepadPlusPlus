using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.Net;
using System.Web.Script.Serialization;

namespace Kbg.NppPluginNET
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            LoadInstructions();
        }

        private void LoadInstructions()
        {
            if (File.Exists(Main.INSTRUCTION_FILEPATH))
            {
                try
                {
                    txtInstructions.Text = File.ReadAllText(Main.INSTRUCTION_FILEPATH);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while reading the file: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("File instructions.ini does not exist.");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(Main.INSTRUCTION_FILEPATH, txtInstructions.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the file: {ex.Message}");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadInstructions();
        }
    }
}
