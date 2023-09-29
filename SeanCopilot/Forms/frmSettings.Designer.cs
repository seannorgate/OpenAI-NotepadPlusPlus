namespace Kbg.NppPluginNET
{
    partial class frmSettings
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
            this.txtInstructions = new System.Windows.Forms.TextBox();
            this.btnInstructionSave = new System.Windows.Forms.Button();
            this.btnInstructionReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConfigurationReset = new System.Windows.Forms.Button();
            this.btnConfigurationSave = new System.Windows.Forms.Button();
            this.cmbGptModel = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInstructions
            // 
            this.txtInstructions.AcceptsReturn = true;
            this.txtInstructions.Location = new System.Drawing.Point(12, 157);
            this.txtInstructions.Multiline = true;
            this.txtInstructions.Name = "txtInstructions";
            this.txtInstructions.Size = new System.Drawing.Size(300, 300);
            this.txtInstructions.TabIndex = 0;
            // 
            // btnInstructionSave
            // 
            this.btnInstructionSave.Location = new System.Drawing.Point(15, 463);
            this.btnInstructionSave.Name = "btnInstructionSave";
            this.btnInstructionSave.Size = new System.Drawing.Size(140, 23);
            this.btnInstructionSave.TabIndex = 1;
            this.btnInstructionSave.Text = "Save";
            this.btnInstructionSave.UseVisualStyleBackColor = true;
            this.btnInstructionSave.Click += new System.EventHandler(this.btnInstructionSave_Click);
            // 
            // btnInstructionReset
            // 
            this.btnInstructionReset.Location = new System.Drawing.Point(169, 463);
            this.btnInstructionReset.Name = "btnInstructionReset";
            this.btnInstructionReset.Size = new System.Drawing.Size(140, 23);
            this.btnInstructionReset.TabIndex = 2;
            this.btnInstructionReset.Text = "Reload";
            this.btnInstructionReset.UseVisualStyleBackColor = true;
            this.btnInstructionReset.Click += new System.EventHandler(this.btnInstructionReset_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Copilot instructions:";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(87, 35);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(218, 20);
            this.txtApiKey.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "API Key:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "GPT Model:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.cmbGptModel);
            this.groupBox1.Controls.Add(this.btnConfigurationReset);
            this.groupBox1.Controls.Add(this.btnConfigurationSave);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 118);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Open AI Configuration:";
            // 
            // btnConfigurationReset
            // 
            this.btnConfigurationReset.Location = new System.Drawing.Point(157, 83);
            this.btnConfigurationReset.Name = "btnConfigurationReset";
            this.btnConfigurationReset.Size = new System.Drawing.Size(136, 23);
            this.btnConfigurationReset.TabIndex = 10;
            this.btnConfigurationReset.Text = "Reload";
            this.btnConfigurationReset.UseVisualStyleBackColor = true;
            this.btnConfigurationReset.Click += new System.EventHandler(this.btnConfigurationReset_Click);
            // 
            // btnConfigurationSave
            // 
            this.btnConfigurationSave.Location = new System.Drawing.Point(6, 83);
            this.btnConfigurationSave.Name = "btnConfigurationSave";
            this.btnConfigurationSave.Size = new System.Drawing.Size(137, 23);
            this.btnConfigurationSave.TabIndex = 9;
            this.btnConfigurationSave.Text = "Save";
            this.btnConfigurationSave.UseVisualStyleBackColor = true;
            this.btnConfigurationSave.Click += new System.EventHandler(this.btnConfigurationSave_Click);
            // 
            // cmbGptModel
            // 
            this.cmbGptModel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbGptModel.FormattingEnabled = true;
            this.cmbGptModel.Location = new System.Drawing.Point(75, 49);
            this.cmbGptModel.Name = "cmbGptModel";
            this.cmbGptModel.Size = new System.Drawing.Size(218, 21);
            this.cmbGptModel.TabIndex = 9;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 681);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInstructionReset);
            this.Controls.Add(this.btnInstructionSave);
            this.Controls.Add(this.txtInstructions);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmSettings";
            this.Text = "frmSettings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInstructions;
        private System.Windows.Forms.Button btnInstructionSave;
        private System.Windows.Forms.Button btnInstructionReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConfigurationReset;
        private System.Windows.Forms.Button btnConfigurationSave;
        private System.Windows.Forms.ComboBox cmbGptModel;
    }
}