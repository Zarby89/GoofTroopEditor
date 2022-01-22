using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public static class GFX
    {
        public static IntPtr vramBuffer = Marshal.AllocHGlobal(0x20000);
        public static Bitmap vramBitmap;

        public static IntPtr vramMirrorBuffer = Marshal.AllocHGlobal(0x20000);
        public static Bitmap vramMirrorBitmap;

        public static IntPtr editingTile16Buffer = Marshal.AllocHGlobal(0x100);
        public static Bitmap editingTile16Bitmap;

        public static IntPtr BG1Buffer = Marshal.AllocHGlobal(0x10000);
        public static Bitmap BG1Bitmap;
        public static ushort[] tilesBG1 = new ushort[0x400];
        public static IntPtr BG2Buffer = Marshal.AllocHGlobal(0x10000);
        public static Bitmap BG2Bitmap;
        public static ushort[] tilesBG2 = new ushort[0x400];

        public static IntPtr BG2BufferMask = Marshal.AllocHGlobal(0x10000);
        public static Bitmap BG2BitmapMask;

        public static IntPtr SprBuffer = Marshal.AllocHGlobal(0x10000);
        public static Bitmap SprBitmap;

        public static IntPtr tile16Buffer = Marshal.AllocHGlobal(0x100000);
        public static Bitmap tile16Bitmap;

        public static IntPtr scratch16Buffer = Marshal.AllocHGlobal(0x100000);
        public static Bitmap scratch16Bitmap;

        public static IntPtr textBuffer = Marshal.AllocHGlobal(0x4000);
        public static Bitmap textBitmap;

        public static IntPtr creditBuffer = Marshal.AllocHGlobal(0x10000);
        public static Bitmap creditBitmap;


        public static IntPtr passwordBuffer = Marshal.AllocHGlobal(0x1000);
        public static Bitmap passwordBitmap;

        public static Color[] palette = new Color[256];

        public static ushort[] scratchpadTiles = new ushort[0x0C48];

        //SNES FORMAT TO PC 8BPP
        //Snes format is a tile format each 32 bytes = 1 tile of 8x8
        //[r0, bp1], [r0, bp2], [r1, bp1], [r1, bp2], [r2, bp1], [r2, bp2], [r3, bp1], [r3, bp2]
        //[r4, bp1], [r4, bp2], [r5, bp1], [r5, bp2], [r6, bp1], [r6, bp2], [r7, bp1], [r7, bp2]
        //[r0, bp3], [r0, bp4], [r1, bp3], [r1, bp4], [r2, bp3], [r2, bp4], [r3, bp3], [r3, bp4]
        //[r4, bp3], [r4, bp4], [r5, bp3], [r5, bp4], [r6, bp3], [r6, bp4], [r7, bp3], [r7, bp4]

        //PC 8bpp is 128 pixel wide (16 tiles of 8x8)
        public static byte[] snesbpp4Tobpp8(byte[] bpp4Data, int displayTiles = 8)
        {
            //Output format
            //1 byte per pixel
            int nbrOfTiles = bpp4Data.Length / 32;
            byte[] lines = new byte[(bpp4Data.Length * 2)];
            int pos = 0;
            int spos = 0;
            byte[] mask = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            for (int l = 0; l < Math.Ceiling((double)(nbrOfTiles/ displayTiles)); l++) //number of line tiles
            {
                for (int b = 0; b < displayTiles; b++) //number of tile per line
                {
                    for (int j = 0; j < 8; j++) //number of lines
                    {
                        pos = (j * (displayTiles*8)) + (8 * b) + (((displayTiles * 8)*8) * l);
                        for (int i = 0; i < 8; i++) //X Pixel in ROW
                        {
                            //bpp1
                            if ((bpp4Data[spos] & mask[i]) == mask[i])
                            {
                                lines[pos + i] |= 1;
                            }
                            //bpp2
                            if ((bpp4Data[spos + 1] & mask[i]) == mask[i])
                            {
                                lines[pos + i] |= 2;
                            }
                            //bpp3
                            if ((bpp4Data[spos + 16] & mask[i]) == mask[i])
                            {
                                lines[pos + i] |= 4;
                            }
                            //bpp4
                            if ((bpp4Data[spos + 17] & mask[i]) == mask[i])
                            {
                                lines[pos + i] |= 8;
                            }
                        }
                        spos += 2; //next row
                    }
                    spos += 16;
                }
            }
            return lines;
        }

        public static byte[] snesbpp2Tobpp8(byte[] bpp4Data, int displayTiles = 8)
        {
            //Output format
            //1 byte per pixel
            int nbrOfTiles = bpp4Data.Length / 16;
            byte[] lines = new byte[(bpp4Data.Length * 4)];
            int pos = 0;
            int spos = 0;
            byte[] mask = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            for (int l = 0; l < (nbrOfTiles / displayTiles); l++) //number of line tiles
            {
                for (int b = 0; b < displayTiles; b++) //number of tile per line
                {
                    for (int j = 0; j < 8; j++) //number of lines
                    {
                        pos = (j * (displayTiles * 8)) + (8 * b) + (((displayTiles * 8) * 8) * l);
                        for (int i = 0; i < 8; i++) //X Pixel in ROW
                        {
                            //bpp1
                            if ((bpp4Data[spos] & mask[i]) == mask[i])
                            {
                                lines[pos + i] |= 1;
                            }
                            //bpp2
                            if ((bpp4Data[spos + 1] & mask[i]) == mask[i])
                            {
                                lines[pos + i] |= 2;
                            }
                        }
                        spos += 2; //next row
                    }
                    //spos += 8;
                }
            }
            return lines;
        }

        public static byte[] snesbpp4Tobpp8Tiles(byte[] bpp4Data, int nbrTiles, int startTile = 0)
        {
            byte[] mask = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            byte[] bpp8Data = new byte[nbrTiles*64];

            int tPos = 0;
            int pos = 0;
            int spos = 0;
            spos += (startTile * 32);

            for (int t = startTile; t < nbrTiles; t++)
            {
                if (spos >= bpp4Data.Length)
                {
                    return bpp8Data;
                }
                for (int y = 0; y < 8; y++) //Y Pixels
                {
                    for (int i = 0; i < 8; i++) //X Pixels
                    {
                        //bpp1
                        if ((bpp4Data[spos] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i] |= 1;
                        }
                        //bpp2
                        if ((bpp4Data[spos + 1] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i ] |= 2;
                        }
                        //bpp3
                        if ((bpp4Data[spos + 16] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i] |= 4;
                        }
                        //bpp4
                        if ((bpp4Data[spos + 17] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i] |= 8;
                        }

                    }

                    spos += 2; //next row
                    pos += 8;
                }
                spos += 16;
            }

            return bpp8Data;

        }

        public static byte[] snesbpp4Tobpp8row(byte[] bpp4Data, int tileperrow = 16, int size = -1, int startTile = 0)
        {
            byte[] mask = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            byte[] bpp8Data = new byte[(bpp4Data.Length * 4)];


            
            int nbrOfTiles = bpp4Data.Length / 32;

            if (size != -1)
            {
                nbrOfTiles = size / 32;
            }
            int tPos = 0;
            int pos = 0;
            int spos = 0;
            spos += (startTile * 32);

            for (int t = startTile; t < nbrOfTiles; t++)
            {
                for (int y = 0; y < 8; y++) //Y Pixels
                {
                    for (int i = 0; i < 8; i++) //X Pixels
                    {
                        if (spos>= bpp4Data.Length)
                        {
                            return bpp8Data;
                        }
                        //bpp1
                        if ((bpp4Data[spos] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i + (y*128)] |= 1;
                        }
                        //bpp2
                        if ((bpp4Data[spos + 1] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i + (y * 128)] |= 2;
                        }
                        //bpp3
                        if ((bpp4Data[spos + 16] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i + (y * 128)] |= 4;
                        }
                        //bpp4
                        if ((bpp4Data[spos + 17] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i + (y * 128)] |= 8;
                        }
                        
                    }

                    spos += 2; //next row
                }
                pos += 8;
                tPos += 1;
                if (tPos >= tileperrow)
                {
                    pos += 896 + ((16-tileperrow)*8);
                    tPos = 0;
                }
                
                spos += 16;
            }

            return bpp8Data;

        }

        public static byte[] snesbpp4Tobpp8TileAt(byte[] bpp4Data, int startTile = 0)
        {
            byte[] mask = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            byte[] bpp8Data = new byte[64];

            int pos = 0;
            int spos = (startTile*32);
                for (int y = 0; y < 8; y++) //Y Pixels
                {
                    for (int i = 0; i < 8; i++) //X Pixels
                    {
                        //bpp1
                        if ((bpp4Data[spos] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i + (y * 8)] |= 1;
                        }
                        //bpp2
                        if ((bpp4Data[spos + 1] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i + (y * 8)] |= 2;
                        }
                        //bpp3
                        if ((bpp4Data[spos + 16] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i + (y * 8)] |= 4;
                        }
                        //bpp4
                        if ((bpp4Data[spos + 17] & mask[i]) == mask[i])
                        {
                            bpp8Data[pos + i + (y * 8)] |= 8;
                        }

                    }

                    spos += 2; //next row
                }

            return bpp8Data;
        }
            


        public unsafe static void ClearBGs()
        {
            byte* ptr = (byte*)BG1Buffer.ToPointer();
            byte* ptr2 = (byte*)BG2Buffer.ToPointer();
            byte* ptr4 = (byte*)BG2BufferMask.ToPointer();
            byte* ptr3 = (byte*)SprBuffer.ToPointer();
            for (int i = 0;i<0x10000;i++)
            {
                ptr[i] = 0;
                ptr2[i] = 0;
                ptr3[i] = 0;
                ptr4[i] = 0;
            }
        }

        public unsafe static void ClearBGSpr()
        {

            byte* ptr = (byte*)SprBuffer.ToPointer();
            for (int i = 0; i < 0x10000; i++)
            {
                ptr[i] = 0;
            }
        }

        public unsafe static void ClearBGText()
        {

            byte* ptr = (byte*)textBuffer.ToPointer();
            for (int i = 0; i < 0x4000; i++)
            {
                ptr[i] = 0;
            }
        }

        public unsafe static void ClearBGCredits()
        {

            byte* ptr = (byte*)creditBuffer.ToPointer();
            for (int i = 0; i < 0x10000; i++)
            {
                ptr[i] = 0;
            }
        }

        public unsafe static void ClearBGPassword()
        {

            byte* ptr = (byte*)passwordBuffer.ToPointer();
            for (int i = 0; i < 0x1000; i++)
            {
                ptr[i] = 0;
            }
        }

        public unsafe static void ClearBGMask()
        {

            byte* ptr = (byte*)BG2BufferMask.ToPointer();
            for (int i = 0; i < 0x10000; i++)
            {
                ptr[i] = 0;
            }
        }

        public unsafe static void DrawTiles8Mirror(int mirrorx, int mirrory, byte pal = 0)
        {
            var alltilesData = (byte*)vramBuffer.ToPointer();
            byte* ptr = (byte*)vramMirrorBuffer.ToPointer();
            int id = 0;
            for (int yy = 0; yy < 128; yy++) //for each tile on the tile buffer
            {
                for (int xx = 0; xx < 16; xx++)
                {
                    for (var yl = 0; yl < 8; yl++)
                    {
                        for (var xl = 0; xl < 8; xl++)
                        {
                            int mx = xl * (1 - mirrorx) + (7 - xl) * (mirrorx);
                            int my = yl * (1 - mirrory) + (7 - yl) * (mirrory);

                            int ty = (id / 16) * 1024;
                            int tx = (id % 16) * 8;
                            var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                            int index = (xx * 8) + (yy * 1024) + ((mx) + (my * 128));
                            ptr[index] = (byte)(pixel);// + pal * 16);
                        }
                    }
                    id++;
                }
            }
        }

        public unsafe static void DrawTiles16(Tile16[] alltiles)
        {
            var alltilesData = (byte*)vramBuffer.ToPointer();
            byte* ptr = (byte*)tile16Buffer.ToPointer();
            int xx = 0;
            int yy = 0;
            for (int ii = 0; ii < alltiles.Length; ii++) //for each tile on the tile buffer
            {
                Tile16 t16 = alltiles[ii];
                //Top Left
                TileInfo t = alltiles[ii].tile0;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = (xx * 16) + (yy * 2048) + ((mx) + (my * 128));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Top Right
                t = alltiles[ii].tile1;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = 8 + (xx * 16) + (yy * 2048) + ((mx) + (my * 128));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Bottom Left
                t = alltiles[ii].tile2;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index =  (xx * 16) + 1024 + (yy * 2048) + ((mx) + (my * 128));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Bottom Right
                t = alltiles[ii].tile3;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = 8 + (xx * 16) + (yy * 2048) + 1024 + ((mx) + (my * 128));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }
                xx += 1;
                if (xx >= 8)
                {
                    xx = 0;
                    yy +=1;
                }
            }
        }


        public unsafe static void DrawEditing16(Tile16[] alltiles, int selectedTile)
        {
            var alltilesData = (byte*)vramBuffer.ToPointer();
            byte* ptr = (byte*)editingTile16Buffer.ToPointer();
            int xx = 0;
            int yy = 0;
            TileInfo t = alltiles[selectedTile].tile0;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = ((mx) + (my * 16));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Top Right
                t = alltiles[selectedTile].tile1;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = 8 + ((mx) + (my * 16));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Bottom Left
                t = alltiles[selectedTile].tile2;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = 128 + ((mx) + (my * 16));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Bottom Right
                t = alltiles[selectedTile].tile3;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = 136 + ((mx) + (my * 16));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }
        }


        public unsafe static void DrawScratchPad(Tile16[] alltiles)
        {
            var alltilesData = (byte*)vramBuffer.ToPointer();
            byte* ptr = (byte*)scratch16Buffer.ToPointer();
            int xx = 0;
            int yy = 0;
            for (int ii = 0; ii < scratchpadTiles.Length; ii++) //for each tile on the tile buffer
            {
                Tile16 t16 = alltiles[scratchpadTiles[ii]];
                //Top Left
                TileInfo t = alltiles[scratchpadTiles[ii]].tile0;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = (xx * 16) + (yy * 2048) + ((mx) + (my * 128));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Top Right
                t = alltiles[scratchpadTiles[ii]].tile1;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = 8 + (xx * 16) + (yy * 2048) + ((mx) + (my * 128));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Bottom Left
                t = alltiles[scratchpadTiles[ii]].tile2;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = (xx * 16) + 1024 + (yy * 2048) + ((mx) + (my * 128));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }

                //Bottom Right
                t = alltiles[scratchpadTiles[ii]].tile3;
                for (var yl = 0; yl < 8; yl++)
                {
                    for (var xl = 0; xl < 8; xl++)
                    {
                        int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                        int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                        int ty = (t.id / 16) * 1024;
                        int tx = (t.id % 16) * 8;
                        var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                        int index = 8 + (xx * 16) + (yy * 2048) + 1024 + ((mx) + (my * 128));
                        ptr[index] = (byte)(pixel + t.palette * 16);
                    }
                }
                xx += 1;
                if (xx >= 8)
                {
                    xx = 0;
                    yy += 1;
                }
            }
        }

        public unsafe static void DrawBG1()
        {
            var alltilesData = (byte*)vramBuffer.ToPointer();
            byte* ptr = (byte*)BG1Buffer.ToPointer();

            for (int yy = 0; yy < 32; yy++) //for each tile on the tile buffer
            {
                for (int xx = 0; xx < 32; xx++)
                {
                    //if (tilesBG1[xx + (yy * 32)] != 0x0000)
                    //{
                        TileInfo t = gettilesinfo(tilesBG1[xx + (yy * 32)]);
                        for (var yl = 0; yl < 8; yl++)
                        {
                            for (var xl = 0; xl < 8; xl++)
                            {
                                int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                                int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                                int ty = (t.id / 16) * 1024;
                                int tx = (t.id % 16) * 8;
                                var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                                int index = (xx * 8) + (yy * 2048) + ((mx) + (my * 256));
                                ptr[index] = (byte)(pixel + t.palette * 16);
                            }
                        }
                   // }
                }
            }
        }

        public unsafe static void DrawBG2()
        {
            ClearBGMask();
            var alltilesData = (byte*)vramBuffer.ToPointer();
            byte* ptr = (byte*)BG2Buffer.ToPointer();
            byte* ptr2 = (byte*)BG2BufferMask.ToPointer();
            for (int yy = 0; yy < 32; yy++) //for each tile on the tile buffer
            {
                for (int xx = 0; xx < 32; xx++)
                {
                    //if (tilesBG2[xx + (yy * 32)] != 0x0000)
                   // {
                        TileInfo t = gettilesinfo(tilesBG2[xx + (yy * 32)]);
                        for (var yl = 0; yl < 8; yl++)
                        {
                            for (var xl = 0; xl < 8; xl++)
                            {
                                int mx = xl * (1 - t.h) + (7 - xl) * (t.h);
                                int my = yl * (1 - t.v) + (7 - yl) * (t.v);

                                int ty = (t.id / 16) * 1024;
                                int tx = (t.id % 16) * 8;
                                var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                                int index = (xx * 8) + (yy * 2048) + ((mx) + (my * 256));
                                ptr[index] = (byte)(pixel + t.palette * 16);
                                if (t.o != 0)
                                {
                                    ptr2[index] = (byte)(pixel + t.palette * 16);
                                }
                            }
                        }
                    //}
                }
            }

        }

        public unsafe static void DrawFromVRAM(IntPtr dest,ushort id, byte x, byte y,bool size = false, byte h = 0, byte v = 0, byte p = 0)
        {
            var alltilesData = (byte*)vramBuffer.ToPointer();
            byte* ptr = (byte*)dest.ToPointer();
            int s = 8;
            if (size)
            {
                s = 16;
            }


            for (var yl = 0; yl < s; yl++)
            {
                for (var xl = 0; xl < s; xl++)
                {
                    int mx = xl * (1 - h) + ((s-1) - xl) * (h);
                    int my = yl * (1 - v) + ((s-1) - yl) * (v);

                    int ty = (id / 16) * 1024;
                    int tx = (id % 16) * 8;
                    var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                    int index = (x) + (y * 256) + ((mx) + (my * 256));
                    if (pixel != 0)
                    {
                        ptr[index] = (byte)(pixel + p * 16);
                    }
                }
            }
        }


        public unsafe static void DrawFromVRAM2bpp(IntPtr dest, ushort id, byte x, byte y, byte p)
        {
            var alltilesData = (byte*)vramBuffer.ToPointer();
            byte* ptr = (byte*)dest.ToPointer();
            int s = 8;
            int h = 0;
            int v = 0;
            for (var yl = 0; yl < s; yl++)
            {
                for (var xl = 0; xl < s; xl++)
                {
                    int mx = xl * (1 - h) + ((s - 1) - xl) * (h);
                    int my = yl * (1 - v) + ((s - 1) - yl) * (v);

                    int ty = (id / 16) * 1024;
                    int tx = (id % 16) * 8;
                    var pixel = alltilesData[(tx + ty) + (yl * 128) + xl];

                    int index = (x) + (y * 256) + ((mx) + (my * 256));
                    if (pixel != 0)
                    {
                        ptr[index] = (byte)(pixel + p * 4);
                    }
                }
            }
        }


        public static TileInfo gettilesinfo(ushort tile)
        {
            //vhopppcc cccccccc
            ushort o = 0;
            ushort v = 0;
            ushort h = 0;
            ushort tid = (ushort)(tile & 0x3FF);
            byte p = (byte)((tile >> 10) & 0x07);

            o = (ushort)((tile & 0x2000) >> 13);
            h = (ushort)((tile & 0x4000) >> 14);
            v = (ushort)((tile & 0x8000) >> 15);
            return new TileInfo(tid, p, v, h, o);

        }


    }
}
