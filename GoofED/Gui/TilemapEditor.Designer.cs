namespace GoofTroopEditor.Gui
{
    partial class TilemapEditor
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.mainPicturebox = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.zoomUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tilepropGroupbox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.priorityCheckbox = new System.Windows.Forms.CheckBox();
            this.mirroryCheckbox = new System.Windows.Forms.CheckBox();
            this.palettePicturebox = new System.Windows.Forms.PictureBox();
            this.mirrorxCheckbox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.vramPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vramPicturebox)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPicturebox)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoomUpDown)).BeginInit();
            this.tilepropGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.palettePicturebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // vramPanel
            // 
            this.vramPanel.Controls.Add(this.panel2);
            this.vramPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vramPanel.Location = new System.Drawing.Point(1049, 0);
            this.vramPanel.Name = "vramPanel";
            this.vramPanel.Size = new System.Drawing.Size(535, 647);
            this.vramPanel.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.vramPicturebox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(535, 647);
            this.panel2.TabIndex = 2;
            // 
            // vramPicturebox
            // 
            this.vramPicturebox.Location = new System.Drawing.Point(6, -3);
            this.vramPicturebox.Name = "vramPicturebox";
            this.vramPicturebox.Size = new System.Drawing.Size(512, 2048);
            this.vramPicturebox.TabIndex = 1;
            this.vramPicturebox.TabStop = false;
            this.vramPicturebox.Paint += new System.Windows.Forms.PaintEventHandler(this.vramPicturebox_Paint);
            this.vramPicturebox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.vramPicturebox_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1049, 647);
            this.panel1.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.AutoScroll = true;
            this.panel4.Controls.Add(this.mainPicturebox);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 155);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1049, 492);
            this.panel4.TabIndex = 9;
            // 
            // mainPicturebox
            // 
            this.mainPicturebox.Location = new System.Drawing.Point(3, 3);
            this.mainPicturebox.Name = "mainPicturebox";
            this.mainPicturebox.Size = new System.Drawing.Size(1024, 1024);
            this.mainPicturebox.TabIndex = 0;
            this.mainPicturebox.TabStop = false;
            this.mainPicturebox.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPicturebox_Paint);
            this.mainPicturebox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainPicturebox_MouseDown);
            this.mainPicturebox.MouseLeave += new System.EventHandler(this.mainPicturebox_MouseLeave);
            this.mainPicturebox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainPicturebox_MouseMove);
            this.mainPicturebox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mainPicturebox_MouseUp);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.zoomUpDown);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.tilepropGroupbox);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.numericUpDown2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1049, 155);
            this.panel3.TabIndex = 8;
            // 
            // zoomUpDown
            // 
            this.zoomUpDown.Hexadecimal = true;
            this.zoomUpDown.Location = new System.Drawing.Point(91, 35);
            this.zoomUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.zoomUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.zoomUpDown.Name = "zoomUpDown";
            this.zoomUpDown.Size = new System.Drawing.Size(90, 20);
            this.zoomUpDown.TabIndex = 9;
            this.zoomUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.zoomUpDown.ValueChanged += new System.EventHandler(this.zoomUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Zoom";
            // 
            // tilepropGroupbox
            // 
            this.tilepropGroupbox.Controls.Add(this.label1);
            this.tilepropGroupbox.Controls.Add(this.numericUpDown1);
            this.tilepropGroupbox.Controls.Add(this.priorityCheckbox);
            this.tilepropGroupbox.Controls.Add(this.mirroryCheckbox);
            this.tilepropGroupbox.Controls.Add(this.palettePicturebox);
            this.tilepropGroupbox.Controls.Add(this.mirrorxCheckbox);
            this.tilepropGroupbox.Location = new System.Drawing.Point(667, 3);
            this.tilepropGroupbox.Name = "tilepropGroupbox";
            this.tilepropGroupbox.Size = new System.Drawing.Size(376, 150);
            this.tilepropGroupbox.TabIndex = 1;
            this.tilepropGroupbox.TabStop = false;
            this.tilepropGroupbox.Text = "Tiles Properties";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tile Index";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Hexadecimal = true;
            this.numericUpDown1.Location = new System.Drawing.Point(6, 103);
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
            this.priorityCheckbox.Location = new System.Drawing.Point(6, 65);
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
            this.mirroryCheckbox.Location = new System.Drawing.Point(6, 42);
            this.mirroryCheckbox.Name = "mirroryCheckbox";
            this.mirroryCheckbox.Size = new System.Drawing.Size(62, 17);
            this.mirroryCheckbox.TabIndex = 2;
            this.mirroryCheckbox.Text = "Mirror Y";
            this.mirroryCheckbox.UseVisualStyleBackColor = true;
            this.mirroryCheckbox.CheckedChanged += new System.EventHandler(this.mirrorxCheckbox_CheckedChanged);
            // 
            // palettePicturebox
            // 
            this.palettePicturebox.Location = new System.Drawing.Point(116, 12);
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
            this.mirrorxCheckbox.Location = new System.Drawing.Point(6, 19);
            this.mirrorxCheckbox.Name = "mirrorxCheckbox";
            this.mirrorxCheckbox.Size = new System.Drawing.Size(62, 17);
            this.mirrorxCheckbox.TabIndex = 0;
            this.mirrorxCheckbox.Text = "Mirror X";
            this.mirrorxCheckbox.UseVisualStyleBackColor = true;
            this.mirrorxCheckbox.CheckedChanged += new System.EventHandler(this.mirrorxCheckbox_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tilemap Index";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Hexadecimal = true;
            this.numericUpDown2.Location = new System.Drawing.Point(91, 7);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(90, 20);
            this.numericUpDown2.TabIndex = 6;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // TilemapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 647);
            this.Controls.Add(this.vramPanel);
            this.Controls.Add(this.panel1);
            this.Name = "TilemapEditor";
            this.Text = "Tilemap Editor";
            this.Load += new System.EventHandler(this.IntroEditor_Load);
            this.vramPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vramPicturebox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainPicturebox)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoomUpDown)).EndInit();
            this.tilepropGroupbox.ResumeLayout(false);
            this.tilepropGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.palettePicturebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
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
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown zoomUpDown;
        private System.Windows.Forms.Label label3;
    }
}