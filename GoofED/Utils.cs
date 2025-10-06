using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public static class Utils
    {
        public static int SnesToPc(int addr)
        {
            return (addr & 0x7FFF) | ((addr & 0x7F0000) >> 1);
        }

        public static int PcToSnes(int addr)
        {
            byte[] b = BitConverter.GetBytes(addr);
            b[2] = (byte)(b[2] * 2);
            if (b[1] >= 0x80)
                b[2] += 1;
            else b[1] += 0x80;

            b[2] += 0x80; //return it back into a FastROM address
            return BitConverter.ToInt32(b, 0);
        }

        public static int AddressFromBytes(byte addr1, byte addr2, byte addr3)
        {
            return (addr1 << 16) | (addr2 << 8) | addr3;
        }

        public static short AddressFromBytes(byte addr1, byte addr2)
        {
            return (short)((addr1 << 8) | (addr2));
        }

        public static byte[] SnesTilesToPc8bppTiles(byte[] snesTiles, int nbrOfTiles, byte bpp = 3)
        {
            if (nbrOfTiles == -1)
            {
                nbrOfTiles = (snesTiles.Length / 0x20);
            }
            byte[] tiles8x8 = new byte[0x40 * nbrOfTiles];
            switch (bpp)
            {
                case 4:
                    for (int i = 0; i < nbrOfTiles; i++)
                    {
                        for (int line = 0; line < 8; line++)
                        {
                            byte[] pixelvalues = new byte[4]
                            {
                                snesTiles[(line * 2) + (i * 0x20)],
                                snesTiles[(line * 2) + 1 + (i * 0x20)],
                                snesTiles[(line * 2) + 16 + (i * 0x20)],
                                snesTiles[(line * 2) + 17 + (i * 0x20)]
                            };
                            for (int pixel = 7; pixel >= 0; pixel--)
                            {

                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)(pixelvalues[0] & 0x01);
                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)((pixelvalues[1] & 0x01) << 1);
                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)((pixelvalues[2] & 0x01) << 2);
                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)((pixelvalues[3] & 0x01) << 3);
                                pixelvalues[0] >>= 1;
                                pixelvalues[1] >>= 1;
                                pixelvalues[2] >>= 1;
                                pixelvalues[3] >>= 1;
                            }
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < nbrOfTiles; i++)
                    {
                        for (int line = 0; line < 8; line++)
                        {
                            byte[] pixelvalues = new byte[3]
                            {
                                snesTiles[(line * 2) + (i * 0x18)],
                                snesTiles[(line * 2) + 1 + (i * 0x18)],
                                snesTiles[(line) + 16 + (i * 0x18)]
                            };
                            for (int pixel = 7; pixel >= 0; pixel--)
                            {

                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)(pixelvalues[0] & 0x01);
                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)((pixelvalues[1] & 0x01) << 1);
                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)((pixelvalues[2] & 0x01) << 2);
                                pixelvalues[0] >>= 1;
                                pixelvalues[1] >>= 1;
                                pixelvalues[2] >>= 1;
                            }
                        }
                    }
                    break;

                case 2:
                    for (int i = 0; i < nbrOfTiles; i++)
                    {
                        for (int line = 0; line < 8; line++)
                        {
                            byte[] pixelvalues = new byte[2]
                            {
                                snesTiles[(line * 2) + (i * 0x10)],
                                snesTiles[(line * 2) + 1 + (i * 0x10)]
                            };
                            for (int pixel = 7; pixel >= 0; pixel--)
                            {

                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)(pixelvalues[0] & 0x01);
                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)((pixelvalues[1] & 0x01) << 1);
                                pixelvalues[0] >>= 1;
                                pixelvalues[1] >>= 1;
                            }
                        }
                    }
                    break;

                case 1:
                    for (int i = 0; i < nbrOfTiles; i++)
                    {
                        for (int line = 0; line < 8; line++)
                        {
                            byte[] pixelvalues = new byte[1]
                            {
                                snesTiles[(line) + (i * 0x08)],
                            };
                            for (int pixel = 7; pixel >= 0; pixel--)
                            {
                                tiles8x8[pixel + (line * 8) + (i * 0x40)] |= (byte)(pixelvalues[0] & 0x01);
                                pixelvalues[0] >>= 1;
                            }
                        }
                    }
                    break;
            }
            return tiles8x8;
        }


        public static byte[] PCSheetToSnesTiles(PointeredImage image, int bpp = 3, int nbrTiles = 16)
        {
            //int nbrTiles = (image.indexedSize / 0x40);
            byte[] snestiles8x8 = new byte[(bpp * 8) * nbrTiles];

            switch (bpp)
            {
                case 8:
                    for (int i = 0; i < nbrTiles; i++)
                    {
                        for (int lines = 0; lines < 8; lines++)
                        {
                            for (int px = 0; px < 8; px++)
                            {
                                int pcX = px + ((i % 16) * 8);
                                int pcY = (lines * 128) + ((i / 16) * 1024);
                                byte currentPixel = image[pcX + pcY]; // first pixel is pixel 7,0 of first tile
                                snestiles8x8[i] = currentPixel;
                            }
                        }
                    }
                    break;
                case 4:

                    for (int i = 0; i < nbrTiles; i++)
                    {
                        for (int lines = 0; lines < 8; lines++)
                        {
                            int shift = 0;
                            for (int px = 7; px >= 0; px--)
                            {
                                int pcX = px + ((i % 16) * 8);
                                int pcY = (lines * 128) + ((i / 16) * 1024);
                                byte currentPixel = image[pcX + pcY]; // first pixel is pixel 7,0 of first tile
                                byte bpp1 = (byte)((currentPixel & 0x01) << shift);
                                byte bpp2 = (byte)(((currentPixel >> 1) & 0x01) << shift);
                                byte bpp3 = (byte)(((currentPixel >> 2) & 0x01) << shift);
                                byte bpp4 = (byte)(((currentPixel >> 3) & 0x01) << shift);
                                shift++;


                                snestiles8x8[(i * 0x20) + (lines * 2)] |= bpp1;
                                snestiles8x8[(i * 0x20) + (lines * 2) + 1] |= bpp2;
                                snestiles8x8[(i * 0x20) + (lines * 2) + 16] |= bpp3;
                                snestiles8x8[(i * 0x20) + (lines * 2) + 17] |= bpp4;
                            }
                        }
                    }
                    break;

                case 3:
                    // [r0, bp1], [r0, bp2], [r1, bp1], [r1, bp2], [r2, bp1], [r2, bp2], [r3, bp1], [r3, bp2]
                    // [r4, bp1], [r4, bp2], [r5, bp1], [r5, bp2], [r6, bp1], [r6, bp2], [r7, bp1], [r7, bp2]
                    // [r0, bp3], [r1, bp3], [r2, bp3], [r3, bp3], [r4, bp3], [r5, bp3], [r6, bp3], [r7, bp3]

                    for (int i = 0; i < nbrTiles; i++)
                    {
                        for (int lines = 0; lines < 8; lines++)
                        {
                            int shift = 0;
                            for (int px = 7; px >= 0; px--)
                            {
                                int pcX = px + ((i % 16) * 8);
                                int pcY = (lines * 128) + ((i / 16) * 1024);
                                byte currentPixel = image[pcX + pcY]; // first pixel is pixel 7,0 of first tile
                                byte bpp1 = (byte)((currentPixel & 0x01) << shift);
                                byte bpp2 = (byte)(((currentPixel >> 1) & 0x01) << shift);
                                byte bpp3 = (byte)(((currentPixel >> 2) & 0x01) << shift);
                                shift++;


                                snestiles8x8[(i * 0x18) + (lines * 2)] |= bpp1;
                                snestiles8x8[(i * 0x18) + (lines * 2) + 1] |= bpp2;
                                snestiles8x8[(i * 0x18) + (lines) + 16] |= bpp3;
                            }
                        }
                    }
                    break;

                case 2:
                    // [r0, bp1], [r0, bp2], [r1, bp1], [r1, bp2], [r2, bp1], [r2, bp2], [r3, bp1], [r3, bp2]
                    // [r4, bp1], [r4, bp2], [r5, bp1], [r5, bp2], [r6, bp1], [r6, bp2], [r7, bp1], [r7, bp2]
                    for (int i = 0; i < nbrTiles; i++)
                    {
                        for (int lines = 0; lines < 8; lines++)
                        {
                            int shift = 0;
                            for (int px = 7; px >= 0; px--)
                            {
                                int pcX = px + ((i % 16) * 8);
                                int pcY = (lines * 128) + ((i / 16) * 1024);
                                byte currentPixel = image[pcX + pcY]; // first pixel is pixel 7,0 of first tile
                                byte bpp1 = (byte)((currentPixel & 0x01) << shift);
                                byte bpp2 = (byte)(((currentPixel >> 1) & 0x01) << shift);
                                shift++;

                                snestiles8x8[(i * 0x10) + (lines * 2)] |= bpp1;
                                snestiles8x8[(i * 0x10) + (lines * 2) + 1] |= bpp2;
                            }
                        }
                    }
                    break;



            }

            return snestiles8x8;
        }
    }
}
