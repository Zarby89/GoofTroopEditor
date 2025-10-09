using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using AsarCLR;
using System.Diagnostics;
using GoofTroopEditor.Gui;

namespace GoofED
{
    public partial class Form1 : Form
    {
        Color[] palettes = new Color[256];
        bool fromForm = false;
        int frame = 0;
        Game game;
        //public ushort selectedTile = 0;
        public byte selectedPalette = 0;

        public ushort selectedTile8 = 0;
        public byte selectedPal8 = 0;
        public int selectedMx8 = 0;
        public int selectedMy8 = 0;
        public int selectedPrior8 = 0;

        public byte[] selected16Collision = new byte[4];
        AsmManagerNew asmManager;

        BGMode bgMode;
        ItemMode itemMode;
        SpriteMode spriteMode;
        ObjectMode objMode;
        HookMode hookMode;
        PlankMode plankMode;
        TransitionMode transitionMode;
        BlockDoorMode blockDoorMode;
        LockedDoorMode lockedDoorMode;
        EnemyDoorMode enemyDoorMode;
        GfxEditor gfxEditor;
        Bitmap editorBitmap = new Bitmap(512, 448);
        MusicViewer mv;
        int Zoom = 2;
        List<ItemName> itemList = new List<ItemName>()
        {
            new ItemName(0x08,"Hookshot"),
            new ItemName(0x09,"Candle"),
            new ItemName(0x0A,"Small Key"),
            new ItemName(0x0B,"Boss Key"),
            new ItemName(0x0C,"Shovel"),
            new ItemName(0x0D,"Bell"),
            new ItemName(0x0E,"Plank"),

            new ItemName(0x22,"Statue Shooting Fireball"),
            new ItemName(0x25,"Unused ????"),
            new ItemName(0x26,"Diggeable Hole?"),
            new ItemName(0x27,"Moving Wall"),
            new ItemName(0x28,"Unused ????"),
            new ItemName(0x29,"Spikes Walls"),
            new ItemName(0x2A,"Secret Waterfall"),
            new ItemName(0x2B,"Lava, Torches"),
            new ItemName(0x2C,"Pressure Switch Sprite"),
            new ItemName(0x2D,"Moving Holes"),
            new ItemName(0x2E,"Unused ????"),

            new ItemName(0x40,"Cherry"),
            new ItemName(0x42,"Banana"),
            new ItemName(0x44,"Red Gem"),
            new ItemName(0x46,"Blue Gem"),

            new ItemName(0x60, "Canon Ball Shooter"),
            new ItemName(0x61, "Canon Ball Following Path"),
            new ItemName(0x62, "Bouncing Boulder"),
            new ItemName(0x63, "Jumping Fish handler"),
            new ItemName(0x64, "Spawning barrel conveyor")

         };
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GFX.vramBitmap = new Bitmap(128, 1024, 128, PixelFormat.Format8bppIndexed, GFX.vramBuffer);
            GFX.vramMirrorBitmap = new Bitmap(128, 1024, 128, PixelFormat.Format8bppIndexed, GFX.vramMirrorBuffer);
            GFX.BG1Bitmap = new Bitmap(256, 256, 256, PixelFormat.Format8bppIndexed, GFX.BG1Buffer);
            GFX.BG2Bitmap = new Bitmap(256, 256, 256, PixelFormat.Format8bppIndexed, GFX.BG2Buffer);
            GFX.BG2BitmapMask = new Bitmap(256, 256, 256, PixelFormat.Format8bppIndexed, GFX.BG2BufferMask);
            GFX.SprBitmap = new Bitmap(256, 256, 256, PixelFormat.Format8bppIndexed, GFX.SprBuffer);
            GFX.tile16Bitmap = new Bitmap(128, 7680, 128, PixelFormat.Format8bppIndexed, GFX.tile16Buffer);
            GFX.scratch16Bitmap = new Bitmap(128, 7680, 128, PixelFormat.Format8bppIndexed, GFX.scratch16Buffer);
            GFX.textBitmap = new Bitmap(256, 64, 256, PixelFormat.Format8bppIndexed, GFX.textBuffer);
            GFX.creditBitmap = new Bitmap(256, 256, 256, PixelFormat.Format8bppIndexed, GFX.creditBuffer);
            GFX.passwordBitmap = new Bitmap(256, 16, 256, PixelFormat.Format8bppIndexed, GFX.passwordBuffer);
            GFX.editingTile16Bitmap = new Bitmap(16, 16, 16, PixelFormat.Format8bppIndexed, GFX.editingTile16Buffer);

            Asar.init();

            selectedMapTextbox.MouseWheel += SelectedMapTextbox_MouseWheel;

            spritesetTextbox.MouseWheel += SpritesetTextbox_MouseWheel;
            sprpalTextbox.MouseWheel += SprpalTextbox_MouseWheel;
            alternatepalTextbox.MouseWheel += AlternatepalTextbox_MouseWheel;
            animatedpalTextbox.MouseWheel += AnimatedpalTextbox_MouseWheel;

            GFX.ClearBGs();

        }

        public void LoadGame(string fn)
        {
            if (fn != "")
            {
                game = new Game(fn);
                gMain = Graphics.FromImage(editorBitmap);
                bgMode = new BGMode(game);
                itemMode = new ItemMode(game);
                objMode = new ObjectMode(game);
                spriteMode = new SpriteMode(game);
                hookMode = new HookMode(game);
                plankMode = new PlankMode(game);
                transitionMode = new TransitionMode(game);
                blockDoorMode = new BlockDoorMode(game);
                lockedDoorMode = new LockedDoorMode(game);
                enemyDoorMode = new EnemyDoorMode(game);

                if (game.rom.data[Constants.EditorVersion] == 0x6C)
                {
                    // unmodified ROM or previous version used

                }

                vramPanel.Enabled = true;
                tilePanel.Enabled = true;
                scratchpadPanel.Enabled = true;
                editingtilePicturebox.Enabled = true;
                editing16mxCheckbox.Enabled = true;
                editing16myCheckbox.Enabled = true;
                editor16priorityCheckbox.Enabled = true;
                collisionCombobox.Enabled = true;
                palettePicturebox.Enabled = true;
                button1.Enabled = true;
                aSMToolStripMenuItem.Enabled = true;
                viewToolStripMenuItem.Enabled = true;
                projectToolStripMenuItem.Enabled = true;
                collisionLabel.Enabled = true;
                if (File.Exists("ScratchPad.bin"))
                {

                    FileStream fs = new FileStream("ScratchPad.bin", FileMode.Open, FileAccess.Read);
                    byte[] data = new byte[0xC48 * 2];
                    fs.Read(data, 0, data.Length);
                    fs.Close();
                    for (int i = 0; i < 0xC48; i++)
                    {
                        GFX.scratchpadTiles[i] = (ushort)(data[(i * 2)] + (data[(i * 2) + 1] << 8));
                    }

                }
                
                asmManager = new AsmManagerNew(filename);
                mv = new MusicViewer(game);
                saveROMAsToolStripMenuItem.Enabled = true;
                saveROMToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = true;
                toolsToolStripMenuItem.Enabled = true;
                graphicsToolStripMenuItem.Enabled = true;
                levelCombobox.Enabled = true;
                groupBox1.Enabled = true;


                for (int i = 0; i < 256; i++)
                {
                    GFX.palette[i] = Color.Black;
                }

                UpdateGFX();

                UpdateMap();
                scratchpadPicturebox.Refresh();
                tiles16Picturebox.Refresh();
                fromForm = true;
                levelCombobox.SelectedIndex = 0;
                game.selectedLevel = 0;
                fromForm = false;

                itemPanel.Location = new Point(6, 460);
                objectPanel.Location = new Point(6, 460);
                transitionPanel.Location = new Point(6, 460);
                doorPanel.Location = new Point(6, 460);
                spritePanel.Location = new Point(6, 460);
                hookPanel.Location = new Point(6, 460);
                plankPanel.Location = new Point(6, 460);
                blockPanel.Location = new Point(6, 460);
                enemyDoorPanel.Location = new Point(6, 460);
                palPanel.Location = new Point(6, 460);
                selectedItemCombobox.Items.Clear();
                foreach (ItemName i in itemList)
                {
                    selectedItemCombobox.Items.Add(i.id.ToString("X2") + " " + i.name);
                }

                gfxEditor = new GfxEditor();
                gfxEditor.game = game;
                mainPicturebox.Image = new Bitmap(512, 448);
            }
        }

        private void AnimatedpalTextbox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta <= -1)
            {
                if (game.levels[game.selectedLevel].maps[game.selectedMap].animatedPal > 0)
                {
                    game.levels[game.selectedLevel].maps[game.selectedMap].animatedPal--;
                }
            }
            else if (e.Delta >= 1)
            {
                if (game.levels[game.selectedLevel].maps[game.selectedMap].animatedPal < 0x40)
                {
                    game.levels[game.selectedLevel].maps[game.selectedMap].animatedPal++;
                }
            }
            UpdateMap();
            UpdateGFX();
            UpdatePalette();
            mainPicturebox.Refresh();
            vramPicturebox.Refresh();
        }

        private void AlternatepalTextbox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta <= -1)
            {
                if (game.levels[game.selectedLevel].maps[game.selectedMap].altPal > 0)
                {
                    game.levels[game.selectedLevel].maps[game.selectedMap].altPal--;
                }
            }
            else if (e.Delta >= 1)
            {
                game.levels[game.selectedLevel].maps[game.selectedMap].altPal++;
            }
            UpdateMap();
            UpdateGFX();
            UpdatePalette();
            mainPicturebox.Refresh();
            vramPicturebox.Refresh();
        }

        private void SprpalTextbox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta <= -1) // scroll down
            {
                if (game.levels[game.selectedLevel].maps[game.selectedMap].spritepal > 3)
                {
                    game.levels[game.selectedLevel].maps[game.selectedMap].spritepal -= 4;
                }
            }
            else if (e.Delta >= 1) // scroll up
            {
                if (game.levels[game.selectedLevel].maps[game.selectedMap].spritepal < 0x7E)
                {
                    game.levels[game.selectedLevel].maps[game.selectedMap].spritepal += 4;
                }
            }
            UpdateMap();
        }

        private void SpritesetTextbox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta <= -1)
            {
                if (game.levels[game.selectedLevel].maps[game.selectedMap].spriteset > 2)
                {
                    game.levels[game.selectedLevel].maps[game.selectedMap].spriteset -= 3;
                }
            }
            else if (e.Delta >= 1)
            {
                if (game.levels[game.selectedLevel].maps[game.selectedMap].spriteset < 0x3D)
                {
                    game.levels[game.selectedLevel].maps[game.selectedMap].spriteset += 3;
                }
            }
            UpdateMap();
        }

        private void SelectedMapTextbox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta <= -1)
            {
                if (game.selectedMap > 0)
                {
                    game.selectedMap--;
                }
            }
            else if (e.Delta >= 1)
            {
                if (game.selectedMap < game.levels[game.selectedLevel].mapCount - 1)
                {
                    game.selectedMap++;
                }
            }

            //fromForm = true;
            //byte b1 = 0;
            //byte.TryParse(levelGfx1Textbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b1);

            selectedMapTextbox.Text = game.selectedMap.ToString("X2");


            //fromForm = false;
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < 256; i++)
            {
                e.Graphics.FillRectangle(new SolidBrush(GFX.palette[i]), new Rectangle(((i % 16) * 8), (i / 16) * 8, 8, 8));
            }
            e.Graphics.DrawRectangle(Pens.Yellow, new Rectangle(0, selectedPalette * 8, 128, 8));
        }

        private void vramPicturebox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.Clear(Color.Black);
            e.Graphics.DrawImage(GFX.vramMirrorBitmap, new Rectangle(0, 0, 256, 1024), 0, 0, 128, 512, GraphicsUnit.Pixel);
            e.Graphics.DrawImage(GFX.vramMirrorBitmap, new Rectangle(256, 0, 256, 1024), 0, 512, 128, 512, GraphicsUnit.Pixel);

            int tx = selectedTile8 % 16;
            int ty = selectedTile8 / 16;
            e.Graphics.DrawRectangle(Pens.Yellow, new Rectangle(tx * 16, ty * 16, 16, 16));
            TileInfo ti = new TileInfo(selectedTile8, selectedPal8, (ushort)selectedMx8, (ushort)selectedMy8, (ushort)selectedPrior8);

            label30.Text = "Tiles8 : ID " + selectedTile8.ToString("X4") + " / TileValue = " + ti.toShort().ToString("X4");

        }

        private void levelCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fromForm = true;
            selectedMapTextbox.Text = "00";
            game.selectedMap = (byte)00;
            UnselectAll();
            fromForm = false;
            game.selectedLevel = (byte)levelCombobox.SelectedIndex;
            UpdateGFX();
            UpdateMap();
            UpdatePalette();
        }

        private void levelGfx1Textbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                byte b1 = 0;
                byte b2 = 0;
                byte.TryParse(levelGfx1Textbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b1);
                byte.TryParse(levelGfx2Textbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b2);
                game.levels[game.selectedLevel].gfx1 = b1;
                game.levels[game.selectedLevel].gfx2 = b2;

                byte song = (byte)levelsongCombobox.SelectedIndex;
                game.levels[game.selectedLevel].song = song;

                UpdateGFX();
                UpdateMap();
                mainPicturebox.Invalidate();
            }
        }


        private void UpdatePalette()
        {

            byte animPal = game.levels[game.selectedLevel].maps[game.selectedMap].animatedPal;
            int ptrAnimPal = Utils.SnesToPc(game.rom.ReadShort(Constants.PalGroup_DataPtr + animPal) + 0x830000); //8812
            byte palCount = game.rom.ReadByte(ptrAnimPal);

            if (frame >= palCount)
            {
                frame = 0;
            }

            int src = (game.rom.ReadByte((ptrAnimPal + 3)+frame*2) << 5) + 0x8AE400;
            int dest = (byte)(game.rom.ReadByte(ptrAnimPal + 2) << 4);
            int delay = (game.rom.ReadByte((ptrAnimPal + 4) + frame * 2));
            timer1.Interval = (delay<<4)+1;





            if (animPal != 0)
            {
                //int sAddr = 0x8AE400 + 0x0A00 + (frame * 0x20);

                for (int i = 0; i < 16; i++)
                {
                    GFX.palette[i + dest] = game.rom.ReadColor((Utils.SnesToPc(src) + (i * 2)));
                }
                if (animPal == 0x22)
                {
                    ptrAnimPal = Utils.SnesToPc(game.rom.ReadShort(Constants.PalGroup_DataPtr + 0x12) + 0x830000); //8812
                    src = (game.rom.ReadByte(ptrAnimPal + 3) << 5) + 0x8AE400;
                    dest = (byte)(game.rom.ReadByte(ptrAnimPal + 2) << 4);
                    for (int i = 0; i < 16; i++)
                    {
                        GFX.palette[i + dest] = game.rom.ReadColor((Utils.SnesToPc(src) + (i * 2)));
                    }
                }
                else if (animPal == 0x10)
                {
                    ptrAnimPal = Utils.SnesToPc(game.rom.ReadShort(Constants.PalGroup_DataPtr + 0x14) + 0x830000); //8812
                    src = (game.rom.ReadByte(ptrAnimPal + 3) << 5) + 0x8AE400;
                    dest = (byte)(game.rom.ReadByte(ptrAnimPal + 2) << 4);
                    for (int i = 0; i < 16; i++)
                    {
                        GFX.palette[i + dest] = game.rom.ReadColor((Utils.SnesToPc(src) + (i * 2)));
                    }
                }
            }

            for (int i = 0; i < 64; i++)
            {
                GFX.palette[i + 0x80] = game.rom.ReadColor((Constants.staticSprPalette + (i * 2)));
            }






            ColorPalette cp = GFX.BG1Bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                cp.Entries[i] = GFX.palette[i];
                if (i % 16 == 0)
                {
                    cp.Entries[i] = Color.Transparent;
                }
            }

            ColorPalette cp2 = GFX.vramBitmap.Palette;
            for (int i = 0; i < 16; i++)
            {
                cp2.Entries[i] = GFX.palette[(i + (selectedPalette * 16))];
            }




            GFX.vramBitmap.Palette = cp2;
            GFX.vramMirrorBitmap.Palette = cp2;
            GFX.BG1Bitmap.Palette = cp;
            GFX.BG2Bitmap.Palette = cp;
            GFX.BG2BitmapMask.Palette = cp;
            GFX.SprBitmap.Palette = cp;
            GFX.tile16Bitmap.Palette = cp;
            GFX.scratch16Bitmap.Palette = cp;
            GFX.passwordBitmap.Palette = cp;
            GFX.textBitmap.Palette = cp;
            GFX.creditBitmap.Palette = cp;
            GFX.editingTile16Bitmap.Palette = cp;

            mainPicturebox.Refresh();
            palettePicturebox.Refresh();
        }

        private void UpdateGFX()
        {
            unsafe
            {
                byte* t = (byte*)GFX.vramBuffer;
                for (int i = 0; i < 0x20000; i++)
                {
                    t[i] = 0;
                }

                for (int i = 0; i < 16; i++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            t[(x) + (i * 8) + (y * 128)] = (byte)(i);
                        }
                    }
                }
            }

            byte sprSet = game.levels[game.selectedLevel].maps[game.selectedMap].spriteset;
            byte sprPal = game.levels[game.selectedLevel].maps[game.selectedMap].spritepal;


            int ptrPC = Utils.SnesToPc(game.rom.ReadShort(Constants.PalSprData1_Ptr) + 0x830000);
            byte g1 = game.rom.ReadByte(ptrPC + sprPal);
            byte g2 = game.rom.ReadByte(ptrPC + sprPal + 1);
            byte g3 = game.rom.ReadByte(ptrPC + sprPal + 2);
            byte g4 = game.rom.ReadByte(ptrPC + sprPal + 3);
            //E0, 80, A0, C0
            UpdateSingleSprPalette(g1, 0x80);
            UpdateSingleSprPalette(g2, 0xA0);
            UpdateSingleSprPalette(g3, 0xC0);
            UpdateSingleSprPalette(g4, 0xE0);
            spritepalLabel.Text = "- " + g1.ToString("X2") + " " + g2.ToString("X2") + " " + g3.ToString("X2") + " " + g4.ToString("X2");
            DecompressBGGfx(game.levels[game.selectedLevel].gfx1);
            DecompressBGGfx(game.levels[game.selectedLevel].gfx2);

            byte[] s = Compression.DecompressGFX(game.rom.data, 0x6C55C, 0x1000);
            byte[] data = GFX.snesbpp4Tobpp8(s, 16);
            int dest = 0xE000;

            unsafe
            {
                byte* t = (byte*)GFX.vramBuffer;
                int p = 0;
                for (int i = dest; i < dest + data.Length; i++)
                {
                    t[i] = data[p];
                    p++;
                }
            }


            /*

            s = Compression.DecompressGFX(game.rom.data, 0x6C55C, 0x0A00);
            data = GFX.snesbpp4Tobpp8(s, 16);
            dest = 0xE000;

            unsafe
            {
                byte* t = (byte*)GFX.vramBuffer;
                int p = 0;
                for (int i = dest; i < dest + data.Length; i++)
                {
                    t[i] = data[p];
                    p++;
                }
            }
           */
            s = Compression.DecompressGFX(game.rom.data, 0x060000, 0x01D00);
            byte[] d1 = new byte[0x500];
            d1 = GFX.snesbpp4Tobpp8row(s, 10, 0x500);


            //Draw Items 
            unsafe
            {
                dest = 0x18030;
                byte* t = (byte*)GFX.vramBuffer;
                int p = 0;
                for (int i = dest; i < dest + d1.Length; i++)
                {
                    t[i] = d1[p];
                    p++;
                }
            }
            data = GFX.snesbpp4Tobpp8row(s, 16, -1, 40);
            unsafe
            {
                dest = 0x19000;
                byte* t = (byte*)GFX.vramBuffer;
                int p = 0;
                for (int i = dest; i < dest + data.Length; i++)
                {
                    t[i] = data[p];
                    p++;
                }
            }


            g1 = game.rom.ReadByte(Constants.SpriteSet_Data + sprSet);
            g2 = game.rom.ReadByte(Constants.SpriteSet_Data + sprSet + 1);
            g3 = game.rom.ReadByte(Constants.SpriteSet_Data + sprSet + 2);
            //Console.WriteLine("G1-3 = " + g1.ToString("X2") + "  " + g2.ToString("X2") + "  " + g3.ToString("X2"));
            DecompressDynamicSprGfx((byte)(g1 / 2), 0xE800 * 2);
            DecompressDynamicSprGfx((byte)(g2 / 2), 0xF000 * 2);
            DecompressDynamicSprGfx((byte)(g3 / 2), 0xF800 * 2);

            spritesetLabel.Text = "- " + g1.ToString("X2") + " " + g2.ToString("X2") + " " + g3.ToString("X2");



            int addr = Utils.SnesToPc(game.rom.ReadLong(0x07FE00 + (10 * 5)));
            int length = game.rom.ReadShort(0x07FE00 + (10 * 5) + 3);
            s = Compression.DecompressGFX(game.rom.data, addr, length);
            data = GFX.snesbpp4Tobpp8row(s, 16);
            unsafe
            {
                dest = 0xF400;
                byte* t = (byte*)GFX.vramBuffer;
                int p = 0;
                for (int i = dest; i < dest + data.Length; i++)
                {
                    t[i] = data[p];
                    p++;
                }
            }

            addr = 0x06C55C;
            length = 0x4000;
            s = Compression.DecompressGFX(game.rom.data, addr, length);
            data = GFX.snesbpp4Tobpp8TileAt(s, 161);
            DrawVRAMTile(data, 0x1C000);
            data = GFX.snesbpp4Tobpp8TileAt(s, 162);
            DrawVRAMTile(data, 0x1C008);
            data = GFX.snesbpp4Tobpp8TileAt(s, 163);
            DrawVRAMTile(data, 0x1C010);
            data = GFX.snesbpp4Tobpp8TileAt(s, 164);
            DrawVRAMTile(data, 0x1C018);
            data = GFX.snesbpp4Tobpp8TileAt(s, 165);
            DrawVRAMTile(data, 0x1C020);


            data = GFX.snesbpp4Tobpp8TileAt(s, 166);
            DrawVRAMTile(data, 0x1C400);
            data = GFX.snesbpp4Tobpp8TileAt(s, 167);
            DrawVRAMTile(data, 0x1C408);
            data = GFX.snesbpp4Tobpp8TileAt(s, 168);
            DrawVRAMTile(data, 0x1C410);
            data = GFX.snesbpp4Tobpp8TileAt(s, 169);
            DrawVRAMTile(data, 0x1C418);



            data = GFX.snesbpp4Tobpp8TileAt(s, 437);
            DrawVRAMTile(data, 0x1C030);
            data = GFX.snesbpp4Tobpp8TileAt(s, 438);
            DrawVRAMTile(data, 0x1C038);
            data = GFX.snesbpp4Tobpp8TileAt(s, 439);
            DrawVRAMTile(data, 0x1C040);
            data = GFX.snesbpp4Tobpp8TileAt(s, 440);
            DrawVRAMTile(data, 0x1C048);
            data = GFX.snesbpp4Tobpp8TileAt(s, 441);
            DrawVRAMTile(data, 0x1C050);
            data = GFX.snesbpp4Tobpp8TileAt(s, 442);
            DrawVRAMTile(data, 0x1C058);

            data = GFX.snesbpp4Tobpp8TileAt(s, 443);
            DrawVRAMTile(data, 0x1C430);
            data = GFX.snesbpp4Tobpp8TileAt(s, 444);
            DrawVRAMTile(data, 0x1C438);
            data = GFX.snesbpp4Tobpp8TileAt(s, 445);
            DrawVRAMTile(data, 0x1C440);
            data = GFX.snesbpp4Tobpp8TileAt(s, 446);
            DrawVRAMTile(data, 0x1C448);
            data = GFX.snesbpp4Tobpp8TileAt(s, 447);
            DrawVRAMTile(data, 0x1C450);
            data = GFX.snesbpp4Tobpp8TileAt(s, 448);
            DrawVRAMTile(data, 0x1C458);



            byte[] ss = new byte[0x400];
            for (int i = 0; i < 0x400; i++)
            {
                ss[i] = game.rom.ReadByte(i + 0x07D400);
            }

            data = GFX.snesbpp4Tobpp8TileAt(ss, 0);
            DrawVRAMTile(data, 0x1C800);
            data = GFX.snesbpp4Tobpp8TileAt(ss, 1);
            DrawVRAMTile(data, 0x1C808);
            data = GFX.snesbpp4Tobpp8TileAt(ss, 2);
            DrawVRAMTile(data, 0x1C810);
            data = GFX.snesbpp4Tobpp8TileAt(ss, 3);
            DrawVRAMTile(data, 0x1C818);
            data = GFX.snesbpp4Tobpp8TileAt(ss, 4);
            DrawVRAMTile(data, 0x1C820);

            data = GFX.snesbpp4Tobpp8TileAt(ss, 5);
            DrawVRAMTile(data, 0x1CC00);
            data = GFX.snesbpp4Tobpp8TileAt(ss, 6);
            DrawVRAMTile(data, 0x1CC08);
            data = GFX.snesbpp4Tobpp8TileAt(ss, 7);
            DrawVRAMTile(data, 0x1CC10);
            data = GFX.snesbpp4Tobpp8TileAt(ss, 8);
            DrawVRAMTile(data, 0x1CC18);
            data = GFX.snesbpp4Tobpp8TileAt(ss, 9);
            DrawVRAMTile(data, 0x1CC20);



            addr = Utils.SnesToPc(game.rom.ReadLong(0x07FE00 + (1 * 5)));
            length = game.rom.ReadShort(0x07FE00 + (1 * 5) + 3);
            s = Compression.DecompressGFX(game.rom.data, addr, length);
            data = GFX.snesbpp2Tobpp8(s, 16);
            unsafe
            {
                dest = 0x10000;
                byte* t = (byte*)GFX.vramBuffer;
                int p = 0;
                for (int i = dest; i < dest + data.Length; i++)
                {
                    t[i] = data[p];
                    p++;
                }
            }

            int mx = 0;
            int my = 0;
            if (editing16mxCheckbox.Checked)
            {
                mx = 1;
            }
            if (editing16myCheckbox.Checked)
            {
                my = 1;
            }
            GFX.DrawTiles8Mirror(mx, my, selectedPalette);



            vramPicturebox.Refresh();
        }

        private unsafe void DrawVRAMTile(byte[] data, int dest)
        {

            byte* t = (byte*)GFX.vramBuffer;
            int p = 0;
            for (int i = 0; i < 64; i++)
            {
                int x = (i % 8);
                int y = (i / 8);
                t[x + (y * 128) + dest] = data[p];
                p++;
            }
        }

        private void UpdateSingleSprPalette(byte pal, byte startIndex)
        {

            //game.rom.ReadLong
            for (int i = 0; i < 16; i++)
            {
                GFX.palette[i + (startIndex / 2) + 128] = game.rom.ReadColor(Constants.staticSprPalette + (pal * 0x20) + (i * 2));
            }
        }

        private void UpdateSingleSprPaletteFromROM(int romPosition, byte startIndex)
        {

            //game.rom.ReadLong
            for (int i = 0; i < 16; i++)
            {
                GFX.palette[i + (startIndex / 2) + 128] = game.rom.ReadColor(romPosition + (i * 2));
            }
        }


        private unsafe void DecompressDynamicSprGfx(byte index, int dest)
        {


            //int gfxdestPtrSnes = 0x830000 + game.rom.ReadLong( + (index * 2);
            //int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
            int gfxPtrSnes = game.rom.ReadLong(Constants.SpritesGfx_Address) + (index * 5);
            int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

            int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
            int length = game.rom.ReadShort(gfxPtrPC + 3);
            //10C38E 0007

            byte[] s = Compression.DecompressGFX(game.rom.data, addr, length);
            //byte[] data = GFX.snesbpp4Tobpp8row(s, 14);
            byte[] data = GFX.snesbpp4Tobpp8Tiles(s, length / 32);

            byte* t = (byte*)GFX.vramBuffer;
            int p = 0;
            int Nbrtile = length / 32;

            for (int tile = 0; tile < Nbrtile; tile++)
            {
                for (int i = 0; i < 64; i++)
                {
                    int x = ((tile % 14) * 8) + (i % 8);
                    int y = (i / 8) + ((tile / 14) * 8);
                    t[x + (y * 128) + dest] = data[p];
                    p++;
                }
            }
            if (index == 4)
            {
                p = (34 * 64);
                for (int tile = 0; tile < 5; tile++)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        int x = ((tile % 14) * 8) + (i % 8);
                        int y = (i / 8) + ((tile / 14) * 8);
                        t[x + (y * 128) + 0x1FC00] = data[p];
                        p++;
                    }
                }
            }
            else if (index == 17)
            {
                p = (38 * 64);
                for (int tile = 0; tile < 10; tile++)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        int x = ((tile % 12) * 8) + (i % 8);
                        int y = (i / 8) + ((tile / 12) * 8);
                        t[x + (y * 128) + 0x1EC00] = data[p];
                        p++;
                    }
                }
            }
            else if (index == 30)
            {
                p = (12 * 64);
                for (int tile = 0; tile < 12; tile++)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        int x = ((tile % 12) * 8) + (i % 8);
                        int y = (i / 8) + ((tile / 12) * 8);
                        t[x + (y * 128) + 0x1E400] = data[p];
                        p++;
                    }
                }
            }
        }
        private void DecompressBGGfx(byte index)
        {
            int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (index * 2);
            int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
            int gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (index * 5);
            int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

            int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
            int length = game.rom.ReadShort(gfxPtrPC + 3);
            byte[] s = Compression.DecompressGFX(game.rom.data, addr, length);
            byte[] data = GFX.snesbpp4Tobpp8(s, 16);
            unsafe
            {
                byte* t = (byte*)GFX.vramBuffer;
                int p = 0;
                for (int i = dest; i < dest + data.Length; i++)
                {
                    t[i] = data[p];
                    p++;
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //UpdateMap();
        }

        private void loadDynamicPalette(byte des, byte src, byte length)
        {
            int srcPosPC = Utils.SnesToPc((0x8AE400 + (src * 32)));
            int p = 0;
            for (int i = (des / 2); i < (des / 2) + (length / 2) + 1; i++)
            {
                GFX.palette[i] = game.rom.ReadColor(srcPosPC + (p * 2));
                p++;
            }
        }

        private void mainPicturebox_Paint(object sender, PaintEventArgs e)
        {
            


            if (game != null)
            {
                Bitmap b = new Bitmap(512, 512);
                Graphics g = Graphics.FromImage(b);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.Clear(Color.Black);
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.Clear(Color.Black);
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;



                if (GlobalOptions.viewBg1)
                {
                    g.DrawImage(GFX.BG1Bitmap, new Rectangle(0, 0, 512, 512), 0, 0, 256, 256, GraphicsUnit.Pixel);
                }
                if (GlobalOptions.viewBg2)
                {
                    g.DrawImage(GFX.BG2Bitmap, new Rectangle(0, 0, 512, 512), 0, 0, 256, 256, GraphicsUnit.Pixel);
                }
                GFX.ClearBGSpr();
                if (GlobalOptions.viewItem)
                {
                    DrawItems(g);
                }

                if (GlobalOptions.viewTransition)
                {
                    DrawTransitions(g);
                }

                if (GlobalOptions.viewObject)
                {
                    DrawObjects(g);
                }

                if (GlobalOptions.viewSprite)
                {
                    DrawSprites(g);
                }

                

                DrawDoorsBlocks(g);

                DrawLockedDoors(g);

                DrawEnemiesDoors(g);

                DrawPlanks(g);


                DrawHooks(g);
                g.DrawImage(GFX.SprBitmap, new Rectangle(0, 0, 512, 512), 0, 0, 256, 256, GraphicsUnit.Pixel);
                if (GlobalOptions.viewBg2)
                {
                   g.DrawImage(GFX.BG2BitmapMask, new Rectangle(0, 0, 512, 512), 0, 0, 256, 256, GraphicsUnit.Pixel);
                }

                if (bg1TButton.Checked || bg2TButton.Checked)
                {
                    bgMode.Draw(gMain);
                    g.DrawImage(editorBitmap, 0, 0);
                }
                
                e.Graphics.DrawImage(b, new Rectangle(0, 0, 256*Zoom, 256*Zoom));

            }
        }


        private void DrawTransitions(Graphics g)
        {
            foreach (Transition t in game.levels[game.selectedLevel].maps[game.selectedMap].transitions)
            {
                byte x = (byte)((t.position & 0x1F));
                byte y = (byte)((t.position >> 5 & 0x1F));
                if (debugCheckbox.Checked)
                {
                    g.DrawString(t.dir.ToString("X2"), this.Font, Brushes.White, new Point((x * 16), y * 16));
                    g.DrawRectangle(new Pen(Color.Orange, 1), new Rectangle((x * 16), (y * 16), 16, 16));
                }
                int size = 1;
                if (t == transitionMode.selectedObject)
                {
                    size = 2;
                }


                if ((t.dir & 0x0F) == 4) //this is rectangle type
                {
                    if (((t.dir >> 4) & 0x07) == 0) // top
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 64, 16));
                    }

                    if (((t.dir >> 4) & 0x07) == 1) // top?
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 64, 32));
                    }

                    if (((t.dir >> 4) & 0x07) == 2) // right
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 16, 64));
                    }
                    if (((t.dir >> 4) & 0x07) == 3) // right
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 64));
                    }
                    if (((t.dir >> 4) & 0x07) == 6) // left
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 16, 64));
                    }
                    if (((t.dir >> 4) & 0x07) == 7) // left
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 64));
                    }
                    if (((t.dir >> 4) & 0x07) == 4) // down
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 64, 16));
                    }
                    if (((t.dir >> 4) & 0x07) == 5) // down
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 64, 32));
                    }
                }

                if ((t.dir & 0x0F) == 2) //this is square type?
                {
                    if (((t.dir >> 4) & 0x07) == 0) // top
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                    }
                    if (((t.dir >> 4) & 0x07) == 1) // top
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                    }
                    if (((t.dir >> 4) & 0x07) == 2) // right
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                    }
                    if (((t.dir >> 4) & 0x07) == 3) // right
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                    }
                    if (((t.dir >> 4) & 0x07) == 6) // left
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                    }
                    if (((t.dir >> 4) & 0x07) == 7) // left
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                    }
                    if (((t.dir >> 4) & 0x07) == 5) // down
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                    }
                    if (((t.dir >> 4) & 0x07) == 4) // down
                    {
                        g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                    }
                }

                if ((t.dir & 0x0F) == 0x0F) //this is small square type?
                {
                    g.DrawRectangle(new Pen(Color.Orange, size), new Rectangle((x * 16), (y * 16), 32, 32));
                }

            }
        }

        private void DrawObjects(Graphics g)
        {
            //Draw Lift/Blocks
            foreach (LiftObject obj in game.levels[game.selectedLevel].maps[game.selectedMap].objects)
            {
                byte x = (byte)(((obj.position & 0x3F) >> 1));
                byte y = (byte)(((obj.position & 0x3FC0) >> 6));
                if (debugCheckbox.Checked)
                {
                    g.DrawRectangle(Pens.Red, new Rectangle((x * 16), (y * 16), 32, 32));
                }
                if (obj == objMode.selectedObject)
                {
                    g.DrawRectangle(new Pen(Brushes.Red, 2), new Rectangle((x * 16), (y * 16), 32, 32));
                }

                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)((obj.id * 2) + 896), (byte)((x * 8)), (byte)((y * 8)), false, 0, 0, 7);
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)((obj.id * 2) + 897), (byte)((x * 8) + 8), (byte)((y * 8)), false, 0, 0, 7);
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)((obj.id * 2) + 898), (byte)((x * 8)), (byte)((y * 8) + 8), false, 0, 0, 7);
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)((obj.id * 2) + 899), (byte)((x * 8) + 8), (byte)((y * 8) + 8), false, 0, 0, 7);

            }
        }

        private void DrawHooks(Graphics g)
        {
            int hookIndex = 0;
            foreach (Hook hook in game.levels[game.selectedLevel].maps[game.selectedMap].hooks)
            {
                if (hookMode.selectedObject == hook)
                {
                    if (hookMode.selectedObjectX == 0)
                    {
                        g.DrawRectangle(new Pen(Brushes.Purple, 2), new Rectangle((hook.x * 2), (hook.y * 2), 16, 16));
                        g.DrawRectangle(Pens.Purple, new Rectangle((hook.x2 * 2), (hook.y2 * 2), 16, 16));

                    }
                    else
                    {
                        g.DrawRectangle(Pens.Purple, new Rectangle((hook.x * 2), (hook.y * 2), 16, 16));
                        g.DrawRectangle(new Pen(Brushes.Purple, 2), new Rectangle((hook.x2 * 2), (hook.y2 * 2), 16, 16));
                    }
                }
                else
                {
                    g.DrawRectangle(Pens.Purple, new Rectangle((hook.x * 2), (hook.y * 2), 16, 16));
                    g.DrawRectangle(Pens.Purple, new Rectangle((hook.x2 * 2), (hook.y2 * 2), 16, 16));
                }
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1072 + hookIndex), (byte)(hook.x), (byte)(hook.y + 9), false, 0, 0, 0);
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1072 + hookIndex), (byte)(hook.x2), (byte)(hook.y2 + 9), false, 0, 0, 0);
                // e.Graphics.DrawString((hook.y2 - hook.y).ToString(), new Font("arial", 16, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, new Point(hook.x * 2, (hook.y - 8) * 2));
                //e.Graphics.DrawString((hook.x2 - hook.x).ToString(), new Font("arial", 16, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, new Point(hook.x * 2, (hook.y - 16) * 2));

                //e.Graphics.DrawString(hookIndex.ToString("X2"), this.Font, Brushes.Red, new Point(hook.x * 2, hook.y * 2));
                //e.Graphics.DrawString(hookIndex.ToString("X2"), this.Font, Brushes.Red, new Point(hook.x2 * 2, hook.y2 * 2));
                hookIndex++;
            }
        }

        private void DrawPlanks(Graphics g)
        {
            int plankIndex = 0;
            foreach (Plank plank in game.levels[game.selectedLevel].maps[game.selectedMap].planks)
            {
                if (plankMode.selectedObject == plank)
                {
                    g.DrawRectangle(new Pen(Brushes.Brown,2), new Rectangle((plank.x * 2), (plank.y * 2), 32, 32));
                }
                else
                {
                    g.DrawRectangle(new Pen(Brushes.Brown, 1), new Rectangle((plank.x * 2), (plank.y * 2), 32, 32));
                }
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1072 + plankIndex), (byte)(plank.x), (byte)(plank.y + 9), false, 0, 0, 0);
                plankIndex++;
            }
        }

        private void UpdateMap()
        {
            fromForm = true;
            if (bgMode != null)
            {
                bgMode.redrawTiles8();
            }
            byte b1 = 0;
            byte.TryParse(selectedMapTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b1);
            if (b1 >= game.levels[game.selectedLevel].maps.Count)
            {
                b1 = (byte)(game.levels[game.selectedLevel].maps.Count - 1);
            }
            game.selectedMap = b1;


            darkCheckbox.Checked = game.levels[game.selectedLevel].maps[game.selectedMap].dark;
            iceCheckbox.Checked = game.levels[game.selectedLevel].maps[game.selectedMap].ice;

            levelGfx1Textbox.Text = game.levels[game.selectedLevel].gfx1.ToString("X2");
            levelGfx2Textbox.Text = game.levels[game.selectedLevel].gfx2.ToString("X2");
            spritesetTextbox.Text = game.levels[game.selectedLevel].maps[game.selectedMap].spriteset.ToString("X2");
            sprpalTextbox.Text = game.levels[game.selectedLevel].maps[game.selectedMap].spritepal.ToString("X2");
            levelsongCombobox.SelectedIndex = game.levels[game.selectedLevel].song;

            GFX.tilesBG1 = game.levels[game.selectedLevel].maps[b1].bg1tilemap8;
            GFX.tilesBG2 = game.levels[game.selectedLevel].maps[b1].bg2tilemap8;
            int PalPtrPC = 0;
            if (game.selectedLevel == 0)
            {
                PalPtrPC = Utils.SnesToPc(0x830000 + game.rom.ReadShort(Constants.level01PalPtr));
                loadDynamicPalette(game.rom.ReadByte(PalPtrPC), game.rom.ReadByte(PalPtrPC + 1), game.rom.ReadByte(PalPtrPC + 2));
            }
            else if (game.selectedLevel == 1)
            {
                PalPtrPC = Utils.SnesToPc(0x830000 + game.rom.ReadShort(Constants.level02PalPtr));
                loadDynamicPalette(game.rom.ReadByte(PalPtrPC), game.rom.ReadByte(PalPtrPC + 1), game.rom.ReadByte(PalPtrPC + 2));

                if (game.levels[game.selectedLevel].maps[game.selectedMap].altPal != 0)
                {
                    loadDynamicPalette(game.rom.ReadByte(PalPtrPC + 3), game.rom.ReadByte(PalPtrPC + 4), game.rom.ReadByte(PalPtrPC + 5));
                    loadDynamicPalette(game.rom.ReadByte(PalPtrPC + 6), game.rom.ReadByte(PalPtrPC + 7), game.rom.ReadByte(PalPtrPC + 8));
                }
            }
            else if (game.selectedLevel == 2)
            {
                PalPtrPC = Utils.SnesToPc(0x830000 + game.rom.ReadShort(Constants.level03PalPtr));
                loadDynamicPalette(game.rom.ReadByte(PalPtrPC), game.rom.ReadByte(PalPtrPC + 1), game.rom.ReadByte(PalPtrPC + 2));
                byte alt = game.levels[game.selectedLevel].maps[game.selectedMap].altPal;
                loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (alt * 3)), game.rom.ReadByte(PalPtrPC + (alt * 3) + 1), game.rom.ReadByte(PalPtrPC + (alt * 3) + 2));
            }
            else if (game.selectedLevel == 3)
            {
                PalPtrPC = Utils.SnesToPc(0x830000 + game.rom.ReadShort(Constants.level04PalPtr));
                loadDynamicPalette(game.rom.ReadByte(PalPtrPC), game.rom.ReadByte(PalPtrPC + 1), game.rom.ReadByte(PalPtrPC + 2));
                byte alt = game.levels[game.selectedLevel].maps[game.selectedMap].altPal;
                byte p1 = (byte)(game.levels[game.selectedLevel].maps[game.selectedMap].altPal & 0x07);
                byte p2 = (byte)((game.levels[game.selectedLevel].maps[game.selectedMap].altPal >> 3) & 0x07);

                loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (p1 * 3)), game.rom.ReadByte(PalPtrPC + (p1 * 3) + 1), game.rom.ReadByte(PalPtrPC + (p1 * 3) + 2));
                if (p2 == 0)
                {
                    //load 0x0B and 0x0C
                    loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (0x0B * 3)), game.rom.ReadByte(PalPtrPC + (0x0B * 3) + 1), game.rom.ReadByte(PalPtrPC + (0x0B * 3) + 2));
                    loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (0x0C * 3)), game.rom.ReadByte(PalPtrPC + (0x0C * 3) + 1), game.rom.ReadByte(PalPtrPC + (0x0C * 3) + 2));
                }
                if (p2 != 7)
                {
                    //load p2 palette
                    loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (p2 * 3)), game.rom.ReadByte(PalPtrPC + (p2 * 3) + 1), game.rom.ReadByte(PalPtrPC + (p2 * 3) + 2));
                    if ((p2 & 0x01) == 1)
                    {
                        //load palette 08
                        loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (0x08 * 3)), game.rom.ReadByte(PalPtrPC + (0x08 * 3) + 1), game.rom.ReadByte(PalPtrPC + (0x0B * 3) + 2));
                    }
                    if ((p2 & 0x02) == 2)
                    {
                        loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (0x07 * 3)), game.rom.ReadByte(PalPtrPC + (0x07 * 3) + 1), game.rom.ReadByte(PalPtrPC + (0x07 * 3) + 2));
                        loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (0x0B * 3)), game.rom.ReadByte(PalPtrPC + (0x0B * 3) + 1), game.rom.ReadByte(PalPtrPC + (0x0B * 3) + 2));
                        //load palette 07
                        //load palette 0B
                    }
                }


            }
            else if (game.selectedLevel == 4)
            {
                PalPtrPC = Utils.SnesToPc(0x830000 + game.rom.ReadShort(Constants.level05PalPtr));
                loadDynamicPalette(game.rom.ReadByte(PalPtrPC), game.rom.ReadByte(PalPtrPC + 1), game.rom.ReadByte(PalPtrPC + 2));
                byte alt = game.levels[game.selectedLevel].maps[game.selectedMap].altPal;
                loadDynamicPalette(game.rom.ReadByte(PalPtrPC + (alt * 3)), game.rom.ReadByte(PalPtrPC + (alt * 3) + 1), game.rom.ReadByte(PalPtrPC + (alt * 3) + 2));
            }


            alternatepalTextbox.Text = game.levels[game.selectedLevel].maps[game.selectedMap].altPal.ToString("X2");
            animatedpalTextbox.Text = game.levels[game.selectedLevel].maps[game.selectedMap].animatedPal.ToString("X2");
            PalPtrPC = Utils.SnesToPc(0x830000 + game.rom.ReadShort(Constants.splashscreenPalPtr));
            loadDynamicPalette(game.rom.ReadByte(PalPtrPC), game.rom.ReadByte(PalPtrPC + 1), game.rom.ReadByte(PalPtrPC + 2));
            PalPtrPC = Constants.sprBGPal;
            loadDynamicPalette(game.rom.ReadByte(PalPtrPC), game.rom.ReadByte(PalPtrPC + 1), game.rom.ReadByte(PalPtrPC + 2));

            GFX.DrawBG1();
            GFX.DrawBG2();
            GFX.DrawTiles16(game.tiles16.ToArray());
            GFX.DrawScratchPad(game.tiles16.ToArray());
            //tiles16Picturebox.Image = GFX.tile16Bitmap;
            //Barrel Draw!

            UpdatePalette();


            fromForm = false;
        }

        private void UnselectAll()
        {
            spriteMode.selectedObject = null;
            objMode.selectedObject = null;
            blockDoorMode.selectedObject = null;
            itemMode.selectedObject = null;
            hookMode.selectedObject = null;
            plankMode.selectedObject = null;
            transitionMode.selectedObject = null;



            for (int i = 0; i < toolStrip1.Items.Count; i++)
            {
                if (toolStrip1.Items[i] is ToolStripButton)
                {

                    if ((toolStrip1.Items[i] as ToolStripButton).Checked == true)
                    {
                        bg1TButton_Click(toolStrip1.Items[i], null);
                    }
                }
            }

            /*itemPanel.Visible = false;
            objectPanel.Visible = false;
            transitionPanel.Visible = false;
            doorPanel.Visible = false;
            spritePanel.Visible = false;
            hookPanel.Visible = false;
            blockPanel.Visible = false;*/
        }
        private void selectedMapTextbox_TextChanged(object sender, EventArgs e)
        {
            GFX.ClearBGs();
            if (!fromForm)
            {
                UnselectAll();
                UpdateMap();
            }
            mainPicturebox.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            frame++;
            if (frame >= 8)
            {
                frame = 0;
            }
            UpdatePalette();

        }

        private void spritesetTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                byte b1 = 0;
                byte.TryParse(spritesetTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b1);
                b1 = (byte)((b1 / 3) * 3);
                if (b1 > 0x3F)
                {
                    b1 = 0x3F;
                }
                game.levels[game.selectedLevel].maps[game.selectedMap].spriteset = b1;

                b1 = 0;
                byte.TryParse(sprpalTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b1);
                b1 = (byte)((b1 / 4) * 4);
                if (b1 > 0x80)
                {
                    b1 = 0x80;
                }
                game.levels[game.selectedLevel].maps[game.selectedMap].spritepal = b1;


            }
            UpdateGFX();
            //UpdateMap();
            vramPicturebox.Refresh();





        }

        private void mainPicturebox_MouseDown(object sender, MouseEventArgs e)
        {
            game.anyChange = true;
            if ((toolStrip1.Items[0] as ToolStripButton).Checked)
            {
                bgMode.mouseDown(e, 0, Zoom);
                editingtilePicturebox.Refresh();
                tiles16Picturebox.Refresh();
            }
            else if ((toolStrip1.Items[1] as ToolStripButton).Checked)
            {
                bgMode.mouseDown(e, 1, Zoom);
                editingtilePicturebox.Refresh();
                tiles16Picturebox.Refresh();
            }
            else if (itemTButton.Checked)
            {


                itemMode.mouseDown(sender, e, Zoom);
                if (itemMode.selectedObject != null)
                {
                    itemPanel.Enabled = true;
                    itemPanel.Visible = true;

                    fromForm = true;
                    selectedItemCombobox.SelectedIndex = itemList.FindIndex(x => x.id == itemMode.selectedObject.id);
                    itemsubtypeTextbox.Text = itemMode.selectedObject.ram.ToString("X2");
                    if ((itemMode.selectedObject.id & 0x20) == 0x20)
                    {
                        itemsubtypeTextbox.Enabled = true;
                    }
                    else
                    {
                        itemsubtypeTextbox.Enabled = false;
                    }
                    fromForm = false;
                }
            }
            else if (objectTButton.Checked)
            {
                objMode.mouseDown(sender, e, Zoom);
                if (objMode.selectedObject != null)
                {
                    objectPanel.Enabled = true;
                    objectPanel.Visible = true;
                    fromForm = true;
                    selectedobjectCombobox.SelectedIndex = (objMode.selectedObject.id / 2);
                    fromForm = false;
                }

            }
            else if (sprTButton.Checked)
            {

                spriteMode.mouseDown(sender, e, Zoom);
                if (spriteMode.selectedObject != null)
                {
                    spritePanel.Enabled = true;
                    spritePanel.Visible = true;

                    fromForm = true;
                    selectedSpriteCombobox.SelectedIndex = (spriteMode.selectedObject.id / 2);
                    sprParamTextbox.Text = spriteMode.selectedObject.unkn.ToString("X2");
                    sprSubtypeTextbox.Text = spriteMode.selectedObject.param.ToString("X2");
                    fromForm = false;
                }
            }
            else if (hooksTButton.Checked)
            {
                
                hookMode.mouseDown(sender, e, Zoom);
                if (hookMode.selectedObject != null)
                {
                    fromForm = true;
                    hooktypeCombobox.SelectedIndex = (hookMode.selectedObject.type);
                    fromForm = false;
                }
            }

            else if (plankTButton.Checked)
            {
                plankMode.mouseDown(sender, e, Zoom);
            }
            else if (transitionsTButton.Checked)
            {
                transitionMode.mouseDown(sender, e, Zoom);
                if (transitionMode.selectedObject != null)
                {
                    fromForm = true;
                    transitionDirCombobox.SelectedIndex = ((transitionMode.selectedObject.dir >> 4) & 0x07);
                    transitiondestmapTextbox.Text = transitionMode.selectedObject.tomap.ToString("X2");

                    if ((transitionMode.selectedObject.dir & 0x0F) == 0x02)
                    {
                        transitiontypeCombobox.SelectedIndex = 0;
                    }
                    else if ((transitionMode.selectedObject.dir & 0x0F) == 0x04)
                    {
                        transitiontypeCombobox.SelectedIndex = 1;
                    }
                    else if ((transitionMode.selectedObject.dir & 0x0F) == 0x0F)
                    {
                        transitiontypeCombobox.SelectedIndex = 2;
                    }
                    byte x = (byte)((transitionMode.selectedObject.position & 0x1F));
                    byte y = (byte)((transitionMode.selectedObject.position >> 5 & 0x1F));
                    transitionxposLabel.Text = "Xpos : " + (x * 8).ToString("X2");
                    transitionyposLabel.Text = "Ypos : " + (y * 8).ToString("X2");
                    transitiondestxTextbox.Text = transitionMode.selectedObject.xDest.ToString("X2");
                    transitiondestyTextbox.Text = transitionMode.selectedObject.yDest.ToString("X2");
                    fromForm = false;
                }
            }
            else if (blockTButton.Checked)
            {
                blockDoorMode.mouseDown(sender, e, Zoom);
                if (blockDoorMode.selectedObject != null)
                {
                    blockPanel.Enabled = true;
                    blockPanel.Visible = true;

                    fromForm = true;
                    savedoorCheckbox.Checked = blockDoorMode.selectedObject.saved;
                    drawswitchCheckbox.Checked = blockDoorMode.selectedObject.drawswitch;
                    saveslotBlockDoor.Value = blockDoorMode.selectedObject.doorRam;
                    blockdoordirCombobox.SelectedIndex = (blockDoorMode.selectedObject.doorDir / 2);
                    fromForm = false;
                }
            }
            else if (doorTButton.Checked)
            {
                lockedDoorMode.mouseDown(sender, e, Zoom);
                if (lockedDoorMode.selectedObject != null)
                {
                    fromForm = true;
                    lockeddoordirCombobox.SelectedIndex = (lockedDoorMode.selectedObject.doorDir / 2);
                    bosskeyCheckbox.Checked = lockedDoorMode.selectedObject.boss;
                    saveslotlockedDoor.Value = lockedDoorMode.selectedObject.doorRam;
                    fromForm = false;
                }
            }
            else if (enemydoorTButton.Checked)
            {
                enemyDoorMode.mouseDown(sender, e, Zoom);
                if (enemyDoorMode.selectedObject != null)
                {
                    fromForm = true;
                    enemydoorsizeCombobox.SelectedIndex = (enemyDoorMode.selectedObject.doorSize / 2);
                    enemydoorpiratecountUpDown.Value = enemyDoorMode.selectedObject.enemyCount;
                    if (enemyDoorMode.selectedObject.save)
                    {
                        saveEnemyDoorCheckbox.Checked = true;
                        enemydoorDIRPanel.Enabled = false;
                        saveslotEnemyDoor.Value = enemyDoorMode.selectedObject.doorRamDir;
                    }
                    else
                    {
                        saveEnemyDoorCheckbox.Checked = false;
                        enemydoorDIRPanel.Enabled = true;
                        int index = 0;
                        if (enemyDoorMode.selectedObject.doorRamDir == 0)
                        {
                            index = 0;
                        }
                        else if (enemyDoorMode.selectedObject.doorRamDir == 6)
                        {
                            index = 1;
                        }
                        else if (enemyDoorMode.selectedObject.doorRamDir == 2)
                        {
                            index = 2;
                        }
                        else if (enemyDoorMode.selectedObject.doorRamDir == 4)
                        {
                            index = 3;
                        }
                        else if (enemyDoorMode.selectedObject.doorRamDir == 0xFF)
                        {
                            index = 4;
                        }
                        enemydoordirectionCombobox.SelectedIndex = index;

                    }

                    fromForm = false;
                }
            }








            mainPicturebox.Refresh();
        }
        Graphics gMain;
        private void mainPicturebox_MouseMove(object sender, MouseEventArgs e)
        {
            
            if ((toolStrip1.Items[0] as ToolStripButton).Checked)
            {
                bgMode.mouseMove(e, 0, gMain, Zoom);
            }
            else if ((toolStrip1.Items[1] as ToolStripButton).Checked)
            {
                bgMode.mouseMove(e, 1, gMain, Zoom);
            }
            else if (itemTButton.Checked)
            {
                itemMode.mouseMove(e, Zoom);

            }
            else if (objectTButton.Checked)
            {
                objMode.mouseMove(e, Zoom);
            }
            else if (sprTButton.Checked)
            {
                spriteMode.mouseMove(e, Zoom);
            }
            else if (hooksTButton.Checked)
            {
                hookMode.mouseMove(e, Zoom);
            }
            else if (plankTButton.Checked)
            {
                plankMode.mouseMove(e, Zoom);
            }
            else if (transitionsTButton.Checked)
            {
                transitionMode.mouseMove(e, Zoom);
                if (transitionMode.selectedObject != null)
                {
                    byte x = (byte)((transitionMode.selectedObject.position & 0x1F));
                    byte y = (byte)((transitionMode.selectedObject.position >> 5 & 0x1F));
                    transitionxposLabel.Text = "Xpos : " + (x * 8).ToString("X2");
                    transitionyposLabel.Text = "Ypos : " + (y * 8).ToString("X2");
                }
            }
            else if (blockTButton.Checked)
            {
                blockDoorMode.mouseMove(e, Zoom);
            }
            else if (doorTButton.Checked)
            {
                lockedDoorMode.mouseMove(e, Zoom);
            }
            else if (enemydoorTButton.Checked)
            {
                enemyDoorMode.mouseMove(e, Zoom);
            }

            mainPicturebox.Refresh();
        }

        private void mainPicturebox_MouseUp(object sender, MouseEventArgs e)
        {
            if ((toolStrip1.Items[0] as ToolStripButton).Checked)
            {
                bgMode.mouseUp(e, 0);
            }
            else if ((toolStrip1.Items[1] as ToolStripButton).Checked)
            {
                bgMode.mouseUp(e, 1);
            }
            else if (itemTButton.Checked)
            {
                itemMode.mouseUp(e);
            }
            else if (objectTButton.Checked)
            {
                objMode.mouseUp(e);
            }
            else if (sprTButton.Checked)
            {
                spriteMode.mouseUp(e);
            }
            else if (hooksTButton.Checked)
            {
                hookMode.mouseUp(e);
            }
            else if (plankTButton.Checked)
            {
                plankMode.mouseUp(e);
            }
            else if (transitionsTButton.Checked)
            {
                transitionMode.mouseUp(e);
            }
            else if (blockTButton.Checked)
            {
                blockDoorMode.mouseUp(e);
            }
            else if (doorTButton.Checked)
            {
                lockedDoorMode.mouseUp(e);
            }
            else if (enemydoorTButton.Checked)
            {
                enemyDoorMode.mouseUp(e);
            }

            mainPicturebox.Refresh();
        }

        private void DrawDoorsBlocks(Graphics g)
        {
            byte bdIndex = 0;

            foreach (BlockDoor bd in game.levels[game.selectedLevel].maps[game.selectedMap].blockDoors)
            {
                int brushsize = 1;
                if (bd == blockDoorMode.selectedObject)
                {
                    brushsize = 2;
                }
                foreach (ushort b in bd.addrAllBlocks)
                {
                    byte x = (byte)(((b & 0x1F)));
                    byte y = (byte)(((b & 0x3E0) >> 5));


                    g.DrawRectangle(new Pen(Brushes.Yellow, brushsize), new Rectangle((x * 16), (y * 16), 32, 32));
                    GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1072 + bdIndex), (byte)(x * 8), (byte)(y * 8), false, 0, 0, 0);

                }
                int w = 64;
                int h = 64;
                if (bd.doorDir == 0x02)
                {
                    w = 32;
                }
                else if (bd.doorDir == 0x04)
                {
                    h = 32;
                }

                byte x2 = (byte)(((bd.doorAddr & 0x1F)));
                byte y2 = (byte)(((bd.doorAddr & 0x3E0) >> 5));
                g.DrawRectangle(new Pen(Brushes.Yellow, brushsize), new Rectangle((x2 * 16), (y2 * 16), w, h));
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1072 + bdIndex), (byte)(x2 * 8), (byte)(y2 * 8), false, 0, 0, 0);
                bdIndex++;
            }
        }

        private void DrawLockedDoors(Graphics g)
        {
            byte bdIndex = 0;

            foreach (LockedDoor d in game.levels[game.selectedLevel].maps[game.selectedMap].lockedDoors)
            {
                int brushsize = 1;
                if (d == lockedDoorMode.selectedObject)
                {
                    brushsize = 2;
                }
                int w = 64;
                int h = 64;
                if (d.doorDir == 0x02)
                {
                    w = 32;
                }
                else if (d.doorDir == 0x04)
                {
                    h = 32;
                }
                byte x2 = (byte)(((d.doorAddr & 0x1F)));
                byte y2 = (byte)(((d.doorAddr & 0x3E0) >> 5));
                g.DrawRectangle(new Pen(Brushes.DeepPink, brushsize), new Rectangle((x2 * 16), (y2 * 16), w, h));
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1072 + bdIndex), (byte)(x2 * 8), (byte)(y2 * 8), false, 0, 0, 0);
                bdIndex++;
            }
        }

        private void DrawEnemiesDoors(Graphics g)
        {
            byte bdIndex = 0;

            foreach (EnemyDoor d in game.levels[game.selectedLevel].maps[game.selectedMap].enemyDoors)
            {
                int brushsize = 1;
                /*if (d == lockedDoorMode.selectedObject)
                {
                    brushsize = 2;
                }*/
                int w = 64;
                int h = 64;
                if (d.doorSize == 0x02)
                {
                    w = 32;
                }
                else if (d.doorSize == 0x04)
                {
                    h = 32;
                }
                byte x2 = (byte)(((d.doorAddr & 0x1F)));
                byte y2 = (byte)(((d.doorAddr & 0x3E0) >> 5));
                g.DrawRectangle(new Pen(Brushes.LightGreen, brushsize), new Rectangle((x2 * 16), (y2 * 16), w, h));
                GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1072 + bdIndex), (byte)(x2 * 8), (byte)(y2 * 8), false, 0, 0, 0);
                bdIndex++;
            }
        }

        ItemsDraw itemDraw = new ItemsDraw();
        private void DrawItems(Graphics g)
        {
            itemDraw.Draw(g, game, itemMode.selectedObject, debugCheckbox.Checked);
        }


        SpritesDraw sprDraw = new SpritesDraw();
        private void DrawSprites(Graphics g)
        {
            sprDraw.Draw(g, game, spriteMode.selectedObject, debugCheckbox.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private bool SaveROM()
        {
            byte[] backupROM = (byte[])game.rom.data.Clone();

            game.rom.data[Constants.EditorVersion] = 0x01; // VERSION EDITOR! 01 = 1.4
           
            Asar.patch("ASM//required//darkice.asm", ref game.rom.data);
            Asar.patch("ASM//required//expandedtile32.asm", ref game.rom.data);
            Asar.patch("ASM//required//planks.asm", ref game.rom.data);
            Asarerror[] errors = Asar.geterrors();
            foreach (Asarerror er in errors)
            {
                Console.WriteLine(er.Fullerrdata);
            }

            createMap32TilesFrom16();
            createMap32TilesFrom16_ex();

            for (int i = 0; i < 4096; i++)
            {
                game.rom.data[Constants.Tiles32Data + (i * 4)] = (byte)(t32Unique[i].tile0 & 0xFF);
                game.rom.data[Constants.Tiles32Data + (i * 4) + 1] = (byte)(t32Unique[i].tile1 & 0xFF);
                game.rom.data[Constants.Tiles32Data + (i * 4) + 2] = (byte)(t32Unique[i].tile2 & 0xFF);
                game.rom.data[Constants.Tiles32Data + (i * 4) + 3] = (byte)(t32Unique[i].tile3 & 0xFF);

                game.rom.data[Constants.Tiles32Ext + (i * 2)] = (byte)(((t32Unique[i].tile0 & 0xF00) >> 8) | ((t32Unique[i].tile1 & 0xF00) >> 4));
                game.rom.data[Constants.Tiles32Ext + (i * 2) + 1] = (byte)(((t32Unique[i].tile2 & 0xF00) >> 8) | ((t32Unique[i].tile3 & 0xF00) >> 4));


                game.rom.data[Constants.Tiles32Data2 + (i * 4)] = (byte)(t32Unique_ex[i].tile0 & 0xFF);
                game.rom.data[Constants.Tiles32Data2 + (i * 4) + 1] = (byte)(t32Unique_ex[i].tile1 & 0xFF);
                game.rom.data[Constants.Tiles32Data2 + (i * 4) + 2] = (byte)(t32Unique_ex[i].tile2 & 0xFF);
                game.rom.data[Constants.Tiles32Data2 + (i * 4) + 3] = (byte)(t32Unique_ex[i].tile3 & 0xFF);

                game.rom.data[Constants.Tiles32Ext2 + (i * 2)] = (byte)(((t32Unique_ex[i].tile0 & 0xF00) >> 8) | ((t32Unique_ex[i].tile1 & 0xF00) >> 4));
                game.rom.data[Constants.Tiles32Ext2 + (i * 2) + 1] = (byte)(((t32Unique_ex[i].tile2 & 0xF00) >> 8) | ((t32Unique_ex[i].tile3 & 0xF00) >> 4));
            }


            for (int i = 0; i < 0xF00; i++)
            {
                game.rom.WriteUShort(Constants.Tiles16DataExt + (i * 8), game.tiles16[i].tile0.toShort());
                game.rom.WriteUShort(Constants.Tiles16DataExt + (i * 8) + 2, game.tiles16[i].tile1.toShort());
                game.rom.WriteUShort(Constants.Tiles16DataExt + (i * 8) + 4, game.tiles16[i].tile2.toShort());
                game.rom.WriteUShort(Constants.Tiles16DataExt + (i * 8) + 6, game.tiles16[i].tile3.toShort());

                game.rom.WriteByte(Constants.Tile16CollisionMap + i, game.collisions[i]);
            }


            if (game.SaveAll())
            {
                game.rom.data = (byte[])backupROM.Clone();
                return true;
            }

            return false;
            

        }


        private void vramPicturebox_MouseDown(object sender, MouseEventArgs e)
        {
            int tx = (e.X / 16);
            int ty = (e.Y / 16);
            selectedTile8 = (ushort)(tx + (ty * 16));
            vramPicturebox.Refresh();
        }

        private void palettePicturebox_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Y < 64)
            {
                selectedPalette = (byte)(e.Y / 8);
            }
            selectedPal8 = selectedPalette;
            UpdatePalette();
            UpdateGFX();
        }



        public List<Tile32> t32Unique = new List<Tile32>();
        public List<Tile32> t32Unique_ex = new List<Tile32>();
        public void createMap32TilesFrom16()
        {
            t32Unique.Clear();
            for (int i = 0; i < 4096; i++)
            {
                t32Unique.Add(new Tile32(666, 666, 666, 666));
            }
            int tiles32count = 0;
            const int nullVal = -1;

            for (int l = 0; l < 3; l++) //For each Levels
            {
                for (int m = 0; m < game.levels[l].maps.Count; m++)
                {
                    int tpos32 = 0;
                    int tpos16 = 0;
                    for (int ty = 0; ty < 8; ty++)
                    {
                        for (int tx = 0; tx < 8; tx++)
                        {

                            short foundIndex = nullVal;
                            short foundIndex2 = nullVal;
                            for (int j = 0; j < tiles32count; j++) // check in all existing tile if we can find one matching
                            {
                                if (t32Unique[j].tile0 == game.levels[l].maps[m].bg1tilemap16[tpos16])
                                {
                                    if (t32Unique[j].tile1 == game.levels[l].maps[m].bg1tilemap16[tpos16 + 1])
                                    {
                                        if (t32Unique[j].tile2 == game.levels[l].maps[m].bg1tilemap16[tpos16 + 16])
                                        {
                                            if (t32Unique[j].tile3 == game.levels[l].maps[m].bg1tilemap16[tpos16 + 17])
                                            {
                                                foundIndex = (short)j;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }


                            if (foundIndex == nullVal)
                            {
                                t32Unique[tiles32count] = new Tile32(game.levels[l].maps[m].bg1tilemap16[tpos16],
                                                                    game.levels[l].maps[m].bg1tilemap16[tpos16 + 1],
                                                                    game.levels[l].maps[m].bg1tilemap16[tpos16 + 16],
                                                                    game.levels[l].maps[m].bg1tilemap16[tpos16 + 17]);
                                game.levels[l].maps[m].bg1tilemap32[tpos32] = (ushort)tiles32count;
                                tiles32count++;
                            }
                            else
                            {
                                game.levels[l].maps[m].bg1tilemap32[tpos32] = (ushort)foundIndex;
                            }


                            for (int j = 0; j < tiles32count; j++) // check in all existing tile if we can find one matching
                            {
                                if (t32Unique[j].tile0 == game.levels[l].maps[m].bg2tilemap16[tpos16])
                                {
                                    if (t32Unique[j].tile1 == game.levels[l].maps[m].bg2tilemap16[tpos16 + 1])
                                    {
                                        if (t32Unique[j].tile2 == game.levels[l].maps[m].bg2tilemap16[tpos16 + 16])
                                        {
                                            if (t32Unique[j].tile3 == game.levels[l].maps[m].bg2tilemap16[tpos16 + 17])
                                            {
                                                foundIndex2 = (short)j;
                                            }
                                        }
                                    }
                                }
                            }



                            if (foundIndex2 == nullVal)
                            {
                                t32Unique[tiles32count] = new Tile32(game.levels[l].maps[m].bg2tilemap16[tpos16],
                                                                    game.levels[l].maps[m].bg2tilemap16[tpos16 + 1],
                                                                    game.levels[l].maps[m].bg2tilemap16[tpos16 + 16],
                                                                    game.levels[l].maps[m].bg2tilemap16[tpos16 + 17]);
                                game.levels[l].maps[m].bg2tilemap32[tpos32] = (ushort)tiles32count;
                                tiles32count++;
                                if (tiles32count > 4095)
                                {
                                    MessageBox.Show("Too many tile32 generated for level 0x00 to 0x02");
                                    return;
                                }
                            }
                            else
                            {
                                game.levels[l].maps[m].bg2tilemap32[tpos32] = (ushort)foundIndex2;
                            }

                            tpos32 += 1;
                            tpos16 += 2;
                        }
                        tpos16 += 16;
                    }


                }
            }
            Console.WriteLine("Nbr of unique tiles32 (decimal) ORIGINAL SPACE = " + tiles32count.ToString());
            
        }


        public void createMap32TilesFrom16_ex()
        {
            t32Unique_ex.Clear();
            for (int i = 0; i < 4096; i++)
            {
                t32Unique_ex.Add(new Tile32(666, 666, 666, 666));
            }
            int tiles32count_ex = 0;
            const int nullVal = -1;

            for (int l = 3; l < 5; l++) //For each Levels
            {
                for (int m = 0; m < game.levels[l].maps.Count; m++)
                {
                    int tpos32 = 0;
                    int tpos16 = 0;
                    for (int ty = 0; ty < 8; ty++)
                    {
                        for (int tx = 0; tx < 8; tx++)
                        {

                            short foundIndex = nullVal;
                            short foundIndex2 = nullVal;
                            for (int j = 0; j < tiles32count_ex; j++) // check in all existing tile if we can find one matching
                            {
                                if (t32Unique_ex[j].tile0 == game.levels[l].maps[m].bg1tilemap16[tpos16])
                                {
                                    if (t32Unique_ex[j].tile1 == game.levels[l].maps[m].bg1tilemap16[tpos16 + 1])
                                    {
                                        if (t32Unique_ex[j].tile2 == game.levels[l].maps[m].bg1tilemap16[tpos16 + 16])
                                        {
                                            if (t32Unique_ex[j].tile3 == game.levels[l].maps[m].bg1tilemap16[tpos16 + 17])
                                            {
                                                foundIndex = (short)j;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }


                            if (foundIndex == nullVal)
                            {
                                t32Unique_ex[tiles32count_ex] = new Tile32(game.levels[l].maps[m].bg1tilemap16[tpos16],
                                                                    game.levels[l].maps[m].bg1tilemap16[tpos16 + 1],
                                                                    game.levels[l].maps[m].bg1tilemap16[tpos16 + 16],
                                                                    game.levels[l].maps[m].bg1tilemap16[tpos16 + 17]);
                                game.levels[l].maps[m].bg1tilemap32[tpos32] = (ushort)tiles32count_ex;
                                tiles32count_ex++;
                            }
                            else
                            {
                                game.levels[l].maps[m].bg1tilemap32[tpos32] = (ushort)foundIndex;
                            }


                            for (int j = 0; j < tiles32count_ex; j++) // check in all existing tile if we can find one matching
                            {
                                if (t32Unique_ex[j].tile0 == game.levels[l].maps[m].bg2tilemap16[tpos16])
                                {
                                    if (t32Unique_ex[j].tile1 == game.levels[l].maps[m].bg2tilemap16[tpos16 + 1])
                                    {
                                        if (t32Unique_ex[j].tile2 == game.levels[l].maps[m].bg2tilemap16[tpos16 + 16])
                                        {
                                            if (t32Unique_ex[j].tile3 == game.levels[l].maps[m].bg2tilemap16[tpos16 + 17])
                                            {
                                                foundIndex2 = (short)j;
                                            }
                                        }
                                    }
                                }
                            }



                            if (foundIndex2 == nullVal)
                            {
                                t32Unique_ex[tiles32count_ex] = new Tile32(game.levels[l].maps[m].bg2tilemap16[tpos16],
                                                                    game.levels[l].maps[m].bg2tilemap16[tpos16 + 1],
                                                                    game.levels[l].maps[m].bg2tilemap16[tpos16 + 16],
                                                                    game.levels[l].maps[m].bg2tilemap16[tpos16 + 17]);
                                game.levels[l].maps[m].bg2tilemap32[tpos32] = (ushort)tiles32count_ex;
                                tiles32count_ex++;
                                if (tiles32count_ex > 4095)
                                {
                                    MessageBox.Show("Too many tile32 generated for level 0x03 to 0x04");
                                    return;
                                }
                            }
                            else
                            {
                                game.levels[l].maps[m].bg2tilemap32[tpos32] = (ushort)foundIndex2;
                            }

                            tpos32 += 1;
                            tpos16 += 2;
                        }
                        tpos16 += 16;
                    }


                }
            }
            //Console.WriteLine("Nbr of unique tiles32 (decimal) BANK92 = " + tiles32count_ex.ToString());
        }

        public List<Tile16> t16Unique = new List<Tile16>();
        public void createMap16TilesFrom8()
        {
            t16Unique.Clear();
            for (int i = 0; i < 0xC48; i++)
            {
                t16Unique.Add(new Tile16(GFX.gettilesinfo(666), GFX.gettilesinfo(666), GFX.gettilesinfo(666), GFX.gettilesinfo(666)));
            }
            int tiles16count = 0;
            const int nullVal = -1;

            for (int l = 0; l < 5; l++) //For each Levels
            {
                for (int m = 0; m < game.levels[l].maps.Count; m++)
                {
                    int tpos16 = 0;
                    int tpos8 = 0;
                    for (int ty = 0; ty < 16; ty++)
                    {
                        for (int tx = 0; tx < 16; tx++)
                        {

                            short foundIndex = nullVal;
                            short foundIndex2 = nullVal;
                            for (int j = 0; j < tiles16count; j++) // check in all existing tile if we can find one matching
                            {
                                if (t16Unique[j].tile0.toShort() == game.levels[l].maps[m].bg1tilemap8[tpos8])
                                {
                                    if (t16Unique[j].tile1.toShort() == game.levels[l].maps[m].bg1tilemap8[tpos8 + 1])
                                    {
                                        if (t16Unique[j].tile2.toShort() == game.levels[l].maps[m].bg1tilemap8[tpos8 + 32])
                                        {
                                            if (t16Unique[j].tile3.toShort() == game.levels[l].maps[m].bg1tilemap8[tpos8 + 33])
                                            {
                                                foundIndex = (short)j;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }


                            if (foundIndex == nullVal)
                            {
                                t16Unique[tiles16count] = new Tile16(GFX.gettilesinfo(game.levels[l].maps[m].bg1tilemap8[tpos8]),
                                                                    GFX.gettilesinfo(game.levels[l].maps[m].bg1tilemap8[tpos8 + 1]),
                                                                    GFX.gettilesinfo(game.levels[l].maps[m].bg1tilemap8[tpos8 + 32]),
                                                                    GFX.gettilesinfo(game.levels[l].maps[m].bg1tilemap8[tpos8 + 33]));
                                game.levels[l].maps[m].bg1tilemap16[tpos16] = (ushort)tiles16count;
                                tiles16count++;
                            }
                            else
                            {
                                game.levels[l].maps[m].bg1tilemap16[tpos16] = (ushort)foundIndex;
                            }
                            //BG2

                            for (int j = 0; j < tiles16count; j++) // check in all existing tile if we can find one matching
                            {
                                if (t16Unique[j].tile0.toShort() == game.levels[l].maps[m].bg2tilemap8[tpos8])
                                {
                                    if (t16Unique[j].tile1.toShort() == game.levels[l].maps[m].bg2tilemap8[tpos8 + 1])
                                    {
                                        if (t16Unique[j].tile2.toShort() == game.levels[l].maps[m].bg2tilemap8[tpos8 + 32])
                                        {
                                            if (t16Unique[j].tile3.toShort() == game.levels[l].maps[m].bg2tilemap8[tpos8 + 33])
                                            {
                                                foundIndex2 = (short)j;
                                            }
                                        }
                                    }
                                }
                            }



                            if (foundIndex2 == nullVal)
                            {
                                t16Unique[tiles16count] = new Tile16(GFX.gettilesinfo(game.levels[l].maps[m].bg2tilemap8[tpos8]),
                                                                    GFX.gettilesinfo(game.levels[l].maps[m].bg2tilemap8[tpos8 + 1]),
                                                                    GFX.gettilesinfo(game.levels[l].maps[m].bg2tilemap8[tpos8 + 32]),
                                                                    GFX.gettilesinfo(game.levels[l].maps[m].bg2tilemap8[tpos8 + 33]));
                                game.levels[l].maps[m].bg2tilemap16[tpos16] = (ushort)tiles16count;
                                tiles16count++;
                            }
                            else
                            {
                                game.levels[l].maps[m].bg2tilemap16[tpos16] = (ushort)foundIndex2;
                            }

                            tpos16 += 1;
                            tpos8 += 2;
                        }
                        tpos8 += 32;
                    }


                }
            }
            Console.WriteLine("Nbr of unique tiles16 (decimal) = " + tiles16count.ToString());
        }

        private void tiles16Picturebox_Paint(object sender, PaintEventArgs e)
        {
            if (game != null)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                e.Graphics.Clear(Color.Black);
                e.Graphics.DrawImage(GFX.tile16Bitmap, new Rectangle(0, 0, 256, 15360));
                if (bgMode.selectedTiles.Length == 1)
                {
                    int tx = (bgMode.selectedTiles[0] % 8);
                    int ty = (bgMode.selectedTiles[0] / 8);
                    e.Graphics.DrawRectangle(Pens.White, new Rectangle(tx * 32, ty * 32, 32, 32));
                }
            }
        }

        private void tiles16Picturebox_MouseDown(object sender, MouseEventArgs e)
        {
            int x = (e.X / 32);
            int y = (e.Y / 32);
            bgMode.selectedTiles[0] = (ushort)(x + (y * 8));
            bgMode.selectedWidth = 1;
            bgMode.selectedHeight = 1;
            tiles16Picturebox.Refresh();
            editingtilePicturebox.Refresh();
            label26.Text = "Tiles16 :  Selected Tile : " + bgMode.selectedTiles[0].ToString("X4");
        }

        private void bg1TButton_Click(object sender, EventArgs e)
        {
            //game.anyChange = true;
            for (int i = 0; i < toolStrip1.Items.Count; i++)
            {
                if (toolStrip1.Items[i] is ToolStripButton)
                {
                    (toolStrip1.Items[i] as ToolStripButton).Checked = false;
                }
            }
            (sender as ToolStripButton).Checked = true;
            itemPanel.Visible = false;
            spritePanel.Visible = false;
            objectPanel.Visible = false;
            doorPanel.Visible = false;
            hookPanel.Visible = false;
            blockPanel.Visible = false;
            transitionPanel.Visible = false;
            enemyDoorPanel.Visible = false;
            plankPanel.Visible = false;
            palPanel.Visible = false;
            if (sender == itemTButton)
            {
                itemPanel.Visible = true;
            }
            else if (sender == sprTButton)
            {
                spritePanel.Visible = true;
            }
            else if (sender == objectTButton)
            {
                objectPanel.Visible = true;
            }
            else if (sender == doorTButton)
            {
                doorPanel.Visible = true;
            }
            else if (sender == blockTButton)
            {
                blockPanel.Visible = true;
            }
            else if (sender == hooksTButton)
            {
                hookPanel.Visible = true;
            }
            else if (sender == hooksTButton)
            {
                hookPanel.Visible = true;
            }
            else if (sender == plankTButton)
            {
                plankPanel.Visible = true;
            }
            else if (sender == transitionsTButton)
            {
                transitionPanel.Visible = true;
            }
            else if (sender == enemydoorTButton)
            {
                enemyDoorPanel.Visible = true;
            }
            else if (sender == palTButton)
            {
                palPanel.Visible = true;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sprTButton.Checked)
            {
                spriteMode.Delete();
                spritePanel.Visible = false;
            }
            else if (objectTButton.Checked)
            {
                objMode.Delete();
                objectPanel.Visible = false;
            }
            else if (itemTButton.Checked)
            {
                itemMode.Delete();
                itemPanel.Visible = false;
            }
            else if (hooksTButton.Checked)
            {
                hookMode.Delete();
                hookPanel.Visible = false;
            }
            else if (plankTButton.Checked)
            {
                plankMode.Delete();
                plankPanel.Visible = false;
            }
            else if (blockTButton.Checked)
            {
                blockDoorMode.Delete();
                blockPanel.Visible = false;
            }
            else if (doorTButton.Checked)
            {
                lockedDoorMode.Delete();
                doorPanel.Visible = false;
            }
            else if (transitionsTButton.Checked)
            {
                transitionMode.Delete();
                doorPanel.Visible = false;
            }
            else if (enemydoorTButton.Checked)
            {
                enemyDoorMode.Delete();
                enemyDoorPanel.Visible = false;
            }
            mainPicturebox.Refresh();
        }

        private void selectedobjectCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (objMode.selectedObject != null)
                {
                    objMode.selectedObject.id = (byte)(selectedobjectCombobox.SelectedIndex * 2);
                    mainPicturebox.Refresh();
                }
            }
        }

        private void textEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextEditor te = new TextEditor(game);
            te.ShowDialog();
        }

        private void passwordEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordEditor pe = new PasswordEditor(game);
            pe.ShowDialog();
        }

        private void selectedItemCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (itemMode.selectedObject != null)
                {
                    itemMode.selectedObject.id = itemList[selectedItemCombobox.SelectedIndex].id;
                    if ((itemMode.selectedObject.id & 0x20) == 0x20)
                    {
                        itemsubtypeTextbox.Enabled = true;
                    }
                    else
                    {
                        itemsubtypeTextbox.Enabled = false;
                    }
                    mainPicturebox.Refresh();
                }
                
            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sprTButton.Checked)
            {
                spriteMode.Copy();
            }
            else if (objectTButton.Checked)
            {
                objMode.Copy();
            }
            else if (itemTButton.Checked)
            {
                itemMode.Copy();
            }
            else if (transitionsTButton.Checked)
            {
                transitionMode.Copy();
            }
            else if (bg1TButton.Checked)
            {
                bgMode.Copy();
            }
            else if (bg2TButton.Checked)
            {
                bgMode.Copy();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sprTButton.Checked)
            {
                spriteMode.Cut();
            }
            else if (objectTButton.Checked)
            {
                objMode.Cut();
            }
            else if (itemTButton.Checked)
            {
                itemMode.Cut();
            }
            else if (transitionsTButton.Checked)
            {
                transitionMode.Cut();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sprTButton.Checked)
            {
                spriteMode.Paste();
            }
            else if (objectTButton.Checked)
            {
                objMode.Paste();
            }
            else if (itemTButton.Checked)
            {
                itemMode.Paste();
            }
            else if (transitionsTButton.Checked)
            {
                transitionMode.Paste();
            }
            else if (bg1TButton.Checked)
            {
                bgMode.Paste();
            }
            else if (bg2TButton.Checked)
            {
                bgMode.Paste();
            }
        }

        private void selectedSpriteCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (spriteMode.selectedObject != null)
                {
                    spriteMode.selectedObject.id = (byte)(selectedSpriteCombobox.SelectedIndex * 2);
                    mainPicturebox.Refresh();
                }

            }
        }

        private void sprParamTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (spriteMode.selectedObject != null)
                {
                    byte b = 0;
                    byte.TryParse(sprParamTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    spriteMode.selectedObject.unkn = b;
                    mainPicturebox.Refresh();
                }

            }
        }

        private void sprSubtypeTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (spriteMode.selectedObject != null)
                {
                    byte b = 0;
                    byte.TryParse(sprSubtypeTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    spriteMode.selectedObject.param = b;
                    mainPicturebox.Refresh();
                }

            }
        }

        private void hooktypeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (hookMode.selectedObject != null)
                {
                    hookMode.selectedObject.type = (byte)(hooktypeCombobox.SelectedIndex);
                    if (hookMode.selectedObject.type == 0)
                    {
                        hookMode.selectedObject.x2 = (byte)(hookMode.selectedObject.x + 96);
                        hookMode.selectedObject.y2 = (byte)(hookMode.selectedObject.y);
                    }
                    else if (hookMode.selectedObject.type == 1)
                    {
                        hookMode.selectedObject.x2 = (byte)(hookMode.selectedObject.x + 128);
                        hookMode.selectedObject.y2 = (byte)(hookMode.selectedObject.y);
                    }
                    else if (hookMode.selectedObject.type == 2)
                    {
                        hookMode.selectedObject.x2 = (byte)(hookMode.selectedObject.x);
                        hookMode.selectedObject.y2 = (byte)(hookMode.selectedObject.y + 96);
                    }
                    else if (hookMode.selectedObject.type == 3)
                    {
                        hookMode.selectedObject.x2 = (byte)(hookMode.selectedObject.x);
                        hookMode.selectedObject.y2 = (byte)(hookMode.selectedObject.y + 128);
                    }
                    mainPicturebox.Refresh();
                }

            }
        }

        private void lock8x8Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            GlobalOptions.movementLock8x8 = lock8x8Checkbox.Checked;
        }

        private void hookPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void transitionDirCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (transitionMode.selectedObject != null)
                {
                    byte a = (byte)((transitionDirCombobox.SelectedIndex << 4));
                    byte b = (byte)(transitionMode.selectedObject.dir & 0x0F);
                    transitionMode.selectedObject.dir = (byte)(b | a);
                    mainPicturebox.Refresh();
                }

            }
        }

        private void transitiontypeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (transitionMode.selectedObject != null)
                {
                    byte a = 0;
                    if (transitiontypeCombobox.SelectedIndex == 0)
                    {
                        a = 0x02;
                    }
                    else if (transitiontypeCombobox.SelectedIndex == 1)
                    {
                        a = 0x04;
                    }
                    else if (transitiontypeCombobox.SelectedIndex == 2)
                    {
                        a = 0x0F;
                    }
                    byte b = (byte)((transitionDirCombobox.SelectedIndex & 0x0F) << 4);
                    transitionMode.selectedObject.dir = (byte)(b + a);
                    mainPicturebox.Refresh();
                    //transitionMode.selectedObject.dir = (byte)((transitionMode.selectedObject.dir & 0x0F) + transitionDirCombobox.SelectedIndex);
                }
            }

        }

        private void transitiondestmapTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (transitionMode.selectedObject != null)
                {
                    byte b = 0;
                    byte.TryParse(transitiondestmapTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    transitionMode.selectedObject.tomap = b;
                }
            }
        }

        private void transitiondestxTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (transitionMode.selectedObject != null)
                {
                    byte b = 0;
                    byte.TryParse(transitiondestxTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    transitionMode.selectedObject.xDest = b;
                }
            }
        }

        private void transitiondestyTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (transitionMode.selectedObject != null)
                {
                    byte b = 0;
                    byte.TryParse(transitiondestyTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    transitionMode.selectedObject.yDest = b;
                }
            }
        }

        private void alternatepalTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                byte b = 0;
                byte.TryParse(alternatepalTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                game.levels[game.selectedLevel].maps[game.selectedMap].altPal = b;
            }
        }

        private void animatedpalTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                byte b = 0;
                byte.TryParse(animatedpalTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                b = (byte)((b / 2) * 2);
                if (b > 0x40)
                {
                    b = 0x40;
                }
                game.levels[game.selectedLevel].maps[game.selectedMap].animatedPal = b;
                UpdatePalette();
                UpdateGFX();
                vramPicturebox.Refresh();
                //UpdateMap();
                mainPicturebox.Refresh();
            }
        }

        private void transitionvisibleVutton_Click(object sender, EventArgs e)
        {
            GlobalOptions.viewBg1 = bg1visibleButton.Checked;
            GlobalOptions.viewBg2 = bg2visibleButton.Checked;
            GlobalOptions.viewItem = itemvisibleButton.Checked;
            GlobalOptions.viewObject = objvisibleButton.Checked;
            GlobalOptions.viewSprite = sprvisibleButton.Checked;
            GlobalOptions.viewTransition = transitionvisibleButton.Checked;
            mainPicturebox.Refresh();
        }

        private void darkCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                game.levels[game.selectedLevel].maps[game.selectedMap].dark = darkCheckbox.Checked;
            }
        }

        private void iceCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {

                game.levels[game.selectedLevel].maps[game.selectedMap].ice = iceCheckbox.Checked;
            }
        }

        private void drawswitchCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (blockDoorMode.selectedObject != null)
                {
                    blockDoorMode.selectedObject.drawswitch = drawswitchCheckbox.Checked;
                    blockDoorMode.selectedObject.saved = savedoorCheckbox.Checked;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void playtestTButton_Click(object sender, EventArgs e)
        {
            if (SaveROM())
            {
                MessageBox.Show("Playtest Cancelled");
                return;
            }
            byte[] backupROM = (byte[])game.rom.data.Clone();
            if (File.Exists("temp.sfc"))
            {
                File.Delete("temp.sfc");
            }


            

            asmManager.buildASM();
            game.anyChange = false;
            
            Asar.patch("ASM//temp.asm", ref game.rom.data);
            Asarerror[] errors = Asar.geterrors();
            foreach (Asarerror er in errors)
            {
                Console.WriteLine(er.Fullerrdata);
            }


            FileStream fs = new FileStream("temp.sfc", FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(game.rom.data, 0, game.rom.data.Length);
            fs.Close();
            Process p = Process.Start("temp.sfc");
            game.rom.data = (byte[])backupROM.Clone();
        }

        private void playdebugTbutton_Click(object sender, EventArgs e)
        {

            if (SaveROM())
            {
                MessageBox.Show("Playtest debug Cancelled");
                return;
            }
            byte[] backupROM = (byte[])game.rom.data.Clone();


            if (File.Exists("temp.sfc"))
            {
                File.Delete("temp.sfc");
            }

            Asarerror[] errors = Asar.geterrors();
            foreach (Asarerror er in errors)
            {
                Console.WriteLine(er.Fullerrdata);
            }

            Asar.patch("ASM//required//debug.asm", ref game.rom.data);

            asmManager.buildASM();
            game.anyChange = false;
            Asar.patch("ASM//temp.asm", ref game.rom.data);

            game.rom.data[0x057E01] = game.selectedMap; //set map
            game.rom.data[0x057E05] = game.selectedLevel; //set map
            FileStream fs = new FileStream("temp.sfc", FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(game.rom.data, 0, game.rom.data.Length);
            fs.Close();
            Process p = Process.Start("temp.sfc");
            game.rom.data = (byte[])backupROM.Clone();
        }

        private void saveslotBlockDoor_ValueChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (blockDoorMode.selectedObject != null)
                {
                    blockDoorMode.selectedObject.doorRam = (byte)saveslotBlockDoor.Value;
                }
                mainPicturebox.Refresh();
            }

        }

        private void blockdoordirCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (blockDoorMode.selectedObject != null)
                {
                    blockDoorMode.selectedObject.doorDir = (byte)(blockdoordirCombobox.SelectedIndex * 2);
                }
                mainPicturebox.Refresh();
            }

        }

        private void lockeddoordirCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (lockedDoorMode.selectedObject != null)
                {
                    lockedDoorMode.selectedObject.doorDir = (byte)(lockeddoordirCombobox.SelectedIndex * 2);
                }
                mainPicturebox.Refresh();
            }

        }

        private void bosskeyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (lockedDoorMode.selectedObject != null)
                {
                    lockedDoorMode.selectedObject.boss = bosskeyCheckbox.Checked;
                }
                mainPicturebox.Refresh();
            }
        }

        private void saveslotlockedDoor_ValueChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (lockedDoorMode.selectedObject != null)
                {
                    lockedDoorMode.selectedObject.doorRam = (byte)saveslotlockedDoor.Value;
                }
                mainPicturebox.Refresh();
            }
        }

        private void scratchpadPicturebox_Paint(object sender, PaintEventArgs e)
        {
            if (game != null)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                e.Graphics.Clear(Color.Black);
                e.Graphics.DrawImage(GFX.scratch16Bitmap, new Rectangle(0, 0, 256, 15360));
                /*int tx = (bgMode.selectedTile % 8);
                int ty = (bgMode.selectedTile / 8);
                e.Graphics.DrawRectangle(Pens.White, new Rectangle(tx * 32, ty * 32, 32, 32));*/
                if (scratchSelecting == false)
                {
                    int tx = mousePosScratchpadX;
                    int ty = mousePosScratchpadY;
                    e.Graphics.DrawRectangle(Pens.White, new Rectangle(tx * 32, ty * 32, bgMode.selectedWidth * 32, bgMode.selectedHeight * 32));
                }
                else
                {
                    int tx = mousePosScratchpadDownX;
                    int ty = mousePosScratchpadDownY;
                    e.Graphics.DrawRectangle(Pens.White, new Rectangle(tx * 32, ty * 32, scratchpadWidth * 32, scratchpadHeight * 32));
                }
            }
        }
        int mousePosScratchpadX = 0;
        int mousePosScratchpadY = 0;
        int mousePosScratchpadDownX = -1;
        int mousePosScratchpadDownY = -1;
        int scratchpadWidth = 0;
        int scratchpadHeight = 0;
        bool scratchSelecting = false;
        private void scratchpadPicturebox_MouseDown(object sender, MouseEventArgs e)
        {

            int tx = (e.X / 32);
            int ty = (e.Y / 32);
            if (e.Button == MouseButtons.Left)
            {
                for (int h = 0; h < bgMode.selectedHeight; h++)
                {
                    for (int w = 0; w < bgMode.selectedWidth; w++)
                    {
                        if (tx + w + ((ty + h) * 8) < GFX.scratchpadTiles.Length)
                        {
                            GFX.scratchpadTiles[tx + w + ((ty + h) * 8)] = bgMode.selectedTiles[w + (h * bgMode.selectedWidth)];
                        }

                    }
                }

                UpdateMap();
                scratchpadPicturebox.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                scratchSelecting = true;
                scratchpadWidth = 1;
                scratchpadHeight = 1;
                mousePosScratchpadDownX = tx;
                mousePosScratchpadDownY = ty;
            }

        }

        private void scratchpadPicturebox_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosScratchpadX = (e.X / 32);
            mousePosScratchpadY = (e.Y / 32);
            scratchpadWidth = (mousePosScratchpadX - mousePosScratchpadDownX) + 1;
            scratchpadHeight = (mousePosScratchpadY - mousePosScratchpadDownY) + 1;
            if (scratchpadWidth < 1)
            {
                scratchpadWidth = 1;
            }
            if (scratchpadHeight < 1)
            {
                scratchpadHeight = 1;
            }
            scratchpadPicturebox.Refresh();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bgMode.Undo();
            UpdateMap();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bgMode.Redo();
            UpdateMap();
        }

        private void scratchpadPicturebox_MouseUp(object sender, MouseEventArgs e)
        {
            if (scratchSelecting)
            {
                bgMode.selectedTiles = new ushort[scratchpadHeight * scratchpadWidth];
                for (int h = 0; h < scratchpadHeight; h++)
                {
                    for (int w = 0; w < scratchpadWidth; w++)
                    {
                        bgMode.selectedTiles[w + (h * scratchpadWidth)] = GFX.scratchpadTiles[mousePosScratchpadDownX + w + ((mousePosScratchpadDownY + h) * 8)];
                        bgMode.selectedWidth = (byte)scratchpadWidth;
                        bgMode.selectedHeight = (byte)scratchpadHeight;
                    }
                }
                editingtilePicturebox.Refresh();

                scratchSelecting = false;
            }
        }

        private void saveScratchpadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sf.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] data = new byte[0xEFF * 2];

                for (int i = 0; i < 0xEFF; i++)
                {
                    data[(i * 2)] = (byte)(GFX.scratchpadTiles[i] & 0xFF);
                    data[(i * 2) + 1] = (byte)((GFX.scratchpadTiles[i] & 0xFF00) >> 8);
                }

                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }

        private void loadScratchpadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            if (of.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(of.FileName, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[0xEFF * 2];
                fs.Read(data, 0, data.Length);
                fs.Close();
                for (int i = 0; i < 0xEFF; i++)
                {
                    GFX.scratchpadTiles[i] = (ushort)(data[(i * 2)] + (data[(i * 2) + 1] << 8));
                }

                UpdateMap();
                scratchpadPicturebox.Refresh();
            }
        }

        private void scratchpadPicturebox_MouseLeave(object sender, EventArgs e)
        {

        }




        private void exportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will create many files make sure you are using a folder, you can type" +
                            " anything for the filename, do not rename the files but their size can be modified" +
                            " This is exporting raw snes gfx data that can be modified with YY-CHR or other snes gfx tools");
            SaveFileDialog sf = new SaveFileDialog();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                string path = Path.GetDirectoryName(sf.FileName);
                for (int i = 0; i < 15; i++)
                {
                    int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (i * 2);
                    int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
                    int gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (i * 5);
                    int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

                    int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
                    int length = game.rom.ReadShort(gfxPtrPC + 3);
                    byte[] s = Compression.DecompressGFX(game.rom.data, addr, length);


                    FileStream fs = new FileStream(path + "\\" + "BG" + i.ToString("X2") + ".bin", FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(s, 0, s.Length);
                    fs.Close();

                    //Create a folder containing all BG Gfx
                }


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
                fss.Close();


            }
        }

        private void importAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();

            int expandedPos = 0x0C0000;
            if (of.ShowDialog() == DialogResult.OK)
            {
                string path = Path.GetDirectoryName(of.FileName);
                for (int i = 0; i < 15; i++)
                {
                    int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.BGGfxValuesAddrDest_1) + (i * 2);
                    int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
                    int gfxPtrSnes = game.rom.ReadLong(Constants.BGGfxValuesPtr_1) + (i * 5);
                    int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

                    int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
                    int length = game.rom.ReadShort(gfxPtrPC + 3);


                    FileStream fs = new FileStream(path + "\\" + "BG" + i.ToString("X2") + ".bin", FileMode.Open, FileAccess.Read);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    fs.Close();
                    

                    byte[] s = Compression.CompressGfx(data);

                    if (data.Length > length) // if new length > previous length then use expanded region
                    {
                        game.rom.WriteLong(gfxPtrPC, Utils.PcToSnes(expandedPos));
                        game.rom.WriteShort(gfxPtrPC + 3, (short)data.Length);
                        addr = expandedPos;
                    }
                    game.rom.WriteBytes(addr, s);
                    if (addr >= 0x0C0000)
                    {
                        expandedPos += data.Length;
                    }

                }

                FileStream fss = new FileStream(path + "\\" + "Items" + ".bin", FileMode.Open, FileAccess.Read);
                byte[] dataItem = new byte[fss.Length];
                fss.Read(dataItem, 0, (int)fss.Length);
                fss.Close();

                byte[] sItem = Compression.CompressGfx(dataItem);
                if (sItem.Length > 0x1D00)
                {
                    MessageBox.Show("Not enough space to import Items.bin this file will be ignored");

                }
                else
                {
                    game.rom.WriteBytes(0x060000, sItem);
                }


                fss = new FileStream(path + "\\" + "Items2" + ".bin", FileMode.Open, FileAccess.Read);
                dataItem = new byte[fss.Length];
                fss.Read(dataItem, 0, (int)fss.Length);
                fss.Close();

                sItem = Compression.CompressGfx(dataItem);
                if (sItem.Length > 0x6000)
                {
                    MessageBox.Show("Not enough space to import Items.bin this file will be ignored");

                }
                else
                {
                    game.rom.WriteBytes(0x6C55C, sItem);
                }

                for (int i = 0; i < 35; i++)
                {
                    int gfxdestPtrSnes = 0x830000 + game.rom.ReadShort(Constants.SpritesGfx_Address) + (i * 2);
                    int dest = game.rom.ReadByte((Utils.SnesToPc(gfxdestPtrSnes)) + 1) << 10;
                    int gfxPtrSnes = game.rom.ReadLong(Constants.SpritesGfx_Address) + (i * 5);
                    int gfxPtrPC = Utils.SnesToPc(gfxPtrSnes);

                    int addr = Utils.SnesToPc(game.rom.ReadLong(gfxPtrPC));
                    int length = game.rom.ReadShort(gfxPtrPC + 3);


                    FileStream fs = new FileStream(path + "\\" + "SPR" + i.ToString("X2") + ".bin", FileMode.Open, FileAccess.Read);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    fs.Close();


                    byte[] s = Compression.CompressGfx(data);
                    //Console.WriteLine("GFX LENGTH FOR SHEET " + i.ToString("X2") + " = " + s.Length.ToString("X4"));
                    if (data.Length > length)
                    {
                                game.rom.WriteLong(gfxPtrPC, Utils.PcToSnes(expandedPos));
                                game.rom.WriteShort(gfxPtrPC + 3, (short)data.Length);
                                addr = expandedPos;
                    }
                    game.rom.WriteBytes(addr, s);
                    if (addr >= 0x0C0000)
                    {
                        expandedPos += data.Length;
                    }

                }

                UpdateGFX();
                bgMode.redrawTiles8();
                vramPicturebox.Refresh();
                palettePicturebox.Refresh();
                mainPicturebox.Refresh();
                editingtilePicturebox.Refresh();
                tiles16Picturebox.Refresh();
                scratchpadPicturebox.Refresh();
            }

        }

        private void paletteEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PaletteEditor pe = new PaletteEditor(game);
            pe.ShowDialog();
            
            

            UpdateGFX(); // write new palettes
            UpdatePalette(); // update the palettes on screen
            
            vramPicturebox.Refresh();
            palettePicturebox.Refresh();
            mainPicturebox.Refresh();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            byte[] data = new byte[0x300];

            for (int i = 0; i < 256; i++)
            {
                data[(i * 3)] = GFX.palette[i].R;
                data[(i * 3) + 1] = GFX.palette[i].G;
                data[(i * 3) + 2] = GFX.palette[i].B;
            }

            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "raw palette file (*.pal)|*.pal";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sf.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }

        private void editingtilePicturebox_MouseDown(object sender, MouseEventArgs e)
        {
            //DrawEditing16
            byte selectedCollision = game.collisions[bgMode.selectedTiles[0]];

            int tileIndex = 0; //0, 1
                               //2, 3

            if (e.X < 32)
            {
                tileIndex = 0;
                if (e.Y > 32)
                {
                    tileIndex = 2;
                }
            }
            if (e.X > 32)
            {
                tileIndex = 1;
                if (e.Y > 32)
                {
                    tileIndex = 3;
                }
            }
            byte hbyte = 0;
            if (collisionCombobox.SelectedIndex == 0)
            {
                hbyte = 0x00;
            }
            else if (collisionCombobox.SelectedIndex == 1)
            {
                hbyte = 0x30;
            }
            else if (collisionCombobox.SelectedIndex == 2)
            {
                hbyte = 0x70;
            }
            else if (collisionCombobox.SelectedIndex == 3)
            {
                hbyte = 0x80;
            }
            else if (collisionCombobox.SelectedIndex == 4)
            {
                hbyte = 0x90;
            }
            else if (collisionCombobox.SelectedIndex == 5)
            {
                hbyte = 0xC0;
            }
            else if (collisionCombobox.SelectedIndex == 6)
            {
                hbyte = 0xE0;
            }
            else if (collisionCombobox.SelectedIndex == 7)
            {
                hbyte = 0xF0;
            }
            else if (collisionCombobox.SelectedIndex == 8)
            {
                hbyte = 0x10;
            }

            if (ModifierKeys == Keys.Shift)
            {

                byte lowCollision = (byte)(selectedCollision & 0x0F);
                byte[] tempArray = (byte[])collisionTable[lowCollision].Clone();


                if (tempArray[tileIndex] == 1)
                {
                    tempArray[tileIndex] = 0;
                }
                else
                {
                    tempArray[tileIndex] = 1;
                }

                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (tempArray[j] != collisionTable[i][j])
                        {
                            break;
                        }
                        if (j == 3)
                        {
                            game.collisions[bgMode.selectedTiles[0]] = (byte)(i + hbyte);
                            editingtilePicturebox.Refresh();
                            return;
                        }
                    }
                }

                game.collisions[bgMode.selectedTiles[0]] = (byte)(hbyte);
                editingtilePicturebox.Refresh();
                return;
            }

            if (e.Button == MouseButtons.Right) //select tile8
            {

                if (tileIndex == 0)
                {
                    selectedTile8 = game.tiles16[bgMode.selectedTiles[0]].tile0.id;
                    selectedPal8 = game.tiles16[bgMode.selectedTiles[0]].tile0.palette;
                    selectedMx8 = game.tiles16[bgMode.selectedTiles[0]].tile0.h;
                    selectedMy8 = game.tiles16[bgMode.selectedTiles[0]].tile0.v;
                    selectedPrior8 = game.tiles16[bgMode.selectedTiles[0]].tile0.o;
                }
                else if (tileIndex == 1)
                {
                    selectedTile8 = game.tiles16[bgMode.selectedTiles[0]].tile1.id;
                    selectedPal8 = game.tiles16[bgMode.selectedTiles[0]].tile1.palette;
                    selectedMx8 = game.tiles16[bgMode.selectedTiles[0]].tile1.h;
                    selectedMy8 = game.tiles16[bgMode.selectedTiles[0]].tile1.v;
                    selectedPrior8 = game.tiles16[bgMode.selectedTiles[0]].tile1.o;
                }
                else if (tileIndex == 2)
                {
                    selectedTile8 = game.tiles16[bgMode.selectedTiles[0]].tile2.id;
                    selectedPal8 = game.tiles16[bgMode.selectedTiles[0]].tile2.palette;
                    selectedMx8 = game.tiles16[bgMode.selectedTiles[0]].tile2.h;
                    selectedMy8 = game.tiles16[bgMode.selectedTiles[0]].tile2.v;
                    selectedPrior8 = game.tiles16[bgMode.selectedTiles[0]].tile2.o;
                }
                else if (tileIndex == 3)
                {
                    selectedTile8 = game.tiles16[bgMode.selectedTiles[0]].tile3.id;
                    selectedPal8 = game.tiles16[bgMode.selectedTiles[0]].tile3.palette;
                    selectedMx8 = game.tiles16[bgMode.selectedTiles[0]].tile3.h;
                    selectedMy8 = game.tiles16[bgMode.selectedTiles[0]].tile3.v;
                    selectedPrior8 = game.tiles16[bgMode.selectedTiles[0]].tile3.o;
                }

                fromForm = true;
                selectedPalette = selectedPal8;
                if (selectedMx8 == 1)
                {
                    editing16mxCheckbox.Checked = true;
                }
                else
                {
                    editing16mxCheckbox.Checked = false;
                }

                if (selectedMy8 == 1)
                {
                    editing16myCheckbox.Checked = true;
                }
                else
                {
                    editing16myCheckbox.Checked = false;
                }

                if (selectedPrior8 == 1)
                {
                    editor16priorityCheckbox.Checked = true;
                }
                else
                {
                    editor16priorityCheckbox.Checked = false;
                }

                if ((selectedCollision & 0xF0) == 0x10)//Damage
                {
                    collisionCombobox.SelectedIndex = 8;
                }
                if ((selectedCollision & 0xF0) == 0xF0)//Water
                {
                    collisionCombobox.SelectedIndex = 7;
                }
                else if ((selectedCollision & 0xF0) == 0xE0)//Solid3
                {
                    collisionCombobox.SelectedIndex = 6;
                }
                else if ((selectedCollision & 0xF0) == 0xC0)//Solid2
                {
                    collisionCombobox.SelectedIndex = 5;
                }
                else if ((selectedCollision & 0xF0) == 0x90)//text
                {
                    collisionCombobox.SelectedIndex = 4;
                }
                else if ((selectedCollision & 0xF0) == 0x80)//Solid1
                {
                    collisionCombobox.SelectedIndex = 3;
                }
                else if ((selectedCollision & 0xF0) == 0x70)//Stairs
                {
                    collisionCombobox.SelectedIndex = 2;
                }
                else if ((selectedCollision & 0xF0) == 0x30)//Hole
                {
                    collisionCombobox.SelectedIndex = 1;
                }
                else if ((selectedCollision & 0xF0) == 0x00)//Passable
                {
                    collisionCombobox.SelectedIndex = 0;
                }

                UpdatePalette();

                vramPicturebox.Refresh();
                palettePicturebox.Refresh();

            }
            else if (e.Button == MouseButtons.Left)
            {
                if (tileIndex == 0)
                {
                    game.tiles16[bgMode.selectedTiles[0]].tile0.id = selectedTile8;
                    game.tiles16[bgMode.selectedTiles[0]].tile0.palette = selectedPal8;
                    game.tiles16[bgMode.selectedTiles[0]].tile0.h = (ushort)selectedMx8;
                    game.tiles16[bgMode.selectedTiles[0]].tile0.v = (ushort)selectedMy8;
                    game.tiles16[bgMode.selectedTiles[0]].tile0.o = (ushort)selectedPrior8;
                }
                else if (tileIndex == 1)
                {
                    game.tiles16[bgMode.selectedTiles[0]].tile1.id = selectedTile8;
                    game.tiles16[bgMode.selectedTiles[0]].tile1.palette = selectedPal8;
                    game.tiles16[bgMode.selectedTiles[0]].tile1.h = (ushort)selectedMx8;
                    game.tiles16[bgMode.selectedTiles[0]].tile1.v = (ushort)selectedMy8;
                    game.tiles16[bgMode.selectedTiles[0]].tile1.o = (ushort)selectedPrior8;
                }
                else if (tileIndex == 2)
                {
                    game.tiles16[bgMode.selectedTiles[0]].tile2.id = selectedTile8;
                    game.tiles16[bgMode.selectedTiles[0]].tile2.palette = selectedPal8;
                    game.tiles16[bgMode.selectedTiles[0]].tile2.h = (ushort)selectedMx8;
                    game.tiles16[bgMode.selectedTiles[0]].tile2.v = (ushort)selectedMy8;
                    game.tiles16[bgMode.selectedTiles[0]].tile2.o = (ushort)selectedPrior8;
                }
                else if (tileIndex == 3)
                {
                    game.tiles16[bgMode.selectedTiles[0]].tile3.id = selectedTile8;
                    game.tiles16[bgMode.selectedTiles[0]].tile3.palette = selectedPal8;
                    game.tiles16[bgMode.selectedTiles[0]].tile3.h = (ushort)selectedMx8;
                    game.tiles16[bgMode.selectedTiles[0]].tile3.v = (ushort)selectedMy8;
                    game.tiles16[bgMode.selectedTiles[0]].tile3.o = (ushort)selectedPrior8;
                }
                UpdateMap();
                UpdateGFX();
                UpdatePalette();

                //GFX.DrawTiles16(game.tiles16.ToArray());
                bgMode.redrawTiles8();
                vramPicturebox.Refresh();
                palettePicturebox.Refresh();
                mainPicturebox.Refresh();
                editingtilePicturebox.Refresh();
                tiles16Picturebox.Refresh();
                scratchpadPicturebox.Refresh();
            }

        }

        byte[][] collisionTable = new byte[15][]
        {
            new byte[4]{1,1,1,1 }, new byte[4]{1,0,0,0 }, new byte[4]{0,1,0,0 }, new byte[4]{0,0,1,0 },
            new byte[4]{0,0,0,1 }, new byte[4]{1,1,0,0 }, new byte[4]{0,0,1,1 }, new byte[4]{1,0,1,0 },
            new byte[4]{0,1,0,1 }, new byte[4]{1,1,0,1 }, new byte[4]{1,0,1,1 }, new byte[4]{0,1,1,1 },
            new byte[4]{1,1,1,0 }, new byte[4]{1,0,0,1 }, new byte[4]{0,1,1,0 }
        };

        Font f = new Font("Arial", 10, FontStyle.Bold);

        private void editingtilePicturebox_Paint(object sender, PaintEventArgs e)
        {
            if (game != null)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                e.Graphics.Clear(Color.Black);

                fromForm = true;
                if (bgMode.selectedTiles.Length >= 1)
                {
                    byte selectedCollision = game.collisions[bgMode.selectedTiles[0]];
                    if ((selectedCollision & 0xF0) == 0x10)//Damage
                    {
                        collisionCombobox.SelectedIndex = 8;
                    }
                    if ((selectedCollision & 0xF0) == 0xF0)//Water
                    {
                        collisionCombobox.SelectedIndex = 7;
                    }
                    else if ((selectedCollision & 0xF0) == 0xE0)//Solid3
                    {
                        collisionCombobox.SelectedIndex = 6;
                    }
                    else if ((selectedCollision & 0xF0) == 0xC0)//Solid2
                    {
                        collisionCombobox.SelectedIndex = 5;
                    }
                    else if ((selectedCollision & 0xF0) == 0x90)//Text
                    {
                        collisionCombobox.SelectedIndex = 4;
                    }
                    else if ((selectedCollision & 0xF0) == 0x80)//Solid1
                    {
                        collisionCombobox.SelectedIndex = 3;
                    }
                    else if ((selectedCollision & 0xF0) == 0x70)//Stairs
                    {
                        collisionCombobox.SelectedIndex = 2;
                    }
                    else if ((selectedCollision & 0xF0) == 0x30)//Hole
                    {
                        collisionCombobox.SelectedIndex = 1;
                    }
                    else if ((selectedCollision & 0xF0) == 0x00)//Passable
                    {
                        collisionCombobox.SelectedIndex = 0;
                    }
                    fromForm = false;


                    GFX.DrawEditing16(game.tiles16.ToArray(), bgMode.selectedTiles[0]);

                    string c = "O";
                    if ((selectedCollision & 0xF0) == 0x10)//Damage
                    {
                        c = "D";
                    }
                    if ((selectedCollision & 0xF0) == 0xF0)//Water
                    {
                        c = "W";
                    }
                    else if ((selectedCollision & 0xF0) == 0xE0)//Solid3
                    {
                        c = "S";
                    }
                    else if ((selectedCollision & 0xF0) == 0xC0)//Solid2
                    {
                        c = "S";
                    }
                    else if ((selectedCollision & 0xF0) == 0x90)//Text
                    {
                        c = "T";
                    }
                    else if ((selectedCollision & 0xF0) == 0x80)//Solid1
                    {
                        c = "S";
                    }
                    else if ((selectedCollision & 0xF0) == 0x70)//Stairs
                    {
                        c = "S";
                    }
                    else if ((selectedCollision & 0xF0) == 0x30)//Hole
                    {
                        c = "H";
                    }
                    else if ((selectedCollision & 0xF0) == 0x00)//Passable
                    {
                        c = "O";
                    }


                    byte lowCollision = (byte)(selectedCollision & 0x0F);


                    e.Graphics.DrawImage(GFX.editingTile16Bitmap, new Rectangle(0, 0, 64, 64), 0, 0, 16, 16, GraphicsUnit.Pixel);
                    //if (showcollisionCheckbox.Checked)
                    //{

                    for (int i = 0; i < 4; i++)
                    {
                        if (collisionTable[lowCollision][i] == 1)
                        {
                            e.Graphics.DrawString(c, f, Brushes.White, new Point(((i % 2) * 32) + 8, ((i / 2) * 32) + 8));
                        }
                        else
                        {
                            e.Graphics.DrawString("O", f, Brushes.White, new Point(((i % 2) * 32) + 8, ((i / 2) * 32) + 8));
                        }
                    }
                    //}


                }
            }

        }

        private void editing16mxCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (editing16mxCheckbox.Checked)
            {
                selectedMx8 = 1;
            }
            else
            {
                selectedMx8 = 0;
            }
            if (editing16myCheckbox.Checked)
            {
                selectedMy8 = 1;
            }
            else
            {
                selectedMy8 = 0;
            }

            if (editor16priorityCheckbox.Checked)
            {
                selectedPrior8 = 1;
            }
            else
            {
                selectedPrior8 = 0;
            }

            UpdateGFX();

        }

        private void showcollisionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //editingtilePicturebox.Refresh();
        }

        private void collisionCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                byte selectedCollision = game.collisions[bgMode.selectedTiles[0]];
                byte hbyte = 0;
                if (collisionCombobox.SelectedIndex == 0)
                {
                    hbyte = 0x00;
                }
                else if (collisionCombobox.SelectedIndex == 1)
                {
                    hbyte = 0x30;
                }
                else if (collisionCombobox.SelectedIndex == 2)
                {
                    hbyte = 0x70;
                }
                else if (collisionCombobox.SelectedIndex == 3)
                {
                    hbyte = 0x80;
                }
                else if (collisionCombobox.SelectedIndex == 4)
                {
                    hbyte = 0x90;
                }
                else if (collisionCombobox.SelectedIndex == 5)
                {
                    hbyte = 0xC0;
                }
                else if (collisionCombobox.SelectedIndex == 6)
                {
                    hbyte = 0xE0;
                }
                else if (collisionCombobox.SelectedIndex == 7)
                {
                    hbyte = 0xF0;
                }
                else if (collisionCombobox.SelectedIndex == 8)
                {
                    hbyte = 0x10;
                }

                byte lowCollision = (byte)(selectedCollision & 0x0F);



                game.collisions[bgMode.selectedTiles[0]] = (byte)(lowCollision + hbyte);
                editingtilePicturebox.Refresh();
                return;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (game != null)
            {
                if (game.anyChange)
                {
                    var window = MessageBox.Show(
                        "There is unsaved changes are you sure you want to exit?",
                        "Are you sure?",
                        MessageBoxButtons.YesNo);

                    e.Cancel = (window == DialogResult.No);
                }
            }
        }

        string filename = "";
        private void saveROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.anyChange = false;
            SaveROM();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Write);
            fs.Write(game.rom.data, 0, game.rom.data.Length);
            fs.Close();
        }

        private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "SNES ROM file (*.sfc)|*.sfc|SNES ROM file (*.smc)|*.smc";
            if (of.ShowDialog() == DialogResult.OK)
            {
                filename = of.FileName;
            }

            LoadGame(filename);
        }

        private void saveROMAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.anyChange = false;
            SaveFileDialog of = new SaveFileDialog();
            of.Filter = "SNES ROM file (*.sfc)|*.sfc|SNES ROM file (*.smc)|*.smc";
            if (of.ShowDialog() == DialogResult.OK)
            {
                filename = of.FileName;
                SaveROM();
                FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(game.rom.data, 0, game.rom.data.Length);
                fs.Close();
            }
        }
        
        private void manageASMToolStripMenuItem_Click(object sender, EventArgs e)
        {

            asmManager = new AsmManagerNew(filename);
            asmManager.ShowDialog();

        }

        private void demorecorderTButton_Click(object sender, EventArgs e)
        {
            byte[] backupROM = (byte[])game.rom.data.Clone();

            Asar.patch("ASM//required//debug.asm", ref game.rom.data);
            Asar.patch("ASM//required//demorecorder.asm", ref game.rom.data);

            Asarerror[] errors = Asar.geterrors();
            foreach (Asarerror er in errors)
            {
                Console.WriteLine(er.Fullerrdata);
            }




            SaveROM();
            game.rom.data[0x057E01] = game.selectedMap; //set map
            game.rom.data[0x057E05] = game.selectedLevel; //set map
            if (File.Exists("temp.sfc"))
            {
                File.Delete("temp.sfc");
            }

            FileStream fs = new FileStream("temp.sfc", FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(game.rom.data, 0, game.rom.data.Length);
            fs.Close();
            Process p = Process.Start("temp.sfc");
            game.rom.data = (byte[])backupROM.Clone();
        }

        private void saveEnemyDoorCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (saveEnemyDoorCheckbox.Checked)
            {
                enemydoorDIRPanel.Enabled = false;
            }
            else
            {
                enemydoorDIRPanel.Enabled = true;
            }
            if (!fromForm)
            {
                if (saveEnemyDoorCheckbox.Checked)
                {
                    enemyDoorMode.selectedObject.save = true;
                }
                else
                {
                    enemyDoorMode.selectedObject.save = false;
                }
            }
        }

        private void enemydoorpiratecountUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (enemyDoorMode.selectedObject != null)
                {
                    enemyDoorMode.selectedObject.enemyCount = (byte)enemydoorpiratecountUpDown.Value;
                }
            }
        }

        private void saveslotEnemyDoor_ValueChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (enemyDoorMode.selectedObject != null)
                {
                    enemyDoorMode.selectedObject.doorRamDir = (byte)saveslotEnemyDoor.Value;
                }
            }
        }

        private void enemydoorsizeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (enemyDoorMode.selectedObject != null)
                {
                    enemyDoorMode.selectedObject.doorSize = (byte)(enemydoorsizeCombobox.SelectedIndex * 2);
                }
                mainPicturebox.Refresh();
            }
        }

        private void enemydoordirectionCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (enemyDoorMode.selectedObject != null)
                {
                    byte index = 0;
                    if (enemydoordirectionCombobox.SelectedIndex == 0)
                    {
                        index = 0;
                    }
                    else if (enemydoordirectionCombobox.SelectedIndex == 1)
                    {
                        index = 6;
                    }
                    else if (enemydoordirectionCombobox.SelectedIndex == 2)
                    {
                        index = 2;
                    }
                    else if (enemydoordirectionCombobox.SelectedIndex == 3)
                    {
                        index = 4;
                    }
                    else if (enemydoordirectionCombobox.SelectedIndex == 4)
                    {
                        index = 0xFF;
                    }
                    enemyDoorMode.selectedObject.doorRamDir = index;
                }
            }
        }

        public void SetDarkTheme(Control pc)
        {
            Color darkMainColor = Color.FromArgb(48, 48, 48);
            Color darkBoxColor = Color.FromArgb(30, 30, 30);
            Color darkTextColor = Color.FromArgb(248, 248, 248);

            pc.BackColor = darkMainColor;
            pc.ForeColor = darkTextColor;

            if (pc.HasChildren)
            {
                foreach (Control c in pc.Controls)
                {
                    SetDarkTheme(c);
                    if (c is Panel)
                    {
                        c.ForeColor = darkTextColor;
                        c.BackColor = darkBoxColor;
                    }
                    else if (c is Label)
                    {
                        c.ForeColor = darkTextColor;
                    }
                    else if (c is TextBox)
                    {
                        c.ForeColor = darkTextColor;
                        c.BackColor = darkBoxColor;

                    }
                    else if (c is GroupBox)
                    {
                        c.ForeColor = darkTextColor;
                        c.BackColor = darkMainColor;
                        SetDarkTheme(c);
                    }
                    else
                    {
                        c.ForeColor = darkTextColor;
                        c.BackColor = darkMainColor;
                    }
                }
            }
        }

        private void darkThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDarkTheme(this);
        }

        private void itemsubtypeTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                if (itemMode.selectedObject != null)
                {
                    byte b = 0;
                    byte.TryParse(itemsubtypeTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
                    itemMode.selectedObject.ram = b;
                    mainPicturebox.Refresh();
                }

            }
        }

        private void editor16priorityCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (editor16priorityCheckbox.Checked)
            {
                selectedPrior8 = 1;
            }
            else
            {
                selectedPrior8 = 0;
            }
        }

        private void mainPicturebox_DoubleClick(object sender, EventArgs e)
        {
            if (transitionsTButton.Checked)
            {
                if (transitionMode.selectedObject != null)
                {
                   
                    if (transitionMode.selectedObject.tomap < game.levels[game.selectedLevel].mapCount)
                    {
                        game.selectedMap = transitionMode.selectedObject.tomap;
                    }
                    else
                    {
                        MessageBox.Show("The transitions is leading to a non-existing map id for this level!");
                    }
                    
                    selectedMapTextbox.Text = game.selectedMap.ToString("X2");
                }
            }
        }

        private void animatedPaletteToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = animatedPaletteToolStripMenuItem.Checked;
        }

        private void buildROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog of = new SaveFileDialog();

            of.Filter = "SNES ROM file (*.sfc)|*.sfc|SNES ROM file (*.smc)|*.smc";
            if (of.ShowDialog() == DialogResult.OK)
            {

                filename = of.FileName;
                SaveROM();
                byte[] backupROM = (byte[])game.rom.data.Clone();
                asmManager.buildASM();

                game.anyChange = false;
                Asar.patch("ASM//temp.asm", ref game.rom.data);
                Asarerror[] errors = Asar.geterrors();
                foreach (Asarerror er in errors)
                {
                    Console.WriteLine(er.Fullerrdata);
                }

                FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(game.rom.data, 0, game.rom.data.Length);
                fs.Close();
                game.rom.data = (byte[])backupROM.Clone();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void showScratchpadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label27.Visible = showScratchpadToolStripMenuItem.Checked;
            scratchpadPanel.Visible = showScratchpadToolStripMenuItem.Checked;
        }

        private void creditsEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte g1 = game.rom.ReadByte(Constants.SpriteSet_Data + 0x3F);
            DecompressDynamicSprGfx((byte)(g1 / 2), 0xE800 * 2);
            GFX.DrawTiles8Mirror(0, 0, selectedPalette);
            int PalPtrPC = 0x1FFFD;
            loadDynamicPalette(game.rom.ReadByte(PalPtrPC), game.rom.ReadByte(PalPtrPC + 1), game.rom.ReadByte(PalPtrPC + 2));
            UpdateSingleSprPaletteFromROM(0x5FEC0, 192);
            GFX.palette[0] = game.rom.ReadColor(0x5FEC0);
            
            //0x5FEC0 //the end palette


            palettePicturebox.Refresh();
            vramPicturebox.Refresh();
            UpdatePalette();
            //show form here
            CreditEditor ce = new CreditEditor(game);
            ce.ShowDialog();
            GFX.palette[0] = Color.Transparent;
            UpdateMap();
            UpdateGFX();
            UpdatePalette();
            palettePicturebox.Refresh();
            vramPicturebox.Refresh();
            //restore gfx here

        }

        private void clearAllMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove everything from the game?\r\nTiles, Items, Objects, Hooks, Doors, etc...","Warning",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int l = 0; l < 5; l++)
                {
                    for (int m = 0; m < game.levels[l].maps.Count; m++)
                    {
                        game.levels[l].maps[m].hooks.Clear();
                        game.levels[l].maps[m].planks.Clear();
                        game.levels[l].maps[m].sprites.Clear();
                        game.levels[l].maps[m].items.Clear();
                        game.levels[l].maps[m].transitions.Clear();
                        game.levels[l].maps[m].lockedDoors.Clear();
                        game.levels[l].maps[m].objects.Clear();
                        game.levels[l].maps[m].enemyDoors.Clear();
                        game.levels[l].maps[m].blockDoors.Clear();

                        for (int i = 0; i < 256; i++)
                        {
                            game.levels[l].maps[m].bg1tilemap16[i] = 0;
                            game.levels[l].maps[m].bg2tilemap16[i] = 0x131;

                            Tile16 t = game.tiles16[game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16[i]];
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[i] = t.tile0.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[i+1] = t.tile1.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[i+16] = t.tile2.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[i+17] = t.tile3.toShort();


                            t = game.tiles16[game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16[i]];
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[i] = t.tile0.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[i + 1] = t.tile1.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[i + 16] = t.tile2.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[i + 17] = t.tile3.toShort();

                        }
                        for (int i = 0; i < 64; i++)
                        {
                            game.levels[l].maps[m].bg1tilemap32[i] = 0;
                            game.levels[l].maps[m].bg2tilemap32[i] = 0;
                        }
                    }
                }
                
            }
        }
        
        private void musicViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mv.ShowDialog();
        }

        private void gfxEditornewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gfxEditor.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                game.tiles16[bgMode.selectedTiles[0] + i].tile0.id = (ushort)(selectedTile8 + (i *2)) ;
                game.tiles16[bgMode.selectedTiles[0] + i].tile0.palette = selectedPal8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile0.h = (ushort)selectedMx8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile0.v = (ushort)selectedMy8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile0.o = (ushort)selectedPrior8;

                game.tiles16[bgMode.selectedTiles[0] + i].tile1.id = (ushort)(selectedTile8 + 1 + (i * 2));
                game.tiles16[bgMode.selectedTiles[0] + i].tile1.palette = selectedPal8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile1.h = (ushort)selectedMx8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile1.v = (ushort)selectedMy8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile1.o = (ushort)selectedPrior8;

                game.tiles16[bgMode.selectedTiles[0] + i].tile2.id = (ushort)(selectedTile8 + 16 + (i * 2));
                game.tiles16[bgMode.selectedTiles[0] + i].tile2.palette = selectedPal8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile2.h = (ushort)selectedMx8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile2.v = (ushort)selectedMy8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile2.o = (ushort)selectedPrior8;

                game.tiles16[bgMode.selectedTiles[0] + i].tile3.id = (ushort)(selectedTile8 + 17 + (i * 2));
                game.tiles16[bgMode.selectedTiles[0] + i].tile3.palette = selectedPal8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile3.h = (ushort)selectedMx8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile3.v = (ushort)selectedMy8;
                game.tiles16[bgMode.selectedTiles[0] + i].tile3.o = (ushort)selectedPrior8;
            }


            UpdateMap();
            UpdateGFX();
            UpdatePalette();

            //GFX.DrawTiles16(game.tiles16.ToArray());
            bgMode.redrawTiles8();
            vramPicturebox.Refresh();
            palettePicturebox.Refresh();
            mainPicturebox.Refresh();
            editingtilePicturebox.Refresh();
            tiles16Picturebox.Refresh();
            scratchpadPicturebox.Refresh();

        }

        private void x4ViewToolStripMenuItem_Click(object sender, EventArgs e)
        {




        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {





        }

        private void zoomIncreaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Zoom += 1;
            if (Zoom >= 4)
            {
                Zoom = 4;
            }

            mainPanel.Width = (256 * Zoom) + 24;
            mainPicturebox.Width = 256 * Zoom;
            mainPicturebox.Height = 224 * Zoom;

            itemPanel.Location = new Point(6, (224 * Zoom) + 16);
            objectPanel.Location = new Point(6, (224 * Zoom) + 16);
            transitionPanel.Location = new Point(6, (224 * Zoom) + 16);
            doorPanel.Location = new Point(6, (224 * Zoom) + 16);
            spritePanel.Location = new Point(6, (224 * Zoom) + 16);
            hookPanel.Location = new Point(6, (224 * Zoom) + 16);
            plankPanel.Location = new Point(6, (224 * Zoom) + 16);
            blockPanel.Location = new Point(6, (224 * Zoom) + 16);
            enemyDoorPanel.Location = new Point(6, (224 * Zoom) + 16);
            palPanel.Location = new Point(6, (224 * Zoom) + 16);

            mainPicturebox.Invalidate();
        }

        private void zoomDecreaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Zoom -= 1;
            if (Zoom == 1)
            {
                Zoom = 2;
            }

            mainPanel.Width = (256 * Zoom) + 24;
            mainPicturebox.Width = 256 * Zoom;
            mainPicturebox.Height = 224 * Zoom;

            itemPanel.Location = new Point(6, (224 * Zoom) + 16);
            objectPanel.Location = new Point(6, (224 * Zoom) + 16);
            transitionPanel.Location = new Point(6, (224 * Zoom) + 16);
            doorPanel.Location = new Point(6, (224 * Zoom) + 16);
            spritePanel.Location = new Point(6, (224 * Zoom) + 16);
            hookPanel.Location = new Point(6, (224 * Zoom) + 16);
            plankPanel.Location = new Point(6, (224 * Zoom) + 16);
            blockPanel.Location = new Point(6, (224 * Zoom) + 16);
            enemyDoorPanel.Location = new Point(6, (224 * Zoom) + 16);
            palPanel.Location = new Point(6, (224 * Zoom) + 16);

            mainPicturebox.Invalidate();
        }

        private void discordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/GgeE9q7wyJ");
        }
    }

}
