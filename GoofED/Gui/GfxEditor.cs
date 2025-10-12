using GoofED;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GoofTroopEditor.Gui
{
    public partial class GfxEditor : Form
    {
        public GfxEditor()
        {
            InitializeComponent();
        }
        Dictionary<string, byte[]> allData = new Dictionary<string, byte[]>();
        public Game game;
        PointeredImage ptrImageDisplay = new PointeredImage(128, 512);
        byte[] pcImgData;
        Color[] pal = new Color[16];
        int palValue = 0;
        List<string> infos = new List<string>();
        private void GfxEditor_Load(object sender, EventArgs e)
        {

            allData.Clear();
            infos.Clear();


            for (int i = 0; i < 15; i++)
            {
                int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (i * 2);
                int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
                int gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (i * 5);
                int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

                int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
                int length = game.rom.ReadShort(gfxPtrPC + 3);
                byte[] s = Compression.DecompressGFX(game.rom.data, addr, length);

                allData.Add("BG" + i.ToString("X2"), s);
            }

            infos.Add("•BG00 - Capcom Logo");
            infos.Add("•BG01 - All text related graphics(letters, numbers, signals), selection fingers");
            infos.Add("•BG02 - Stage 1 Graphics(beach), also used on other stages(fences, doors, hooks)");
            infos.Add("•BG03 - Stage 2 Graphics(village)");
            infos.Add("•BG04 - Stage 3 Graphics(castle)");
            infos.Add("•BG05 - Stage 4 Graphics(underground caves)");
            infos.Add("•BG06 - Stage 5 Graphics(pirate ship)");
            infos.Add("•BG07 - Title Screen(goofy and max, big letters)");
            infos.Add("•BG08 - Player Select(boxes, 1p, 2p, letters, goofy and max)");
            infos.Add("•BG09 - Island Map(letters, overworld map)");
            infos.Add("•BG0A - Triggered switch, Plank(event placed), Diggeable Holes");
            infos.Add("•BG0B - Cutscene graphics");
            infos.Add("•BG0C - Cutscene graphics");
            infos.Add("•BG0D - Cutscene graphics");
            infos.Add("•BG0E - Password Screen (letters, stars, goofy and max))");



            for (int i = 0; i < 35; i++)
            {
                int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.SpritesGfx_Address) + (i * 2);
                int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
                int gfxPtrSnes = game.rom.ReadLong(Constants.SpritesGfx_Address) + (i * 5);
                int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

                int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
                int length = game.rom.ReadShort(gfxPtrPC + 3);
                byte[] s = Compression.DecompressGFX(game.rom.data, addr, length);


                allData.Add("SPR" + i.ToString("X2"), s);


            }

            infos.Add("•SPR00 - Edgehog");
            infos.Add("•SPR01 - Bee");
            infos.Add("•SPR02 - Ghost");
            infos.Add("•SPR03 - Snake");
            infos.Add("•SPR04 - Bat, Pirate Frag");
            infos.Add("•SPR05 - Islander, Fish");
            infos.Add("•SPR06 - Alive Armor");
            infos.Add("•SPR07 - Alive Armor");
            infos.Add("•SPR08 - Rolling Spikes(used on level 2)");
            infos.Add("•SPR09 - Spike Balls thrown by Jester Hole(level 1 boss), Bouncing boulders");
            infos.Add("•SPR0A - Rumblers vertical(level 4 boss)");
            infos.Add("•SPR0B - Rumbers horizontal");
            infos.Add("•SPR0C - Cannon");
            infos.Add("•SPR0D - Broken cannon");
            infos.Add("•SPR0E - Giant cannon ball following path, cannon ball, moving spikes(all used on level 3)");
            infos.Add("•SPR0F - Sword projectile from green pirate, small cannon ball from level 5 small cannons");
            infos.Add("•SPR10 - Frog");
            infos.Add("•SPR11 - Debris from breakable walls (mirror from level 3 uses it), moving platforms(SILVER PLATFORM from level 3 and WOODEN PLATFORM from level 5)");
            infos.Add("•SPR12 - 9 - Direction platform, Castle gate(from level 3)");
            infos.Add("•SPR13 - Falling stalactite(from level 4)");
            infos.Add("•SPR14 - Skeleton boss(level 3 boss)");
            infos.Add("•SPR15 - Skeleton boss");
            infos.Add("•SPR16 - Skeleton boss");
            infos.Add("•SPR17 - Final cutscene graphics(Alligator, Pete and PJ on a rope)");
            infos.Add("•SPR18 - Fire bug(level 2 boss)");
            infos.Add("•SPR19 - Fire bug(mostly his big arms)");
            infos.Add("•SPR1A - Fire bug(and flame pieces)");
            infos.Add("•SPR1B - Keelhaul Pete(final boss)");
            infos.Add("•SPR1C - Keelhaul Pete");
            infos.Add("•SPR1D - Keelhaul Pete(mostly his hook sprites)");
            infos.Add("•SPR1E - Minecart");
            infos.Add("•SPR1F - Final cutscene(pete and pj moving on the rope)");
            infos.Add("•SPR20 - Final cutscene(pirate king defeated)");
            infos.Add("•SPR21 - Final cutscene(small sprite of pirate king and alligator)");
            infos.Add("•SPR22 - The End text from staff roll");


            byte[] sItem = Compression.DecompressGFX(game.rom.data, 0x060000, 0x01D00);
        allData.Add("Items", sItem);
            infos.Add("- Goofy and Max HUD boxes\r\n- Banana and Cherry\r\n- Gem (two variation for palette purposes)\r\n- Hit effect (hookshot)\r\n- Bomb explosion effect\r\n- Drop shadow for Goofy and Max\r\n- Shovel\r\n- Hookshot\r\n- Small and boss keys\r\n- Plank (item)\r\n- Bell\r\n- Candle\r\n- Hookshot active graphics\r\n- Hookshot rope tied to a hook (event)\r\n- Parts of Goofy and Max sprites\r\n- Pickable bone from level 3 boss");

                sItem = Compression.DecompressGFX(game.rom.data, 0x06C55C, 0x60C0);
            allData.Add("Items2", sItem);
            infos.Add("- Pickable objects to throw: Barrel, Pot, Egg, Sign, Plant, Bomb, Log, Fences, Ice, Shell, Plates, Rocks and Nut\r\n- Star Block\r\n- Bomb Block\r\n- Bird that hatch from Egg (attention for animated parts)\r\n- Debris from object impact\r\n- More impact graphics\r\n- Object falling on hole (the small stars)\r\n- Water effect when object or enemy throw on water collision tiles\r\n- Pirates, both thin and fat variations, and the ball form (Rap) animation tiles.");

            sItem = Compression.DecompressGFX(game.rom.data, 0x061574, 0xD200);

            allData.Add("MaxGoofy", sItem);

            listBox1.Items.AddRange(allData.Keys.ToArray());
            infos.Add("Max and Goofy gfx");

                int nbr = 0;
        //PointeredImage tmpPtrImg = new PointeredImage(512, 512);
        //tmpPtrImg.Draw8bppTiles(0, 0, s, 16, 0, 0);
        pcImgData = Utils.SnesTilesToPc8bppTiles(allData.Values.ToArray()[0], allData.Values.ToArray()[0].Length / 32, 4);
                for (int j = 0; j< 16; j++)
                {
                    pal[j] = GFX.palette[j + (palValue * 16)];
                }
    ptrImageDisplay.UpdatePalettes(pal);
                ptrImageDisplay.Draw8bppTiles(0, 0, pcImgData, 16, 0, 0);
            listBox1.SelectedIndex = 0;
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = infos[listBox1.SelectedIndex];
            pcImgData = Utils.SnesTilesToPc8bppTiles(allData.Values.ToArray()[listBox1.SelectedIndex], allData.Values.ToArray()[listBox1.SelectedIndex].Length / 32, 4);
            for (int j = 0; j < 16; j++)
            {
                pal[j] = GFX.palette[j + (palValue * 16)];
            }
            ptrImageDisplay.UpdatePalettes(pal);
            ptrImageDisplay.Draw8bppTiles(0, 0, pcImgData, 16, 0, 0);
            pictureBox1.Invalidate();
        }
    }
}
