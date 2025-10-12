using GoofED;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofTroopEditor.Gui
{
    public partial class IntroEditor : Form
    {
        public IntroEditor()
        {
            InitializeComponent();
        }
        public Game game;
        Color[] pal = new Color[256];
        List<Color[]> palgroup0 = new List<Color[]>();
        List<ushort[]> BGs = new List<ushort[]>();

        ushort selectedTile = 0;
        byte selectedPal = 0;
        byte mirrorX = 0;
        byte mirrorY = 0;
        byte priority = 0;
        byte[] vramRaw;

        int mx = -1;
        int my = -1;
        int lastmx = 0;
        int lastmy = 0;
        private void palettePicturebox_Paint(object sender, PaintEventArgs e)
        {
            for(int i = 0;i<256;i++)
            {
                e.Graphics.FillRectangle(new SolidBrush(pal[i]), new Rectangle((i%16)*16, (i/16)*16, 16, 16));
            }
            e.Graphics.DrawRectangle(Pens.Yellow, new Rectangle(0, selectedPal*16, 256, 16));
        }
        PointeredImage ptrImageVram = new PointeredImage(128, 512);
        PointeredImage mainImage = new PointeredImage(96, 72);

        private void IntroEditor_Load(object sender, EventArgs e)
        {
            BGs = new List<ushort[]>();
            //BG0B - Frame
            //BG0C - Rest

            // Palette 15 (frame) slot1
            // Palette 0D(what's used for intro0-1) slot2+
            // Palette 0E(what's used for intro2-3) slot2+

            int bAddr = 0x056400;
            for (int i = 0; i < 24; i++) //24 first group from moved region
            {
                int src = game.rom.ReadByte(0x01FFB8 + (i * 3) + 1) * 32;
                int length2 = (game.rom.ReadByte(0x01FFB8 + (i * 3) + 2) + 1) / 2;
                Color[] colors = new Color[length2];
                for (int j = 0; j < length2; j++)
                {
                    colors[j] = game.rom.ReadColor(bAddr + src + (j * 2));
                }

                palgroup0.Add(colors);
            }

            
            for(int i = 0; i< 256; i++)
            {
                pal[i] = Color.FromArgb(255, (i%16)*16, (i%16)*16, (i%16)*16);
            }

            for(int i = 0; i < palgroup0[0x15].Length; i++)
            {
                pal[i + 16] = palgroup0[0x15][i];
            }
            for (int i = 0; i < palgroup0[0x0D].Length; i++)
            {
                pal[i + 32] = palgroup0[0x0D][i];
            }
            ptrImageVram.UpdatePalettes(pal);
            mainImage.UpdatePalettes(pal);
            palettePicturebox.Invalidate();

            vramRaw = new byte[0x10000];

            for (int i = 0; i < 16; i++)
            {
                for (int y = 0; y < 64; y++)
                {
                    vramRaw[(y) + (i * 64)] = (byte)(i);
                }
            }

            int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (0x0B * 2);
            int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
            int gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (0x0B * 5);
            int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

            int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
            int length = game.rom.ReadShort(gfxPtrPC + 3);
            byte[] frameGfx = Compression.DecompressGFX(game.rom.data, addr, length);

            byte[] data = Utils.SnesTilesToPc8bppTiles(frameGfx, length / 0x20, 4);

            for(int i = 0; i< data.Length; i++)
            {
                vramRaw[i + 0x400] = data[i];
            }


            gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (0x0C * 2);
            dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
            gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (0x0C * 5);
            gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

            addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
            length = game.rom.ReadShort(gfxPtrPC + 3);
            byte[] sheet1Gfx = Compression.DecompressGFX(game.rom.data, addr, length);

            byte[] data2 = Utils.SnesTilesToPc8bppTiles(sheet1Gfx, length / 0x20, 4);

            for (int i = 0; i < data2.Length; i++)
            {
                vramRaw[i + 0x400 + data.Length] = data2[i];
            }

            
            ptrImageVram.Draw8bppTiles(0, 0, vramRaw, 16, 0, 0);
            vramPicturebox.Invalidate();

            

            byte[] s = Compression.DecompressGFX(game.rom.data, 0x04E2CD, 0x480);

            ushort[] bg = new ushort[0x6C];
            ushort[] bg2 = new ushort[0x6C];
            int p = 0;
            int pp = 0;

            for (int j = 0; j < 0x09; j++)
            {
                pp = (j * 0x40);
                for (int i = 0; i < 0x0C; i++)
                {
                    bg[p] = (ushort)(s[pp] + (s[pp+1]<<8));
                    bg2[p] = (ushort)(s[pp + 0x18] + (s[pp+0x19] << 8));
                    p += 1;
                    pp += 2;
                }
            }
            BGs.Add(bg);
            BGs.Add(bg2);

            bg = new ushort[0x6C];
            bg2 = new ushort[0x6C];
            p = 0;
            for (int j = 0; j < 0x09; j++)
            {
                pp = 0x240 + (j * 0x40);
                for (int i = 0; i < 0x0C; i++)
                {
                    bg[p] = (ushort)(s[pp] + (s[pp+1] << 8));
                    bg2[p] = (ushort)(s[pp + 0x18] + (s[pp + 0x19] << 8));
                    p += 1;
                    pp += 2;
                }
            }
            BGs.Add(bg);
            BGs.Add(bg2);

            s = Compression.DecompressGFX(game.rom.data, 0x04E5C9, 0x480);

            bg = new ushort[0x6C];
            bg2 = new ushort[0x6C];
            p = 0;
            pp = 0;

            for (int j = 0; j < 0x09; j++)
            {
                pp = (j * 0x40);
                for (int i = 0; i < 0x0C; i++)
                {
                    bg[p] = (ushort)(s[pp] + (s[pp + 1] << 8));
                    bg2[p] = (ushort)(s[pp + 0x18] + (s[pp + 0x19] << 8));
                    p += 1;
                    pp += 2;
                }
            }
            BGs.Add(bg);
            BGs.Add(bg2);

            bg = new ushort[0x6C];
            bg2 = new ushort[0x6C];
            p = 0;
            for (int j = 0; j < 0x09; j++)
            {
                pp = 0x240 + (j * 0x40);
                for (int i = 0; i < 0x0C; i++)
                {
                    bg[p] = (ushort)(s[pp] + (s[pp + 1] << 8));
                    bg2[p] = (ushort)(s[pp + 0x18] + (s[pp + 0x19] << 8));
                    p += 1;
                    pp += 2;
                }
            }
            BGs.Add(bg);
            BGs.Add(bg2);






            s = Compression.DecompressGFX(game.rom.data, 0x04DEB0, 0x0600);

            bg = new ushort[0x6C];
            bg2 = new ushort[0x6C];
            p = 0;
            pp = 0;

            for (int j = 0; j < 0x09; j++)
            {
                pp = (j * 0x40);
                for (int i = 0; i < 0x0C; i++)
                {
                    bg[p] = (ushort)(s[pp] + (s[pp + 1] << 8));
                    bg2[p] = (ushort)(s[pp + 0x18] + (s[pp + 0x19] << 8));
                    p += 1;
                    pp += 2;
                }
            }
            BGs.Add(bg);
            BGs.Add(bg2);

            bg = new ushort[0x6C];
            bg2 = new ushort[0x6C];
            p = 0;
            for (int j = 0; j < 0x09; j++)
            {
                pp = 0x240 + (j * 0x40);
                for (int i = 0; i < 0x0C; i++)
                {
                    bg[p] = (ushort)(s[pp] + (s[pp + 1] << 8));
                    bg2[p] = (ushort)(s[pp + 0x18] + (s[pp + 0x19] << 8));
                    p += 1;
                    pp += 2;
                }
            }
            BGs.Add(bg);
            BGs.Add(bg2);



            mainImage.DrawTilemap(BGs[(int)numericUpDown2.Value], 12, ptrImageVram, 0);
            mainPicturebox.Invalidate();


        }

        byte[] palused = new byte[12] { 0x0D, 0x0E, 0x0D, 0x0E, 0x10, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F };
        

        private void vramPicturebox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.DrawImage(ptrImageVram.bitmap, new Rectangle(0, 0, 512, 2048));

            int tx = (selectedTile % 16) * 32;
            int ty = (selectedTile / 16) * 32;

            e.Graphics.DrawRectangle(new Pen(Brushes.Yellow, 2), new Rectangle(tx, ty, 32, 32));

        }

        private void mainPicturebox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.DrawImage(mainImage.bitmap, new Rectangle(0, 0, 384, 288));

            if (mx != -1 || my != -1)
            {
                e.Graphics.DrawRectangle(Pens.White, new Rectangle(mx*32, my*32, 32, 32));
            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < 256; i++)
            {
                pal[i] = Color.FromArgb(255, (i % 16) * 16, (i % 16) * 16, (i % 16) * 16);
            }

            for (int i = 0; i < palgroup0[0x15].Length; i++)
            {
                pal[i + 16] = palgroup0[0x15][i];
            }
            for (int i = 0; i < palgroup0[palused[(int)numericUpDown2.Value]].Length; i++)
            {
                pal[i + 32] = palgroup0[palused[(int)numericUpDown2.Value]][i];
            }

            
            ptrImageVram.UpdatePalettes(pal);
            mainImage.UpdatePalettes(pal);
            palettePicturebox.Invalidate();

            mainImage.ClearBitmap(0);
            mainImage.DrawTilemap(BGs[(int)numericUpDown2.Value], 12, ptrImageVram, 0);
            mainPicturebox.Invalidate();
        }

        private void mainPicturebox_MouseMove(object sender, MouseEventArgs e)
        {
            mx = (int)e.X/32;
            my = (int)e.Y/32;




            if (lastmx != mx || lastmy != my)
            {
                if (mxDown)
                {
                    BGs[(int)numericUpDown2.Value][mx + (my * 12)] = BuildTileData();
                }

                mainImage.ClearBitmap(0);
                mainImage.DrawTilemap(BGs[(int)numericUpDown2.Value], 12, ptrImageVram, 0);
                mainPicturebox.Invalidate();
            }


            lastmx = mx;
            lastmy = my;


        }

        public ushort BuildTileData()
        {
            return (ushort)(selectedTile + (selectedPal <<10) + (mirrorX<< 14) + (mirrorY << 15) + (priority << 13));

        }

        private void mainPicturebox_MouseLeave(object sender, EventArgs e)
        {
            mx = -1;
            my = -1;
            mainPicturebox.Invalidate();
        }

        private void palettePicturebox_MouseDown(object sender, MouseEventArgs e)
        {
            selectedPal = (byte)(e.Y / 16);
            palettePicturebox.Invalidate();

            UpdateVRAM();

        }
        bool mxDown = false;
        private void mainPicturebox_MouseDown(object sender, MouseEventArgs e)
        {
            if (mx == -1 || my == -1)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                mxDown = true;
                BGs[(int)numericUpDown2.Value][mx + (my * 12)] = BuildTileData();
                mainImage.ClearBitmap(0);
                mainImage.DrawTilemap(BGs[(int)numericUpDown2.Value], 12, ptrImageVram, 0);
                mainPicturebox.Invalidate();
            }
            else
            {
                updateTilesInfo(BGs[(int)numericUpDown2.Value][mx + (my * 12)]);
            }
        }

        private void mainPicturebox_MouseUp(object sender, MouseEventArgs e)
        {
            mxDown = false;
        }
        bool FromForm = false;
        public void updateTilesInfo(ushort tile)
        {
            FromForm = true;
            mirrorxCheckbox.Checked = false;
            mirroryCheckbox.Checked = false;
            mirrorX = 0;
            mirrorY = 0;
            priority = 0;
            selectedTile = (ushort)(tile & 0x3FF);
            if ((tile & 0x8000) == 0x8000)
            {
                mirrorY = 1;
                mirroryCheckbox.Checked = true;
            }
            if ((tile & 0x4000) == 0x4000)
            {
                mirrorX = 1;
                mirrorxCheckbox.Checked = true;
            }
            if ((tile & 0x2000) == 0x2000)
            {
                priority = 1;
                priorityCheckbox.Checked = true;
            }
            selectedPal = (byte)((tile & 0x1C00)>>10);
            numericUpDown1.Value = selectedTile;

            palettePicturebox.Invalidate();

            


            UpdateVRAM();
            FromForm = false;
        }

        public void UpdateVRAM()
        {
            Color[] tempPal = new Color[16];
            for (int i = 0; i < 16; i++)
            {
                tempPal[i] = pal[i + (selectedPal * 16)];
            }

            ptrImageVram.UpdatePalettes(tempPal);
            ptrImageVram.Draw8bppTiles(0, 0, vramRaw, 16, mirrorX, mirrorY);
            vramPicturebox.Invalidate();
        }

        private void mirrorxCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FromForm)
            {
                UpdateTileAndVram();
            }

        }

        public void UpdateTileAndVram()
        {
            mirrorX = 0;
            mirrorY = 0;
            priority = 0;
            selectedTile = (ushort)numericUpDown1.Value;
            if (mirrorxCheckbox.Checked)
                mirrorX = 1;
            if (mirroryCheckbox.Checked)
                mirrorY = 1;
            if (priorityCheckbox.Checked)
                priority = 1;
            UpdateVRAM();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!FromForm)
            {
                UpdateTileAndVram();
            }
        }

        private void vramPicturebox_MouseDown(object sender, MouseEventArgs e)
        {
            
            numericUpDown1.Value = ((e.X/32)%16) + (((e.Y/32))*16);
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[0x480];
            for (int i = 0; i < 0x480; i++)
            {
                data[i] = 0x00;
            }

            int p = 0;
            int pp = 0;

            for (int l = 0; l < 9; l++)
            {
                pp = (l * 0x0C);
                p = (l * 0x40);
                for (int i = 0; i < 0x0C; i++)
                {
                    data[p] = (byte)(BGs[0][pp] & 0xFF);
                    data[p + 1] = (byte)((BGs[0][pp] >> 8) & 0xFF);
                    p += 2;
                    pp++;
                }
            }


            for (int l = 0; l < 9; l++)
            {
                pp = (l * 0x0C);
                p = (l * 0x40) + 0x18;
                for (int i = 0; i < 0x0C; i++)
                {
                    data[p] = (byte)(BGs[1][pp] & 0xFF);
                    data[p + 1] = (byte)((BGs[1][pp] >> 8) & 0xFF);
                    p += 2;
                    pp++;
                }
            }








            for (int l = 0; l < 9; l++)
            {
                pp = (l * 0x0C);
                p = (l * 0x40) + 0x240;
                for (int i = 0; i < 0x0C; i++)
                {
                    data[p] = (byte)(BGs[2][pp] & 0xFF);
                    data[p + 1] = (byte)((BGs[2][pp] >> 8) & 0xFF);
                    p += 2;
                    pp++;
                }
            }


            for (int l = 0; l < 9; l++)
            {
                pp = (l * 0x0C);
                p = (l * 0x40) + 0x240 + 0x18;
                for (int i = 0; i < 0x0C; i++)
                {
                    data[p] = (byte)(BGs[3][pp] & 0xFF);
                    data[p + 1] = (byte)((BGs[3][pp] >> 8) & 0xFF);
                    p += 2;
                    pp++;
                }
            }

            byte[] s = Compression.CompressGfx(data);
            for (int i = 0; i < s.Length; i++)
            {
                game.rom.data[i + 0x04E2CD] = s[i];
            }


            this.Close();
        }
    }
}
