
namespace GoofED
{
    partial class AsmManagerNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsmManagerNew));
            this.label1 = new System.Windows.Forms.Label();
            this.pluginListbox = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.codeTextbox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.removeselectedButton = new System.Windows.Forms.Button();
            this.addnewButton = new System.Windows.Forms.Button();
            this.addexistingButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginFileFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "List of Plugins used";
            // 
            // pluginListbox
            // 
            this.pluginListbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.pluginListbox.FormattingEnabled = true;
            this.pluginListbox.Location = new System.Drawing.Point(0, 13);
            this.pluginListbox.Name = "pluginListbox";
            this.pluginListbox.Size = new System.Drawing.Size(221, 472);
            this.pluginListbox.TabIndex = 1;
            this.pluginListbox.SelectedIndexChanged += new System.EventHandler(this.pluginListbox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.codeTextbox);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(990, 562);
            this.panel1.TabIndex = 2;
            // 
            // codeTextbox
            // 
            this.codeTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeTextbox.Location = new System.Drawing.Point(221, 0);
            this.codeTextbox.Multiline = true;
            this.codeTextbox.Name = "codeTextbox";
            this.codeTextbox.Size = new System.Drawing.Size(769, 562);
            this.codeTextbox.TabIndex = 4;
            this.codeTextbox.TextChanged += new System.EventHandler(this.codeTextbox_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.removeselectedButton);
            this.panel2.Controls.Add(this.addnewButton);
            this.panel2.Controls.Add(this.addexistingButton);
            this.panel2.Controls.Add(this.pluginListbox);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(221, 562);
            this.panel2.TabIndex = 3;
            // 
            // removeselectedButton
            // 
            this.removeselectedButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.removeselectedButton.Location = new System.Drawing.Point(0, 531);
            this.removeselectedButton.Name = "removeselectedButton";
            this.removeselectedButton.Size = new System.Drawing.Size(221, 23);
            this.removeselectedButton.TabIndex = 4;
            this.removeselectedButton.Text = "Remove selected plugin from project";
            this.removeselectedButton.UseVisualStyleBackColor = true;
            this.removeselectedButton.Click += new System.EventHandler(this.removeselectedButton_Click);
            // 
            // addnewButton
            // 
            this.addnewButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.addnewButton.Location = new System.Drawing.Point(0, 508);
            this.addnewButton.Name = "addnewButton";
            this.addnewButton.Size = new System.Drawing.Size(221, 23);
            this.addnewButton.TabIndex = 3;
            this.addnewButton.Text = "Add new plugin to project";
            this.addnewButton.UseVisualStyleBackColor = true;
            this.addnewButton.Click += new System.EventHandler(this.addnewButton_Click);
            // 
            // addexistingButton
            // 
            this.addexistingButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.addexistingButton.Location = new System.Drawing.Point(0, 485);
            this.addexistingButton.Name = "addexistingButton";
            this.addexistingButton.Size = new System.Drawing.Size(221, 23);
            this.addexistingButton.TabIndex = 2;
            this.addexistingButton.Text = "Add existing plugin to project";
            this.addexistingButton.UseVisualStyleBackColor = true;
            this.addexistingButton.Click += new System.EventHandler(this.addexistingButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 595);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(687, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(903, 598);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(822, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(990, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pluginFileFormatToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // pluginFileFormatToolStripMenuItem
            // 
            this.pluginFileFormatToolStripMenuItem.Name = "pluginFileFormatToolStripMenuItem";
            this.pluginFileFormatToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.pluginFileFormatToolStripMenuItem.Text = "Plugin File Format";
            this.pluginFileFormatToolStripMenuItem.Click += new System.EventHandler(this.pluginFileFormatToolStripMenuItem_Click);
            // 
            // AsmManagerNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 629);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AsmManagerNew";
            this.Text = "Asm Manager New";
            this.Load += new System.EventHandler(this.AsmManagerNew_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox pluginListbox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox codeTextbox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button addnewButton;
        private System.Windows.Forms.Button addexistingButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button removeselectedButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginFileFormatToolStripMenuItem;
    }
}