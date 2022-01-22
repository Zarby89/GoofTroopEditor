using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public partial class CreditEditor : Form
    {
        Game game;
        List<CreditLine> tempLines = new List<CreditLine>();
        int renderPos = 0;
        bool fromForm = false;
        bool needRefresh = false;
        public CreditEditor(Game game)
        {
            this.game = game;
            CreditLine[] c = (CreditLine[])game.creditLines.ToArray().Clone();
            for(int i = 0;i<c.Length;i++)
            {
                tempLines.Add(c[i]);
            }
            InitializeComponent();
        }

        private void CreditEditor_Load(object sender, EventArgs e)
        {

            updateList();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (creditList.SelectedIndex != -1)
            {
                tempLines.RemoveAt(creditList.SelectedIndex);
                updateList();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (creditList.SelectedIndex != -1)
            {
                tempLines.Insert(creditList.SelectedIndex, new CreditLine(0, 0, 0, 20, new byte[0]));
                updateList();
            }
        }

        private void updateList()
        {
            creditList.Items.Clear();
            int length = 0;
            foreach (CreditLine line in tempLines)
            {
                string s = "";
                for (int i = 0; i < line.data.Length; i++)
                {
                    s += (char)line.data[i];
                }
                
                length += line.linesSkip;
                line.yPos = length;
                creditList.Items.Add(s);
            }

            vScrollBar1.Maximum = length-16;

            creditBox.Refresh();


        }

        private void updateSize()
        {
            int length = 0;
            foreach (CreditLine line in tempLines)
            {
                length += line.linesSkip;
                line.yPos = length;
            }
            vScrollBar1.Maximum = length - 16;
        }

            private void creditBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.Clear(GFX.palette[0]);
            GFX.ClearBGCredits();
            foreach (CreditLine line in tempLines)
            {
                for (int i = 0; i < line.data.Length; i++)
                {
                    if (line.yPos < (renderPos + 32) && line.yPos >= renderPos)
                    {
                        GFX.DrawFromVRAM2bpp(GFX.creditBuffer, (ushort)(1056 + (line.data[i] - 0x20)), (byte)((line.xStart * 8) + (i * 8)), (byte)((line.yPos - renderPos) * 8),(byte)((line.palette>>2)&0x07)); //vert line
                    }
                }
            }

            e.Graphics.DrawImage(GFX.creditBitmap, new Rectangle(0, 0, 512, 512), 0, 0, 256, 256, GraphicsUnit.Pixel);

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            renderPos = vScrollBar1.Value;
            creditBox.Refresh();
            if (needRefresh)
            {
                updateList();
                needRefresh = false;
            }

            //updateList();
        }

        private string BytesToText(byte[] d)
        {
            string s = "";
            for (int i = 0; i < d.Length; i++)
            {
                s += (char)d[i];
            }
            return s;
        }

        private byte[] TextToBytes(string s)
        {
            byte[] bytes = new byte[s.Length];
            int i = 0;
            foreach(char c in s)
            {
                bytes[i] = (byte)c;
                i++;
            }
            return bytes;
            
        }

        private void creditList_SelectedIndexChanged(object sender, EventArgs e)
        {
            fromForm = true;
            if (creditList.SelectedIndex != -1)
            {
                string s = BytesToText(tempLines[creditList.SelectedIndex].data);
                horizontalTextbox.Text = tempLines[creditList.SelectedIndex].xStart.ToString("X2");
                linetoskipTextbox.Text = tempLines[creditList.SelectedIndex].linesSkip.ToString("X2");
                palTextbox.Text = tempLines[creditList.SelectedIndex].palette.ToString("X2");
                textTextbox.Text = s;
            }
            fromForm = false;
        }

        private void textTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (creditList.SelectedIndex != -1)
                {
                    tempLines[creditList.SelectedIndex].data = TextToBytes(textTextbox.Text);
                    needRefresh = true;
                    creditBox.Refresh();
                }
            }

            
        }

        private void horizontalTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (creditList.SelectedIndex != -1)
                {
                    byte b = 0;
                    byte.TryParse(horizontalTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    tempLines[creditList.SelectedIndex].xStart = b;
                    creditBox.Refresh();
                }
            }
        }

        private void linetoskipTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (creditList.SelectedIndex != -1)
                {
                    byte b = 0;
                    byte.TryParse(linetoskipTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    tempLines[creditList.SelectedIndex].linesSkip = b;
                    updateSize();
                    creditBox.Refresh();
                }
            }
        }

        private void palTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (creditList.SelectedIndex != -1)
                {
                    byte b = 0;
                    byte.TryParse(palTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    tempLines[creditList.SelectedIndex].palette = b;
                    creditBox.Refresh();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
                game.creditLines.Clear();
                CreditLine[] c = (CreditLine[])tempLines.ToArray().Clone();
                for (int i = 0; i < c.Length; i++)
                {
                    c[i].cCount = (byte)c[i].data.Length;
                    game.creditLines.Add(c[i]);
                }
            game.rom.WriteColor(0x5FEC0, GFX.palette[0]);

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        ColorDialog cd = new ColorDialog();
        private void bgcolorPicturebox_DoubleClick(object sender, EventArgs e)
        {
            
            cd.Color = GFX.palette[0];
            if (cd.ShowDialog() ==  DialogResult.OK)
            {
                GFX.palette[0] = cd.Color;
                bgcolorPicturebox.Refresh();
                creditBox.Refresh();
            }
            
        }

        private void bgcolorPicturebox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(GFX.palette[0]);

        }
    }
}
