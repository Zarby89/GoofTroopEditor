
namespace GoofED
{
    partial class PasswordEditor
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
            this.levelCombobox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.down5Button = new System.Windows.Forms.Button();
            this.down4Button = new System.Windows.Forms.Button();
            this.down3Button = new System.Windows.Forms.Button();
            this.down2Button = new System.Windows.Forms.Button();
            this.down1Button = new System.Windows.Forms.Button();
            this.up5Button = new System.Windows.Forms.Button();
            this.up4Button = new System.Windows.Forms.Button();
            this.up3Button = new System.Windows.Forms.Button();
            this.up2Button = new System.Windows.Forms.Button();
            this.up1Button = new System.Windows.Forms.Button();
            this.passwordBox = new System.Windows.Forms.PictureBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passwordBox)).BeginInit();
            this.SuspendLayout();
            // 
            // levelCombobox
            // 
            this.levelCombobox.FormattingEnabled = true;
            this.levelCombobox.Items.AddRange(new object[] {
            "0x01 Cave",
            "0x02 Castle",
            "0x03 Cavern",
            "0x04 Ship"});
            this.levelCombobox.Location = new System.Drawing.Point(15, 25);
            this.levelCombobox.Name = "levelCombobox";
            this.levelCombobox.Size = new System.Drawing.Size(247, 21);
            this.levelCombobox.TabIndex = 0;
            this.levelCombobox.SelectedIndexChanged += new System.EventHandler(this.levelCombobox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Level : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.down5Button);
            this.groupBox1.Controls.Add(this.down4Button);
            this.groupBox1.Controls.Add(this.down3Button);
            this.groupBox1.Controls.Add(this.down2Button);
            this.groupBox1.Controls.Add(this.down1Button);
            this.groupBox1.Controls.Add(this.up5Button);
            this.groupBox1.Controls.Add(this.up4Button);
            this.groupBox1.Controls.Add(this.up3Button);
            this.groupBox1.Controls.Add(this.up2Button);
            this.groupBox1.Controls.Add(this.up1Button);
            this.groupBox1.Controls.Add(this.passwordBox);
            this.groupBox1.Location = new System.Drawing.Point(15, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 118);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Password";
            // 
            // down5Button
            // 
            this.down5Button.Location = new System.Drawing.Point(204, 87);
            this.down5Button.Name = "down5Button";
            this.down5Button.Size = new System.Drawing.Size(24, 23);
            this.down5Button.TabIndex = 10;
            this.down5Button.Text = "▼";
            this.down5Button.UseVisualStyleBackColor = true;
            this.down5Button.Click += new System.EventHandler(this.down1Button_Click);
            // 
            // down4Button
            // 
            this.down4Button.Location = new System.Drawing.Point(156, 87);
            this.down4Button.Name = "down4Button";
            this.down4Button.Size = new System.Drawing.Size(24, 23);
            this.down4Button.TabIndex = 9;
            this.down4Button.Text = "▼";
            this.down4Button.UseVisualStyleBackColor = true;
            this.down4Button.Click += new System.EventHandler(this.down1Button_Click);
            // 
            // down3Button
            // 
            this.down3Button.Location = new System.Drawing.Point(108, 86);
            this.down3Button.Name = "down3Button";
            this.down3Button.Size = new System.Drawing.Size(24, 23);
            this.down3Button.TabIndex = 8;
            this.down3Button.Text = "▼";
            this.down3Button.UseVisualStyleBackColor = true;
            this.down3Button.Click += new System.EventHandler(this.down1Button_Click);
            // 
            // down2Button
            // 
            this.down2Button.Location = new System.Drawing.Point(60, 86);
            this.down2Button.Name = "down2Button";
            this.down2Button.Size = new System.Drawing.Size(24, 23);
            this.down2Button.TabIndex = 7;
            this.down2Button.Text = "▼";
            this.down2Button.UseVisualStyleBackColor = true;
            this.down2Button.Click += new System.EventHandler(this.down1Button_Click);
            // 
            // down1Button
            // 
            this.down1Button.Location = new System.Drawing.Point(12, 86);
            this.down1Button.Name = "down1Button";
            this.down1Button.Size = new System.Drawing.Size(24, 23);
            this.down1Button.TabIndex = 6;
            this.down1Button.Text = "▼";
            this.down1Button.UseVisualStyleBackColor = true;
            this.down1Button.Click += new System.EventHandler(this.down1Button_Click);
            // 
            // up5Button
            // 
            this.up5Button.Location = new System.Drawing.Point(204, 20);
            this.up5Button.Name = "up5Button";
            this.up5Button.Size = new System.Drawing.Size(24, 23);
            this.up5Button.TabIndex = 5;
            this.up5Button.Text = "▲";
            this.up5Button.UseVisualStyleBackColor = true;
            this.up5Button.Click += new System.EventHandler(this.up1Button_Click_1);
            // 
            // up4Button
            // 
            this.up4Button.Location = new System.Drawing.Point(156, 20);
            this.up4Button.Name = "up4Button";
            this.up4Button.Size = new System.Drawing.Size(24, 23);
            this.up4Button.TabIndex = 4;
            this.up4Button.Text = "▲";
            this.up4Button.UseVisualStyleBackColor = true;
            this.up4Button.Click += new System.EventHandler(this.up1Button_Click_1);
            // 
            // up3Button
            // 
            this.up3Button.Location = new System.Drawing.Point(108, 19);
            this.up3Button.Name = "up3Button";
            this.up3Button.Size = new System.Drawing.Size(24, 23);
            this.up3Button.TabIndex = 3;
            this.up3Button.Text = "▲";
            this.up3Button.UseVisualStyleBackColor = true;
            this.up3Button.Click += new System.EventHandler(this.up1Button_Click_1);
            // 
            // up2Button
            // 
            this.up2Button.Location = new System.Drawing.Point(60, 19);
            this.up2Button.Name = "up2Button";
            this.up2Button.Size = new System.Drawing.Size(24, 23);
            this.up2Button.TabIndex = 2;
            this.up2Button.Text = "▲";
            this.up2Button.UseVisualStyleBackColor = true;
            this.up2Button.Click += new System.EventHandler(this.up1Button_Click_1);
            // 
            // up1Button
            // 
            this.up1Button.Location = new System.Drawing.Point(12, 19);
            this.up1Button.Name = "up1Button";
            this.up1Button.Size = new System.Drawing.Size(24, 23);
            this.up1Button.TabIndex = 1;
            this.up1Button.Text = "▲";
            this.up1Button.UseVisualStyleBackColor = true;
            this.up1Button.Click += new System.EventHandler(this.up1Button_Click_1);
            // 
            // passwordBox
            // 
            this.passwordBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.passwordBox.Location = new System.Drawing.Point(5, 49);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(232, 32);
            this.passwordBox.TabIndex = 0;
            this.passwordBox.TabStop = false;
            this.passwordBox.Paint += new System.Windows.Forms.PaintEventHandler(this.passwordBox_Paint);
            // 
            // acceptButton
            // 
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(187, 180);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            // 
            // PasswordEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 215);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.levelCombobox);
            this.Name = "PasswordEditor";
            this.Text = "PasswordEditor";
            this.Load += new System.EventHandler(this.PasswordEditor_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.passwordBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox levelCombobox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button up1Button;
        private System.Windows.Forms.PictureBox passwordBox;
        private System.Windows.Forms.Button up5Button;
        private System.Windows.Forms.Button up4Button;
        private System.Windows.Forms.Button up3Button;
        private System.Windows.Forms.Button up2Button;
        private System.Windows.Forms.Button down5Button;
        private System.Windows.Forms.Button down4Button;
        private System.Windows.Forms.Button down3Button;
        private System.Windows.Forms.Button down2Button;
        private System.Windows.Forms.Button down1Button;
        private System.Windows.Forms.Button acceptButton;
    }
}