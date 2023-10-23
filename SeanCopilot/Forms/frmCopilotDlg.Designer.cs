using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    partial class frmCopilotDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.refactorButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.lblResponse = new System.Windows.Forms.Label();
            this.textGPTResponse = new System.Windows.Forms.TextBox();
            this.readThisDocButton = new System.Windows.Forms.Button();
            this.maintainHistoryCheckbox = new System.Windows.Forms.CheckBox();
            this.enterToRunCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.CausesValidation = false;
            this.textBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox1.Location = new System.Drawing.Point(12, 26);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(300, 231);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Request for Copilot:";
            // 
            // refactorButton
            // 
            this.refactorButton.Location = new System.Drawing.Point(15, 309);
            this.refactorButton.Name = "refactorButton";
            this.refactorButton.Size = new System.Drawing.Size(140, 23);
            this.refactorButton.TabIndex = 1;
            this.refactorButton.Text = "Run";
            this.refactorButton.UseVisualStyleBackColor = true;
            this.refactorButton.Click += new System.EventHandler(this.refactorButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(169, 309);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(140, 23);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Clear History";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(15, 371);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(106, 13);
            this.lblResponse.TabIndex = 3;
            this.lblResponse.Text = "Copilot\'s explanation:";
            this.lblResponse.Visible = false;
            // 
            // textGPTResponse
            // 
            this.textGPTResponse.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textGPTResponse.CausesValidation = false;
            this.textGPTResponse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textGPTResponse.Location = new System.Drawing.Point(12, 387);
            this.textGPTResponse.Multiline = true;
            this.textGPTResponse.Name = "textGPTResponse";
            this.textGPTResponse.ReadOnly = true;
            this.textGPTResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textGPTResponse.Size = new System.Drawing.Size(300, 231);
            this.textGPTResponse.TabIndex = 0;
            this.textGPTResponse.Visible = false;
            this.textGPTResponse.TextChanged += new System.EventHandler(this.textGPTResponse_TextChanged);
            // 
            // readThisDocButton
            // 
            this.readThisDocButton.Location = new System.Drawing.Point(15, 338);
            this.readThisDocButton.Name = "readThisDocButton";
            this.readThisDocButton.Size = new System.Drawing.Size(294, 23);
            this.readThisDocButton.TabIndex = 2;
            this.readThisDocButton.Text = "Read this Document";
            this.readThisDocButton.UseVisualStyleBackColor = true;
            this.readThisDocButton.Click += new System.EventHandler(this.readThisDocButton_Click);
            // 
            // maintainHistoryCheckbox
            // 
            this.maintainHistoryCheckbox.AutoSize = true;
            this.maintainHistoryCheckbox.Checked = true;
            this.maintainHistoryCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.maintainHistoryCheckbox.Location = new System.Drawing.Point(15, 286);
            this.maintainHistoryCheckbox.Name = "maintainHistoryCheckbox";
            this.maintainHistoryCheckbox.Size = new System.Drawing.Size(209, 17);
            this.maintainHistoryCheckbox.TabIndex = 4;
            this.maintainHistoryCheckbox.Text = "Maintain message history (0 messages)";
            this.maintainHistoryCheckbox.UseVisualStyleBackColor = true;
            // 
            // enterToRunCheckbox
            // 
            this.enterToRunCheckbox.AutoSize = true;
            this.enterToRunCheckbox.Checked = true;
            this.enterToRunCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enterToRunCheckbox.Location = new System.Drawing.Point(15, 264);
            this.enterToRunCheckbox.Name = "enterToRunCheckbox";
            this.enterToRunCheckbox.Size = new System.Drawing.Size(109, 17);
            this.enterToRunCheckbox.TabIndex = 4;
            this.enterToRunCheckbox.Text = "Press enter to run";
            this.enterToRunCheckbox.UseVisualStyleBackColor = true;
            // 
            // frmCopilotDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 692);
            this.Controls.Add(this.enterToRunCheckbox);
            this.Controls.Add(this.maintainHistoryCheckbox);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.readThisDocButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.refactorButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textGPTResponse);
            this.Controls.Add(this.textBox1);
            this.KeyPreview = true;
            this.Name = "frmCopilotDlg";
            this.Text = "frmCopilotDlg";
            this.Load += new System.EventHandler(this.frmCopilotDlg_Load);
            this.GotFocus += new System.EventHandler(this.frmCopilotDlg_GetFocus);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCopilotDlg_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button refactorButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.TextBox textGPTResponse;
        private Button readThisDocButton;
        private CheckBox maintainHistoryCheckbox;
        private CheckBox enterToRunCheckbox;
    }
}