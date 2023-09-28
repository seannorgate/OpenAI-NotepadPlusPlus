namespace Kbg.NppPluginNET
{
    partial class frmMyDlg
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
            this.label2 = new System.Windows.Forms.Label();
            this.textGPTResponse = new System.Windows.Forms.TextBox();
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
            this.textBox1.Size = new System.Drawing.Size(286, 231);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Refactor instructions:";
            // 
            // refactorButton
            // 
            this.refactorButton.Location = new System.Drawing.Point(15, 263);
            this.refactorButton.Name = "refactorButton";
            this.refactorButton.Size = new System.Drawing.Size(140, 23);
            this.refactorButton.TabIndex = 1;
            this.refactorButton.Text = "Refactor";
            this.refactorButton.UseVisualStyleBackColor = true;
            this.refactorButton.Click += new System.EventHandler(this.refactorButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(155, 263);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(140, 23);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Response:";
            // 
            // textGPTResponse
            // 
            this.textGPTResponse.AcceptsReturn = true;
            this.textGPTResponse.AllowDrop = true;
            this.textGPTResponse.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textGPTResponse.CausesValidation = false;
            this.textGPTResponse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textGPTResponse.Location = new System.Drawing.Point(12, 319);
            this.textGPTResponse.Multiline = true;
            this.textGPTResponse.Name = "textGPTResponse";
            this.textGPTResponse.ReadOnly = true;
            this.textGPTResponse.Size = new System.Drawing.Size(286, 231);
            this.textGPTResponse.TabIndex = 0;
            // 
            // frmMyDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 557);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.refactorButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textGPTResponse);
            this.Controls.Add(this.textBox1);
            this.KeyPreview = true;
            this.Name = "frmMyDlg";
            this.Text = "frmMyDlg";
            this.Load += new System.EventHandler(this.frmMyDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button refactorButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textGPTResponse;
    }
}