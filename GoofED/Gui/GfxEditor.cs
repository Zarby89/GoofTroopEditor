using GoofED;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofTroopEditor.Gui
{
    public partial class GfxEditor : Form
    {
        public GfxEditor()
        {
            InitializeComponent();
        }
        public Game game;
        PointeredImage ptrImageDisplay = new PointeredImage(128, 512);
        byte[] pcImgData;
        Color[] pal = new Color[16];
        int palValue = 0;
        private void GfxEditor_Load(object sender, EventArgs e)
        {


            for (int i = 0; i < 15; i++)
            {
                int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (i * 2);
                int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
                int gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (i * 5);
                int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

                int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
                int length = game.rom.ReadShort(gfxPtrPC + 3);
                byte[] s = Compression.DecompressGFX(game.rom.data, addr, length);


                if (i == 0)
                {
                    int nbr = 0;
                    //PointeredImage tmpPtrImg = new PointeredImage(512, 512);
                    //tmpPtrImg.Draw8bppTiles(0, 0, s, 16, 0, 0);
                    pcImgData = Utils.SnesTilesToPc8bppTiles(s, length / 32, 4);
                    for (int j = 0; j < 16; j++)
                    {
                        pal[j] = GFX.palette[j+(palValue*16)];
                    }
                    ptrImageDisplay.UpdatePalettes(pal);
                    ptrImageDisplay.Draw8bppTiles(0,0,pcImgData, 16, 0, 0);


                }


                //Create a folder containing all BG Gfx
            }


            /*

            for (int i = 0; i < 35; i++)
            {
                int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.SpritesGfx_Address) + (i * 2);
                int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
                int gfxPtrSnes = game.rom.ReadLong(Constants.SpritesGfx_Address) + (i * 5);
                int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

                int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
                int length = game.rom.ReadShort(gfxPtrPC + 3);
                byte[] s = Compression.DecompressGFX(game.rom.data, addr, length);


                FileStream fs = new FileStream(path + "\\" + "SPR" + i.ToString("X2") + ".bin", FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(s, 0, s.Length);
                fs.Close();

                //Create a folder containing all BG Gfx
            }


            byte[] sItem = Compression.DecompressGFX(game.rom.data, 0x060000, 0x01D00);
            FileStream fss = new FileStream(path + "\\" + "Items" + ".bin", FileMode.OpenOrCreate, FileAccess.Write);
            fss.Write(sItem, 0, sItem.Length);
            fss.Close();

            sItem = Compression.DecompressGFX(game.rom.data, 0x06C55C, 0x60C0);
            fss = new FileStream(path + "\\" + "Items2" + ".bin", FileMode.OpenOrCreate, FileAccess.Write);
            fss.Write(sItem, 0, sItem.Length);
            fss.Close();*/
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            ptrImageDisplay.ClearBitmap(0);
            for (int j = 0; j < 16; j++)
            {
                pal[j] = GFX.palette[j + (palValue * 16)];
                pal[0] = Color.White;
            }
            ptrImageDisplay.UpdatePalettes(pal);
            ptrImageDisplay.Draw8bppTiles(0, 0, pcImgData, (int)numericUpDown1.Value, 0, 0);


            e.Graphics.DrawImage(ptrImageDisplay.bitmap,new Rectangle(0,0,256,1024), new Rectangle(0,0,128,512), GraphicsUnit.Pixel);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

            pictureBox1.Invalidate();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            for(int i = 0; i< 256;i++)
            {
                e.Graphics.FillRectangle(new SolidBrush(GFX.palette[i]), new Rectangle((i%16)*16, (i/16)*16, 16, 16));                
            }

        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            palValue = e.Y / 16;
            pictureBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int nbrTiles = pcImgData.Length / 64;
            int h = (int)(nbrTiles / numericUpDown1.Value);
            Bitmap b = new Bitmap(128, (h*8)+8);
            Graphics g = Graphics.FromImage(b);

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(ptrImageDisplay.bitmap, new Rectangle(0,0,128,h*8), new Rectangle(0,0,128,h*8), GraphicsUnit.Pixel);
            for(int i = 0; i <16;i++)
            {
                g.FillRectangle(new SolidBrush(pal[i]), new Rectangle(8*i, h*8, 8, 8));
            }


            Clipboard.SetImage(b);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            unsafe
            {
                if (Clipboard.ContainsImage())
                {
                    Bitmap b = (Bitmap)Clipboard.GetImage();
                    //Bitmap newdata = new Bitmap(128, b.Height - 8);
                    Color[] importPal = new Color[16];
                    for (int i = 0; i < 16; i++) // get palette
                    {
                        importPal[i] = Color.FromArgb(
                            b.GetPixel(i * 8, b.Height - 4).A,
                            b.GetPixel(i * 8, b.Height - 4).R >> 3,
                            b.GetPixel(i * 8, b.Height - 4).G >> 3,
                            b.GetPixel(i * 8, b.Height - 4).B >> 3);
                        //Console.Write("[" + i.ToString() + "]R" + importPal[i].R + ",G" + importPal[i].G + ",B" + importPal[i].B + " | ");
                    }

                    //Console.WriteLine("");
                    // convert bitmap data into a 4bpp directly and reload 4bpp sheet
                    Console.WriteLine(b.PixelFormat);
                    BitmapData bd = b.LockBits(new Rectangle(0, 0, (int)numericUpDown1.Value*8, b.Height - 8), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                    //BitmapData bd2 = newdata.LockBits(new Rectangle(0, 0, (int)numericUpDown1.Value*8, b.Height - 8), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                    Console.WriteLine("Stride " + bd.Stride);

                    byte* data = (byte*)bd.Scan0.ToPointer();
                    byte[] data2 = new byte[(int)((numericUpDown1.Value * 8) * (b.Height - 8))];
                    //byte* data2 = (byte*)bd2.Scan0.ToPointer();
                    int p = 0;
                    int pp = 0;
                    for (int y = 0; y < b.Height - 8; y++)
                    {
                        for (int x = 0; x < (int)numericUpDown1.Value * 8; x++)
                        {
                            Color c = Color.FromArgb(data[pp + 3] >> 3, data[pp + 2] >> 3, data[pp + 1] >> 3, data[pp + 0] >> 3);
                            byte pxValue = FindColor(importPal, c);
                            data2[p] = pxValue;
                            pp += 4;
                            p++;
                        }
                        pp = (bd.Stride * y);
                       
                    }


                    //newdata.Save("ABC.png");
                    PointeredImage tmpPtrImg = new PointeredImage((int)numericUpDown1.Value * 8, b.Height - 8);
                    tmpPtrImg.Draw8bppTiles(0,0,data2,(int)numericUpDown1.Value,0,0);

                    b.UnlockBits(bd);
                    //newdata.UnlockBits(bd2);


                    byte[] bpp4 = Utils.PCSheetToSnesTiles(tmpPtrImg, 4, (int)numericUpDown1.Value*2);
                    File.WriteAllBytes("Test4bpp.bin", bpp4);

                }



            }
        }

        public byte FindColor(Color[] pal, Color c)
        {
            for(int i =0; i < 16; i++)
            {
                if (c.R == pal[i].R && c.G == pal[i].G && c.B == pal[i].B)
                {
                    return (byte)i;
                }
            }
            Console.WriteLine("Cannot find R" + c.R + " G" + c.G + " B" + c.B);
            return 0;
        }
    }
}
