using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Kbg.NppPluginNET
{

    public partial class frmCopilotDlg : Form
    {
        private OpenAI openAI;

        public frmCopilotDlg()
        {
            InitializeComponent();
            CheckAPIKey();

            openAI = new OpenAI();
        }

        private void frmCopilotDlg_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void frmCopilotDlg_GetFocus(object sender, EventArgs e)
        {
            CheckAPIKey();
            textBox1.Focus();
        }

        private void frmCopilotDlg_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the pressed key is the "Return" key
            if (e.KeyCode == Keys.Return && enterToRunCheckbox.Checked)
            {
                // Activate the button click event or call your function here
                refactorButton.PerformClick();
            }
        }


        private void clearButton_Click(object sender, EventArgs e)
        {
            openAI.ClearHistory();

            textBox1.Text = string.Empty;
            textGPTResponse.Text = string.Empty;
            readThisDocButton.Enabled = true;
            UpdateHistoryCountOnCheckbox();

            lblResponse.Hide();
            textGPTResponse.Hide();
        }

        private void refactorButton_Click(object sender, EventArgs e)
        {
            // get selected text
            var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            string selectionText = scintilla.GetSelText();

            SendToGPT(selectionText);
        }

        private void readThisDocButton_Click(object sender, EventArgs e)
        {
            // get selected text
            var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            string allText = scintilla.GetText(scintilla.GetTextLength());
            readThisDocButton.Enabled = false;

            SendToGPT(RemoveLineBreaks(allText), false);
        }

        private void textGPTResponse_TextChanged(object sender, EventArgs e)
        {
            UpdateHistoryCountOnCheckbox();
        }

        private void UpdateHistoryCountOnCheckbox()
        {
            string replacement = Convert.ToString(openAI.GetHistoryLength());

            string newText = maintainHistoryCheckbox.Text;
            if (newText.Contains("(") && newText.Contains(")"))
            {
                int openBracketIndex = newText.LastIndexOf('(');
                int closeBracketIndex = newText.LastIndexOf(')');

                if (openBracketIndex != -1 && closeBracketIndex != -1 && openBracketIndex < closeBracketIndex)
                {
                    newText = newText.Substring(0, openBracketIndex + 1) + replacement + newText.Substring(closeBracketIndex);
                }
                else
                {
                    newText = newText + " (" + replacement + " messages)";
                }
            }
            else
            {
                newText = newText + " (" + replacement + " messages)";
            }

            maintainHistoryCheckbox.Text = newText;
        }

        private void SendToGPT(string selectionText, bool replaceSelection = true)
        {
            Task.Run(() => GetGPTResponse(selectionText, maintainHistoryCheckbox.Checked, replaceSelection));

            lblResponse.Show();
            textGPTResponse.Show();
        }

        public static string RemoveLineBreaks(string input)
        {
            // Replace line breaks and their surrounding whitespace with empty string
            return Regex.Replace(input, @"[\t ]*\r?\n[\t ]*", "");
        }

        private async void GetGPTResponse(string selectionText, bool useHistory, bool replaceSelection)
        {
            string whitespace = ExtractWhitespace(selectionText);

            // UI updates
            textGPTResponse.Text = "Working on it...";
            refactorButton.Enabled = false;

            // make the GPT call
            string prompt = textBox1.Text + "\r\n" + selectionText;
            string[] response = await openAI.RunGPTFunction(prompt, useHistory);

            // replace the selected text if necessary
            if (response[0].Length > 0 && replaceSelection)
            {
                var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
                scintilla.ReplaceSel(InsertWhitespace(response[0], whitespace));
            }

            // update the UI
            refactorButton.Enabled = true;
            textGPTResponse.Text = response[1];
        }

        private static string InsertWhitespace(string responseCode, string whitespace)
        {
            string newCode = responseCode;
            if (!String.IsNullOrEmpty(whitespace))
            {
                newCode = String.Join("\n", newCode.Split('\n').Select(line => whitespace + line));
            }

            return newCode;
        }

        public string ExtractWhitespace(string selectionText)
        {
            string whitespace = null;
            if (selectionText.Contains("\n"))
            {
                var lines = selectionText.Split('\n');
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    whitespace = line.Length > 0 ? line.Substring(0, line.Length - line.TrimStart().Length) : null;
                    break;
                }
            }
            else
            {
                whitespace = selectionText.Length > 0 ? selectionText.Substring(0, selectionText.Length - selectionText.TrimStart().Length) : null;
            }
            return whitespace;
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

                readThisDocButton.Enabled = false;
                refactorButton.Enabled = false;
            }
            else
            {
                if (textBox1.Text == noApiKeyString)
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
            clearButton.Enabled = enabledForm;
            maintainHistoryCheckbox.Enabled = enabledForm;
            enterToRunCheckbox.Enabled = enabledForm;
        }
    }
}
