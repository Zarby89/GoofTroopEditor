using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public partial class TextEditor : Form
    {
        Game game;
        List<int> messageOffsets = new List<int>();
        int messagePos = 0;
        string[] allTexts = new string[50];
        int selectedText = 49;
        public TextEditor(Game game)
        {
            InitializeComponent();
            this.game = game;
            GFX.ClearBGText();

            for (int j = 0; j < 50; j++)
            {
                messagesListbox.Items.Add("Message " + j.ToString("X2"));
                int i = 0;
                text = "";
                while (game.texts[j][i] != 0)
                {
                    if (game.texts[j][i] == 0x80) //new line
                    {
                        text += "\r\n";
                        i++;
                        continue;
                    }
                    else if (game.texts[j][i] == 0x81) //new line wait
                    {
                        text += '>';
                        i++;
                        continue;
                    }
                    text += (char)game.texts[j][i];
                    i++;
                }
                allTexts[j] = text;

            }
            //fromForm = true;
            messagesListbox.SelectedIndex = 0;
            //fromForm = false;
            
        }

        private void TextEditor_Load(object sender, EventArgs e)
        {

            
            GFX.DrawFromVRAM(GFX.textBuffer, 32, 0, 0);

        }

        private void TextEditor_Shown(object sender, EventArgs e)
        {

        }
        string text = "";
        bool fromForm = false;
        private void messagesListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                allTexts[selectedText] = text;
                selectedText = messagesListbox.SelectedIndex;
                messagePos = 0;
                upButton.Enabled = false;
                downButton.Enabled = false;
                GFX.ClearBGText();
                fromForm = true;
                textBox1.Text = allTexts[messagesListbox.SelectedIndex];
                text = allTexts[messagesListbox.SelectedIndex];
                fromForm = false;
                messagePicturebox.Refresh();
            }
        }

        private void getsubMessagesPtrs()
        {
            messageOffsets.Clear();
            messageOffsets.Add(0);
            for (int i = 0; i < text.Length; i++)
            {

                if (text[i] == '>') //new line wait
                {
                    i++;
                    messageOffsets.Add(i);
                }
            }

            if (messageOffsets.Count > 1)
            {
                if (messagePos < messageOffsets.Count-1)
                {
                    downButton.Enabled = true;
                }
                else
                {
                    downButton.Enabled = false;
                }
            }
            else
            {
                messagePos = 0;
            }
            if (messagePos != 0)
            {
                upButton.Enabled = true;
            }
            
        }

        private void messagePicturebox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.Clear(Color.DarkGray);
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            getsubMessagesPtrs();

            GFX.ClearBGText();

            GFX.DrawFromVRAM(GFX.textBuffer, 1026, 0, 0); //corner
            GFX.DrawFromVRAM(GFX.textBuffer, 1026, 200, 0,false,1); //corner
            GFX.DrawFromVRAM(GFX.textBuffer, 1026, 0, 56,false,0,1); //corner
            GFX.DrawFromVRAM(GFX.textBuffer, 1026, 200, 56,false,1,1); //corner


            for (int i = 0;i<24;i++)
            {
                GFX.DrawFromVRAM(GFX.textBuffer, 1027, (byte)((i * 8) + 8), 0); //horiz line
                GFX.DrawFromVRAM(GFX.textBuffer, 1027, (byte)((i * 8) + 8), 56,false,0,1); //horiz line
            }

            for (int i = 0; i < 6; i++)
            {
                GFX.DrawFromVRAM(GFX.textBuffer, 1028, 0, (byte)((i * 8) + 8)); //vert line
                GFX.DrawFromVRAM(GFX.textBuffer, 1028, 200, (byte)((i * 8) + 8), false, 1); //vert line
            }

            int line = 0;
            int p = 0;
            for (int i = messageOffsets[messagePos];i< text.Length; i++)
            {

                if (text[i] == '>') //new line wait
                {
                    i++;
                    p = 0;
                    line = 0;
                    break;
                }

                if (text[i] == '\n')
                {

                    i++;
                    if (text[i] == '\r')
                    {
                        i++;
                    }
                    p = 0;
                    line++;
                }
                else if (text[i] == '\r')
                {
                    i++;
                    if (text[i] == '\n')
                    {
                        i++;
                    }
                    p = 0;
                    line++;
                }

                if (i == text.Length)
                {
                    break;
                }
                GFX.DrawFromVRAM(GFX.textBuffer, (ushort)(1056 + (text[i]-0x20)), (byte)(8 + (p*8)), (byte)(8+(line*16))); //vert line
                p++;
            }


            if (messagePos < messageOffsets.Count-1)
            {
                GFX.DrawFromVRAM(GFX.textBuffer, (ushort)(1207), (byte)(104-8), (byte)(48),false,0,0,0); //vert line
                GFX.DrawFromVRAM(GFX.textBuffer, (ushort)(1208), (byte)(112-8), (byte)(48), false, 0, 0, 0); //vert line
            }



            //GFX.DrawFromVRAM(GFX.textBuffer, 1027, 0, 0); //horiz line
            //GFX.DrawFromVRAM(GFX.textBuffer, 1028, 0, 0); //vertic line
            e.Graphics.DrawImage(GFX.textBitmap, new Rectangle(0, 0, 416, 128), 0,0,208,64,GraphicsUnit.Pixel);

        }

        private void downButton_Click(object sender, EventArgs e)
        {
            messagePos++;
            upButton.Enabled = true;
            Console.WriteLine(messageOffsets.Count);
            if (messagePos == messageOffsets.Count-1)
            {
                
                downButton.Enabled = false;
            }
            messagePicturebox.Refresh();
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            messagePos--;
            if (messagePos == 0)
            {
                upButton.Enabled = false;
                downButton.Enabled = true;
            }
            messagePicturebox.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                text = textBox1.Text;
                allTexts[selectedText] = text;
                upButton.Enabled = false;
                downButton.Enabled = false;
                messagePicturebox.Refresh();
            }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            List<byte> newTextByte = new List<byte>();

            for (int j = 0; j < 50; j++)
            {
                newTextByte.Clear();
                for (int i = 0; i < allTexts[j].Length; i++)
                {
                    
                    if (allTexts[j][i] == '>') //new line wait
                    {
                        newTextByte.Add((byte)(0x81));
                        i++;
                    }

                    if (allTexts[j][i] == '\n')
                    {

                        i++;
                        if (allTexts[j][i] == '\r')
                        {
                            i++;
                        }
                        newTextByte.Add((byte)(0x80));
                    }
                    else if (allTexts[j][i] == '\r')
                    {

                        i++;
                        if (allTexts[j][i] == '\n')
                        {
                            i++;
                        }
                        newTextByte.Add((byte)(0x80));
                    }


                    if (i >= allTexts[j].Length)
                    {
                        break;
                    }

                    newTextByte.Add((byte)(allTexts[j][i]));
                    
                }
                newTextByte.Add((byte)(0x00));
                game.texts[j] = newTextByte.ToArray();
                //Console.WriteLine("Message " + j.ToString("X2") + " Added Length" + newTextByte.Count);
            }
        }
    }
}
