
namespace GoofED
{
    partial class AsmHelp
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
            this.asmhelpRichtextbox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // asmhelpRichtextbox
            // 
            this.asmhelpRichtextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.asmhelpRichtextbox.Location = new System.Drawing.Point(0, 0);
            this.asmhelpRichtextbox.Name = "asmhelpRichtextbox";
            this.asmhelpRichtextbox.ReadOnly = true;
            this.asmhelpRichtextbox.Size = new System.Drawing.Size(590, 516);
            this.asmhelpRichtextbox.TabIndex = 1;
            this.asmhelpRichtextbox.Text = "";
            // 
            // AsmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 516);
            this.Controls.Add(this.asmhelpRichtextbox);
            this.Name = "AsmHelp";
            this.Text = "Asm Help";
            this.Load += new System.EventHandler(this.AsmHelp_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox asmhelpRichtextbox;
    }
}