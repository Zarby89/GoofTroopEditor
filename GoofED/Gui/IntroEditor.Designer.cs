namespace GoofTroopEditor.Gui
{
    partial class IntroEditor
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
            this.vramPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.vramPicturebox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tilepropGroupbox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.priorityCheckbox = new System.Windows.Forms.CheckBox();
            this.mirroryCheckbox = new System.Windows.Forms.CheckBox();
            this.palettePicturebox = new System.Windows.Forms.PictureBox();
            this.mirrorxCheckbox = new System.Windows.Forms.CheckBox();
            this.mainPicturebox = new System.Windows.Forms.PictureBox();
            this.vramPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vramPicturebox)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.tilepropGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.palettePicturebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPicturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // vramPanel
            // 
            this.vramPanel.Controls.Add(this.panel2);
            this.vramPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vramPanel.Location = new System.Drawing.Point(384, 0);
            this.vramPanel.Name = "vramPanel";
            this.vramPanel.Size = new System.Drawing.Size(538, 647);
            this.vramPanel.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.vramPicturebox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(538, 647);
            this.panel2.TabIndex = 2;
            // 
            // vramPicturebox
            // 
            this.vramPicturebox.Location = new System.Drawing.Point(6, 3);
            this.vramPicturebox.Name = "vramPicturebox";
            this.vramPicturebox.Size = new System.Drawing.Size(512, 2048);
            this.vramPicturebox.TabIndex = 1;
            this.vramPicturebox.TabStop = false;
            this.vramPicturebox.Paint += new System.Windows.Forms.PaintEventHandler(this.vramPicturebox_Paint);
            this.vramPicturebox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.vramPicturebox_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.numericUpDown2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tilepropGroupbox);
            this.panel1.Controls.Add(this.mainPicturebox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 647);
            this.panel1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(303, 612);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Hexadecimal = true;
            this.numericUpDown2.Location = new System.Drawing.Point(72, 444);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(306, 20);
            this.numericUpDown2.TabIndex = 6;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 446);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Intro Index";
            // 
            // tilepropGroupbox
            // 
            this.tilepropGroupbox.Controls.Add(this.label1);
            this.tilepropGroupbox.Controls.Add(this.numericUpDown1);
            this.tilepropGroupbox.Controls.Add(this.priorityCheckbox);
            this.tilepropGroupbox.Controls.Add(this.mirroryCheckbox);
            this.tilepropGroupbox.Controls.Add(this.palettePicturebox);
            this.tilepropGroupbox.Controls.Add(this.mirrorxCheckbox);
            this.tilepropGroupbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.tilepropGroupbox.Location = new System.Drawing.Point(0, 288);
            this.tilepropGroupbox.Name = "tilepropGroupbox";
            this.tilepropGroupbox.Size = new System.Drawing.Size(384, 150);
            this.tilepropGroupbox.TabIndex = 1;
            this.tilepropGroupbox.TabStop = false;
            this.tilepropGroupbox.Text = "Tiles Properties";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tile Index";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Hexadecimal = true;
            this.numericUpDown1.Location = new System.Drawing.Point(12, 103);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(104, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // priorityCheckbox
            // 
            this.priorityCheckbox.AutoSize = true;
            this.priorityCheckbox.Location = new System.Drawing.Point(12, 65);
            this.priorityCheckbox.Name = "priorityCheckbox";
            this.priorityCheckbox.Size = new System.Drawing.Size(57, 17);
            this.priorityCheckbox.TabIndex = 3;
            this.priorityCheckbox.Text = "Priority";
            this.priorityCheckbox.UseVisualStyleBackColor = true;
            this.priorityCheckbox.CheckedChanged += new System.EventHandler(this.mirrorxCheckbox_CheckedChanged);
            // 
            // mirroryCheckbox
            // 
            this.mirroryCheckbox.AutoSize = true;
            this.mirroryCheckbox.Location = new System.Drawing.Point(12, 42);
            this.mirroryCheckbox.Name = "mirroryCheckbox";
            this.mirroryCheckbox.Size = new System.Drawing.Size(62, 17);
            this.mirroryCheckbox.TabIndex = 2;
            this.mirroryCheckbox.Text = "Mirror Y";
            this.mirroryCheckbox.UseVisualStyleBackColor = true;
            this.mirroryCheckbox.CheckedChanged += new System.EventHandler(this.mirrorxCheckbox_CheckedChanged);
            // 
            // palettePicturebox
            // 
            this.palettePicturebox.Location = new System.Drawing.Point(122, 17);
            this.palettePicturebox.Name = "palettePicturebox";
            this.palettePicturebox.Size = new System.Drawing.Size(256, 128);
            this.palettePicturebox.TabIndex = 1;
            this.palettePicturebox.TabStop = false;
            this.palettePicturebox.Paint += new System.Windows.Forms.PaintEventHandler(this.palettePicturebox_Paint);
            this.palettePicturebox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.palettePicturebox_MouseDown);
            // 
            // mirrorxCheckbox
            // 
            this.mirrorxCheckbox.AutoSize = true;
            this.mirrorxCheckbox.Location = new System.Drawing.Point(12, 19);
            this.mirrorxCheckbox.Name = "mirrorxCheckbox";
            this.mirrorxCheckbox.Size = new System.Drawing.Size(62, 17);
            this.mirrorxCheckbox.TabIndex = 0;
            this.mirrorxCheckbox.Text = "Mirror X";
            this.mirrorxCheckbox.UseVisualStyleBackColor = true;
            this.mirrorxCheckbox.CheckedChanged += new System.EventHandler(this.mirrorxCheckbox_CheckedChanged);
            // 
            // mainPicturebox
            // 
            this.mainPicturebox.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainPicturebox.Location = new System.Drawing.Point(0, 0);
            this.mainPicturebox.Name = "mainPicturebox";
            this.mainPicturebox.Size = new System.Drawing.Size(384, 288);
            this.mainPicturebox.TabIndex = 0;
            this.mainPicturebox.TabStop = false;
            this.mainPicturebox.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPicturebox_Paint);
            this.mainPicturebox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainPicturebox_MouseDown);
            this.mainPicturebox.MouseLeave += new System.EventHandler(this.mainPicturebox_MouseLeave);
            this.mainPicturebox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainPicturebox_MouseMove);
            this.mainPicturebox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mainPicturebox_MouseUp);
            // 
            // IntroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 647);
            this.Controls.Add(this.vramPanel);
            this.Controls.Add(this.panel1);
            this.Name = "IntroEditor";
            this.Text = "Intro Editor";
            this.Load += new System.EventHandler(this.IntroEditor_Load);
            this.vramPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vramPicturebox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.tilepropGroupbox.ResumeLayout(false);
            this.tilepropGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.palettePicturebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPicturebox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mainPicturebox;
        private System.Windows.Forms.Panel vramPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox vramPicturebox;
        private System.Windows.Forms.GroupBox tilepropGroupbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox priorityCheckbox;
        private System.Windows.Forms.CheckBox mirroryCheckbox;
        private System.Windows.Forms.PictureBox palettePicturebox;
        private System.Windows.Forms.CheckBox mirrorxCheckbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button button1;
    }
}