
namespace GoofED
{
    partial class TextEditor
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
            this.messagesListbox = new System.Windows.Forms.ListBox();
            this.messagePicturebox = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.messagePicturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // messagesListbox
            // 
            this.messagesListbox.Dock = System.Windows.Forms.DockStyle.Left;
            this.messagesListbox.FormattingEnabled = true;
            this.messagesListbox.Location = new System.Drawing.Point(0, 0);
            this.messagesListbox.Name = "messagesListbox";
            this.messagesListbox.Size = new System.Drawing.Size(267, 529);
            this.messagesListbox.TabIndex = 0;
            this.messagesListbox.SelectedIndexChanged += new System.EventHandler(this.messagesListbox_SelectedIndexChanged);
            // 
            // messagePicturebox
            // 
            this.messagePicturebox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.messagePicturebox.Location = new System.Drawing.Point(273, 12);
            this.messagePicturebox.Name = "messagePicturebox";
            this.messagePicturebox.Size = new System.Drawing.Size(416, 128);
            this.messagePicturebox.TabIndex = 1;
            this.messagePicturebox.TabStop = false;
            this.messagePicturebox.Paint += new System.Windows.Forms.PaintEventHandler(this.messagePicturebox_Paint);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(273, 205);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(416, 116);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(609, 494);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(528, 494);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 324);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "Use a normal jump line (enter) to go to next line\r\nuse the character  >  to make " +
    "new line and wait for key before continuing";
            // 
            // upButton
            // 
            this.upButton.Enabled = false;
            this.upButton.Location = new System.Drawing.Point(663, 151);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(26, 23);
            this.upButton.TabIndex = 6;
            this.upButton.Text = "▲";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // downButton
            // 
            this.downButton.Enabled = false;
            this.downButton.Location = new System.Drawing.Point(663, 176);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(26, 23);
            this.downButton.TabIndex = 7;
            this.downButton.Text = "▼";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 529);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.messagePicturebox);
            this.Controls.Add(this.messagesListbox);
            this.Name = "TextEditor";
            this.Text = "TextEditor";
            this.Load += new System.EventHandler(this.TextEditor_Load);
            this.Shown += new System.EventHandler(this.TextEditor_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.messagePicturebox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox messagesListbox;
        private System.Windows.Forms.PictureBox messagePicturebox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
    }
}