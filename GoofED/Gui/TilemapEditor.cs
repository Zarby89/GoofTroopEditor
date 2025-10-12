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
    public partial class TilemapEditor : Form
    {
        public TilemapEditor()
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
        PointeredImage mainImage = new PointeredImage(256, 256);

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

            for (int i = 0; i < palgroup0[0x05].Length; i++)
            {
                pal[i + 16] = palgroup0[0x05][i];
            }
                Color[] colors2 = new Color[48];
            for(int i = 0; i<48;i++)
            {
                colors2[i] = game.rom.ReadColor(0x579A0 + (i * 2));
                pal[i+80] = colors2[i];
            }


            /*for (int i = 0; i < palgroup0[0x0D].Length; i++)
            {
                pal[i + 80] = palgroup0[0x0D][i];
            }*/
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

            int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (0x07 * 2);
            int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
            int gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (0x07 * 5);
            int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

            int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
            int length = game.rom.ReadShort(gfxPtrPC + 3);
            byte[] frameGfx = Compression.DecompressGFX(game.rom.data, addr, length);

            byte[] data = Utils.SnesTilesToPc8bppTiles(frameGfx, length / 0x20, 4);

            for(int i = 0; i< data.Length; i++)
            {
                vramRaw[i + 0x400] = data[i];
            }

            
            ptrImageVram.Draw8bppTiles(0, 0, vramRaw, 16, 0, 0);
            vramPicturebox.Invalidate();


            for (int i = 0; i < 9; i++)
            {
                if (i == 5 || i == 6 || i == 7) { continue; }
                int addr2 = game.rom.ReadLong(0x7FEA0 + (i * 5));
                int length2 = game.rom.ReadShort(0x7FEA0 + 3 + (i * 5));
                byte[] s = Compression.DecompressGFX(game.rom.data, Utils.SnesToPc(addr2), length2);
                ushort[] us = new ushort[s.Length/2];
                for (int j = 0; j < us.Length; j++)
                {
                    us[j] = (ushort)((s[(j*2)+1]<<8) + (s[(j*2)]));
                }


                BGs.Add(us);
            }


            mainImage.DrawTilemap(BGs[(int)numericUpDown2.Value], 32, ptrImageVram, 0);
            mainPicturebox.Invalidate();


        }

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
            e.Graphics.DrawImage(mainImage.bitmap, new Rectangle(0, 0, (int)(256*zoomUpDown.Value), (int)(256 * zoomUpDown.Value)));

            if (mx != -1 || my != -1)
            {
                e.Graphics.DrawRectangle(Pens.White, new Rectangle(((int)(mx * (zoomUpDown.Value*8))), (int)(my * (zoomUpDown.Value * 8)), (int)(8 * zoomUpDown.Value), (int)(8 * zoomUpDown.Value)));
            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < 16; i++)
            {
                for (int y = 0; y < 64; y++)
                {
                    vramRaw[(y) + (i * 64)] = (byte)(i);
                }
            }

            int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (BGGfxIndex[(int)numericUpDown2.Value] * 2);
            int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
            int gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (BGGfxIndex[(int)numericUpDown2.Value] * 5);
            int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

            int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
            int length = game.rom.ReadShort(gfxPtrPC + 3);
            byte[] frameGfx = Compression.DecompressGFX(game.rom.data, addr, length);

            byte[] data = Utils.SnesTilesToPc8bppTiles(frameGfx, length / 0x20, 4);

            for (int i = 0; i < data.Length; i++)
            {
                vramRaw[i + 0x400] = data[i];
            }


            for (int i = 0; i < 256; i++)
            {
                pal[i] = Color.FromArgb(255, (i % 16) * 16, (i % 16) * 16, (i % 16) * 16);
            }
            if ((int)numericUpDown2.Value == 0 || (int)numericUpDown2.Value == 5)
            {
                Color[] colors2 = new Color[48];
                for (int i = 0; i < 48; i++)
                {
                    colors2[i] = game.rom.ReadColor(0x579A0 + (i * 2));
                    pal[i + 80] = colors2[i];
                }

                for (int i = 0; i < palgroup0[0x05].Length; i++)
                {
                    pal[i + 16] = palgroup0[0x05][i];
                }
            }
            else if ((int)numericUpDown2.Value == 1 || (int)numericUpDown2.Value == 2)
            {
                
                    Color[] colors2 = new Color[48];
                    for (int i = 0; i < 48; i++)
                    {
                        colors2[i] = game.rom.ReadColor(0x56500 + (i * 2));
                        pal[i + 16] = colors2[i];
                    }
                    for (int i = 0; i < 16; i++)
                    {
                        pal[i + 64] = colors2[i+32];
                        pal[i + 80] = colors2[i+32];
                    }
                        Color[] colors3 = new Color[16];
                    for (int i = 0; i < 16; i++)
                    {
                        colors3[i] = game.rom.ReadColor(0x56460 + (i * 2));
                        pal[i + 112] = colors3[i];
                    }
            }
            else if ((int)numericUpDown2.Value == 3 || (int)numericUpDown2.Value == 4)
            {

                for (int i = 0; i < palgroup0[0x06].Length; i++)
                {
                    pal[i + 16] = palgroup0[0x06][i];
                }
            }

            mainImage.UpdatePalettes(pal);

                ptrImageVram.Draw8bppTiles(0, 0, vramRaw, 16, 0, 0);
            vramPicturebox.Invalidate();

            mainImage.ClearBitmap(0);
            mainImage.DrawTilemap(BGs[(int)numericUpDown2.Value], 32, ptrImageVram, 0);
            mainPicturebox.Invalidate();
        }

        byte[] BGGfxIndex = new byte[6] {07, 08, 08, 09, 09, 07 };

        private void mainPicturebox_MouseMove(object sender, MouseEventArgs e)
        {
            mx = (int)(e.X / (8 * zoomUpDown.Value));
            my = (int)(e.Y / (8 * zoomUpDown.Value));

            if (lastmx != mx || lastmy != my)
            {
                if (mxDown)
                {
                    if (mx + (my * 32) < BGs[(int)numericUpDown2.Value].Length)
                    {
                        BGs[(int)numericUpDown2.Value][mx + (my * 32)] = BuildTileData();
                    }
                }

                mainImage.ClearBitmap(0);
                mainImage.DrawTilemap(BGs[(int)numericUpDown2.Value], 32, ptrImageVram, 0);
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
            mx = (int)(e.X / (8 * zoomUpDown.Value));
            my = (int)(e.Y / (8 * zoomUpDown.Value));
            if (mx == -1 || my == -1)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                mxDown = true;
                if (mx + (my * 32) < BGs[(int)numericUpDown2.Value].Length)
                {
                    BGs[(int)numericUpDown2.Value][mx + (my * 32)] = BuildTileData();
                }
                mainImage.ClearBitmap(0);
                mainImage.DrawTilemap(BGs[(int)numericUpDown2.Value], 32, ptrImageVram, 0);
                mainPicturebox.Invalidate();
            }
            else
            {
                if (mx + (my * 32) < BGs[(int)numericUpDown2.Value].Length)
                {
                    updateTilesInfo(BGs[(int)numericUpDown2.Value][mx + (my * 32)]);
                }
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

            for (int i = 0; i < 5; i++)
            {
                byte[] data = new byte[BGs[i].Length * 2];
                for (int j = 0; j < BGs[i].Length; j++)
                {
                    data[(j * 2)] = (byte)BGs[i][j];
                    data[(j * 2) + 1] = (byte)(BGs[i][j]>>8);
                }
                int addr2 = game.rom.ReadLong(0x7FEA0 + (i * 5));
                int length2 = game.rom.ReadShort(0x7FEA0 + 3 + (i * 5));
                game.rom.WriteBytes(Utils.SnesToPc(addr2), Compression.CompressGfx(data));

                //BGs.Add(us);
            }


            byte[] data2 = new byte[BGs[5].Length * 2];
            for (int j = 0; j < BGs[5].Length; j++)
            {
                data2[(j * 2)] = (byte)BGs[5][j];
                data2[(j * 2) + 1] = (byte)(BGs[5][j] >> 8);
            }
            int addr = game.rom.ReadLong(0x7FEA0 + (8 * 5));
            int length = game.rom.ReadShort(0x7FEA0 + 3 + (8 * 5));
            game.rom.WriteBytes(Utils.SnesToPc(addr), Compression.CompressGfx(data2));
            this.Close();
        }

        private void zoomUpDown_ValueChanged(object sender, EventArgs e)
        {
            mainPicturebox.Invalidate();
        }
    }
}
