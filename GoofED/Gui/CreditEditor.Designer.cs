
namespace GoofED
{
    partial class CreditEditor
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
            this.creditBox = new System.Windows.Forms.PictureBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.creditList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.horizontalTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linetoskipTextbox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.palTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bgcolorPicturebox = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.creditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgcolorPicturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // creditBox
            // 
            this.creditBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.creditBox.Location = new System.Drawing.Point(12, 12);
            this.creditBox.Name = "creditBox";
            this.creditBox.Size = new System.Drawing.Size(512, 448);
            this.creditBox.TabIndex = 0;
            this.creditBox.TabStop = false;
            this.creditBox.Paint += new System.Windows.Forms.PaintEventHandler(this.creditBox_Paint);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(527, 12);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 448);
            this.vScrollBar1.TabIndex = 1;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // creditList
            // 
            this.creditList.FormattingEnabled = true;
            this.creditList.Location = new System.Drawing.Point(547, 12);
            this.creditList.Name = "creditList";
            this.creditList.Size = new System.Drawing.Size(221, 381);
            this.creditList.TabIndex = 2;
            this.creditList.SelectedIndexChanged += new System.EventHandler(this.creditList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(774, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Text";
            // 
            // textTextbox
            // 
            this.textTextbox.Location = new System.Drawing.Point(777, 28);
            this.textTextbox.Name = "textTextbox";
            this.textTextbox.Size = new System.Drawing.Size(176, 20);
            this.textTextbox.TabIndex = 4;
            this.textTextbox.TextChanged += new System.EventHandler(this.textTextbox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(774, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Horizontal Position";
            // 
            // horizontalTextbox
            // 
            this.horizontalTextbox.Location = new System.Drawing.Point(777, 67);
            this.horizontalTextbox.Name = "horizontalTextbox";
            this.horizontalTextbox.Size = new System.Drawing.Size(25, 20);
            this.horizontalTextbox.TabIndex = 6;
            this.horizontalTextbox.TextChanged += new System.EventHandler(this.horizontalTextbox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(774, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Lines to skip (before draw)";
            // 
            // linetoskipTextbox
            // 
            this.linetoskipTextbox.Location = new System.Drawing.Point(777, 106);
            this.linetoskipTextbox.Name = "linetoskipTextbox";
            this.linetoskipTextbox.Size = new System.Drawing.Size(25, 20);
            this.linetoskipTextbox.TabIndex = 8;
            this.linetoskipTextbox.TextChanged += new System.EventHandler(this.linetoskipTextbox_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(547, 428);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(221, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Insert at selected position";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(547, 399);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(221, 23);
            this.deleteButton.TabIndex = 10;
            this.deleteButton.Text = "Delete selected";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(885, 466);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(68, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Ok";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(811, 466);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(68, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // palTextbox
            // 
            this.palTextbox.Location = new System.Drawing.Point(777, 145);
            this.palTextbox.Name = "palTextbox";
            this.palTextbox.Size = new System.Drawing.Size(25, 20);
            this.palTextbox.TabIndex = 14;
            this.palTextbox.TextChanged += new System.EventHandler(this.palTextbox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(774, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Palette (must be << 2) + 0x20";
            // 
            // bgcolorPicturebox
            // 
            this.bgcolorPicturebox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bgcolorPicturebox.Location = new System.Drawing.Point(12, 466);
            this.bgcolorPicturebox.Name = "bgcolorPicturebox";
            this.bgcolorPicturebox.Size = new System.Drawing.Size(32, 32);
            this.bgcolorPicturebox.TabIndex = 15;
            this.bgcolorPicturebox.TabStop = false;
            this.bgcolorPicturebox.Paint += new System.Windows.Forms.PaintEventHandler(this.bgcolorPicturebox_Paint);
            this.bgcolorPicturebox.DoubleClick += new System.EventHandler(this.bgcolorPicturebox_DoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 485);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(217, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Background Color (double click to change it)";
            // 
            // CreditEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 501);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bgcolorPicturebox);
            this.Controls.Add(this.palTextbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.linetoskipTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.horizontalTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.creditList);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.creditBox);
            this.Name = "CreditEditor";
            this.Text = "CreditEditor";
            this.Load += new System.EventHandler(this.CreditEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.creditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgcolorPicturebox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox creditBox;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.ListBox creditList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox horizontalTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox linetoskipTextbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox palTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox bgcolorPicturebox;
        private System.Windows.Forms.Label label5;
    }
}