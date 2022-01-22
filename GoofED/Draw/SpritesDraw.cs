using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    class SpritesDraw
    {
        private byte searchPal(byte index, int ptrPC, Game game)
        {
            for (int p = 0; p < 4; p++)
            {
                if (game.rom.ReadByte(ptrPC + game.levels[game.selectedLevel].maps[game.selectedMap].spritepal + p) == index)
                {
                    return (byte)(p + 12);
                }
            }
            return 15;
        }

        public void Draw(Graphics g, Game game, Sprite selectedSpr, bool debug)
        {
            int ptrPC = Utils.SnesToPc(game.rom.ReadShort(Constants.PalSprData1_Ptr) + 0x830000);
            byte p = 0;
            foreach (Sprite spr in game.levels[game.selectedLevel].maps[game.selectedMap].sprites)
            {
                g.DrawRectangle(Pens.Blue, new Rectangle((spr.x * 2), (spr.y * 2), 32, 32));
                if (debug)
                {

                    g.DrawString(spr.id.ToString("X2") + "  " + spr.param.ToString("X2"), new Font("arial", 16, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, new Point(spr.x * 2, spr.y * 2));
                    g.DrawString(spr.unkn.ToString("X2"), new Font("arial", 16, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, new Point(spr.x * 2, (spr.y + 12) * 2));
                }
                if (spr == selectedSpr)
                {
                    g.DrawRectangle(new Pen(Brushes.Blue, 2), new Rectangle((spr.x * 2), (spr.y * 2), 32, 32));
                }
                switch (spr.id)
                {
                    case 00://edgehog
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1859, (byte)((spr.x + 0) - 11), (byte)((spr.y + 0) - 21), true, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1861, (byte)((spr.x + 0) - 11), (byte)((spr.y + 16) - 21), false, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1862, (byte)((spr.x + 8) - 11), (byte)((spr.y + 16) - 21), false, 1, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1877, (byte)((spr.x + 16) - 11), (byte)((spr.y + 16) - 21), false, 0, 0, 14);





                        break;
                    case 0x0C://pirates
                        if (spr.param == 0)//small blue pirate
                        {
                            p = searchPal(6, ptrPC,game);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((spr.x + 0) - 9), (byte)((spr.y + 0) - 30), true, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((spr.x + 0) - 9), (byte)((spr.y + 16) - 30), true, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((spr.x + 16) - 9), (byte)((spr.y + 13) - 30), false, 1, 0, p);
                        }
                        else if (spr.param == 2)//small green pirate
                        {
                            //Search for green palette (07)
                            p = searchPal(7, ptrPC, game);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((spr.x + 0) - 9), (byte)((spr.y + 0) - 30), true, 1, 0, (byte)(p));
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((spr.x + 0) - 9), (byte)((spr.y + 16) - 30), true, 1, 0, (byte)(p));
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((spr.x + 16) - 9), (byte)((spr.y + 13) - 30), false, 1, 0, (byte)(p));
                        }
                        else if (spr.param == 4)//small red pirate
                        {
                            p = searchPal(8, ptrPC, game);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((spr.x + 0) - 9), (byte)((spr.y + 0) - 30), true, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((spr.x + 0) - 9), (byte)((spr.y + 16) - 30), true, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((spr.x + 16) - 9), (byte)((spr.y + 13) - 30), false, 1, 0, p);
                        }
                        else if (spr.param == 8)//big blue pirate
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1798, (byte)((spr.x + 0) - 9), (byte)((spr.y + 0) - 26), true, 1, 0, 13);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1800, (byte)((spr.x + 0) - 9), (byte)((spr.y + 16) - 26), true, 1, 0, 13);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1803, (byte)((spr.x - 8) - 9), (byte)((spr.y + 8) - 26), false, 1, 0, 13);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1802, (byte)((spr.x + 16) - 9), (byte)((spr.y + 7) - 26), false, 1, 0, 13);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1818, (byte)((spr.x + 16) - 9), (byte)((spr.y + 15) - 26), false, 1, 0, 13);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1819, (byte)((spr.x - 8) - 9), (byte)((spr.y + 16) - 26), false, 1, 0, 13);
                        }
                        else if (spr.param == 0x0A)//big purple pirate
                        {
                            p = searchPal(0x0C, ptrPC, game);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1798, (byte)((spr.x + 0) - 9), (byte)((spr.y + 0) - 26), true, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1800, (byte)((spr.x + 0) - 9), (byte)((spr.y + 16) - 26), true, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1803, (byte)((spr.x - 8) - 9), (byte)((spr.y + 8) - 26), false, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1802, (byte)((spr.x + 16) - 9), (byte)((spr.y + 7) - 26), false, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1818, (byte)((spr.x + 16) - 9), (byte)((spr.y + 15) - 26), false, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1819, (byte)((spr.x - 8) - 9), (byte)((spr.y + 16) - 26), false, 1, 0, p);
                        }
                        else if (spr.param == 0x0C)//big green pirate
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1798, (byte)((spr.x + 0) - 9), (byte)((spr.y + 0) - 26), true, 1, 0, 14);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1800, (byte)((spr.x + 0) - 9), (byte)((spr.y + 16) - 26), true, 1, 0, 14);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1803, (byte)((spr.x - 8) - 9), (byte)((spr.y + 8) - 26), false, 1, 0, 14);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1802, (byte)((spr.x + 16) - 9), (byte)((spr.y + 7) - 26), false, 1, 0, 14);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1818, (byte)((spr.x + 16) - 9), (byte)((spr.y + 15) - 26), false, 1, 0, 14);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1819, (byte)((spr.x - 8) - 9), (byte)((spr.y + 16) - 26), false, 1, 0, 14);
                        }
                        else if (spr.param == 0x0E)//big red pirate
                        {
                            p = searchPal(0x1B, ptrPC, game);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1798, (byte)((spr.x + 0) - 9), (byte)((spr.y + 0) - 26), true, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1800, (byte)((spr.x + 0) - 9), (byte)((spr.y + 16) - 26), true, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1803, (byte)((spr.x - 8) - 9), (byte)((spr.y + 8) - 26), false, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1802, (byte)((spr.x + 16) - 9), (byte)((spr.y + 7) - 26), false, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1818, (byte)((spr.x + 16) - 9), (byte)((spr.y + 15) - 26), false, 1, 0, p);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1819, (byte)((spr.x - 8) - 9), (byte)((spr.y + 16) - 26), false, 1, 0, p);
                        }


                        break;
                    case 0x0E: //islander
                        if (game.selectedLevel != 2)
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1984), (byte)((spr.x - 16)), (byte)((spr.y - 28)), true, 0, 0, 8);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1986), (byte)((spr.x)), (byte)((spr.y - 28)), true, 0, 0, 8);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1988), (byte)((spr.x - 8)), (byte)((spr.y - 12)), true, 0, 0, 8);
                        }
                        break;
                    case 0x02: //bee
                        GFX.DrawFromVRAM(GFX.SprBuffer, 1923, (byte)(spr.x - 2), (byte)(spr.y - 29), false, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, 1939, (byte)(spr.x - 10), (byte)(spr.y - 32), false, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, 1921, (byte)(spr.x - 10), (byte)(spr.y - 27), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, 1920, (byte)(spr.x + 6), (byte)(spr.y - 27), false, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, 1936, (byte)(spr.x + 6), (byte)(spr.y - 19), false, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, 1614, (byte)(spr.x - 8), (byte)(spr.y - 8), true, 0, 0, 11);


                        break;
                    case 0x06: //snaku
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1858), (byte)((spr.x - 12)), (byte)((spr.y - 12)), true, 1, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1868), (byte)((spr.x + 4)), (byte)((spr.y - 4)), false, 1, 0, 13); //tail
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1889), (byte)((spr.x - 7)), (byte)((spr.y - 19)), false, 1, 0, 13); //head
                        break;
                    case 0x08: //ghost
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1992, (byte)((spr.x) - 4), (byte)((spr.y) - 24), true, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1984, (byte)((spr.x + 8) - 4), (byte)((spr.y - 8) - 24), false, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)2000, (byte)((spr.x) - 4), (byte)((spr.y - 8) - 24), false, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1987, (byte)((spr.x - 8) - 4), (byte)((spr.y - 8) - 24), false, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)2003, (byte)((spr.x - 8) - 4), (byte)((spr.y) - 24), false, 0, 0, 14);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1614, (byte)((spr.x - 3) - 4), (byte)((spr.y + 12) - 24), true, 0, 0, 11);

                        break;
                    case 0x0A: //bat
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1984, (byte)((spr.x - 16)), (byte)((spr.y) - 32), true, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1984, (byte)((spr.x)), (byte)((spr.y) - 32), true, 1, 0, 14);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1614, (byte)((spr.x - 8)), (byte)((spr.y - 8)), true, 0, 0, 11);

                        break;
                    case 0x10: //armor
                        p = searchPal(0x10, ptrPC, game);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1856), (byte)((spr.x) - 9), (byte)((spr.y - 32)), true, 1, 0, p);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1858), (byte)((spr.x) - 9), (byte)((spr.y) - 16), true, 1, 0, p);
                        break;
                    case 0x1A: //frogu
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1924), (byte)((spr.x - 8)), (byte)((spr.y - 8)), true, 0, 0, 15);
                        break;
                    case 0x16: //rollinpin
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1856, (byte)((spr.x)), (byte)((spr.y) - 16), true, 1, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1860, (byte)((spr.x)), (byte)((spr.y)), true, 1, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1858, (byte)((spr.x) - 16), (byte)((spr.y - 16)), true, 1, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1862, (byte)((spr.x) - 16), (byte)((spr.y)), true, 1, 0, 11);
                        break;
                    case 0x18: //Spike
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)((spr.x)), (byte)((spr.y - 16)), true, 1, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1898, (byte)((spr.x)), (byte)((spr.y)), true, 1, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1868, (byte)((spr.x - 16)), (byte)((spr.y - 16)), true, 1, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1900, (byte)((spr.x - 16)), (byte)((spr.y)), true, 1, 0, 14);
                        break;
                    case 0x1E:
                        GFX.DrawFromVRAM(GFX.SprBuffer, 2032, (byte)(spr.x + 0), (byte)(spr.y + 0), false, 0, 0, 9);
                        GFX.DrawFromVRAM(GFX.SprBuffer, 2023, (byte)(spr.x - 4), (byte)(spr.y - 16), true, 0, 0, 9);
                        GFX.DrawFromVRAM(GFX.SprBuffer, 2023, (byte)(spr.x - 4), (byte)(spr.y - 32), true, 0, 0, 9);

                        break;

                    case 0x20: //canon ball
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1888, (byte)((spr.x) - 4), (byte)((spr.y) - 4), false, 0, 0, 13);

                        break;

                    case 0x22: //canon
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1920, (byte)((spr.x) - 16), (byte)((spr.y) - 16), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1920, (byte)((spr.x)), (byte)((spr.y) - 16), true, 1, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1952, (byte)((spr.x) - 16), (byte)((spr.y)), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1952, (byte)((spr.x)), (byte)((spr.y)), true, 1, 0, 13);

                        break;
                    case 0x26: //skeleton bosses

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1856, (byte)((spr.x) - 16), (byte)((spr.y) - 36), true, 0, 0, (byte)(13 - (spr.param / 2)));
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1858, (byte)((spr.x) - 16), (byte)((spr.y + 8) - 36), true, 0, 0, (byte)(13 - (spr.param / 2)));
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1860, (byte)((spr.x) - 16), (byte)((spr.y + 24) - 36), true, 0, 0, (byte)(13 - (spr.param / 2)));
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1856, (byte)((spr.x + 16) - 16), (byte)((spr.y) - 36), true, 1, 0, (byte)(13 - (spr.param / 2)));
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1858, (byte)((spr.x + 16) - 16), (byte)((spr.y + 8) - 36), true, 1, 0, (byte)(13 - (spr.param / 2)));
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1860, (byte)((spr.x + 16) - 16), (byte)((spr.y + 24) - 36), true, 1, 0, (byte)(13 - (spr.param / 2)));

                        break;
                    case 0x28: //pirate bossu
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((32) - 9), (byte)((64 + 0) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((32) - 9), (byte)((64 + 16) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((32 + 16) - 9), (byte)((64 + 13) - 30), false, 1, 0, 15);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((96 + 0) - 9), (byte)((64 + 0) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((96 + 0) - 9), (byte)((64 + 16) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((96 + 16) - 9), (byte)((64 + 13) - 30), false, 1, 0, 15);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((160 + 0) - 9), (byte)((64 + 0) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((160 + 0) - 9), (byte)((64 + 16) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((160 + 16) - 9), (byte)((64 + 13) - 30), false, 1, 0, 15);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((224 + 0) - 9), (byte)((64 + 0) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((224 + 0) - 9), (byte)((64 + 16) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((224 + 16) - 9), (byte)((64 + 13) - 30), false, 1, 0, 15);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((64 + 0) - 9), (byte)((96 + 0) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((64 + 0) - 9), (byte)((96 + 16) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((64 + 16) - 9), (byte)((96 + 13) - 30), false, 1, 0, 15);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((128 + 0) - 9), (byte)((96 + 0) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((128 + 0) - 9), (byte)((96 + 16) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((128 + 16) - 9), (byte)((96 + 13) - 30), false, 1, 0, 15);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1792, (byte)((192 + 0) - 9), (byte)((96 + 0) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1794, (byte)((192 + 0) - 9), (byte)((96 + 16) - 30), true, 1, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1796, (byte)((192 + 16) - 9), (byte)((96 + 13) - 30), false, 1, 0, 15);
                        g.DrawString("Level1 BOSS", new Font("arial", 14, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, new Point((spr.x * 2) - 40, (spr.y) * 2));
                        break;
                    case 0x2A: //switch sprite
                        if (spr.param != 0)
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(429), (byte)(((spr.x - 8) / 8) * 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(429), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)(((spr.y - 8) / 8) * 8), false, 1, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(445), (byte)(((spr.x - 8) / 8) * 8), (byte)(((((spr.y - 8) / 8)) * 8) + 8), false, 0, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(445), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)((((spr.y - 8) / 8) * 8) + 8), false, 1, 0, 4);
                        }
                        else
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(992), (byte)(((spr.x - 8) / 8) * 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(993), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(994), (byte)(((spr.x - 8) / 8) * 8), (byte)(((((spr.y - 8) / 8)) * 8) + 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(995), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)((((spr.y - 8) / 8) * 8) + 8), false, 0, 0, 6);


                        }

                        break;
                    case 0x2E: //Level 2 Bossu
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1992, (byte)((spr.x + 0) + 16), (byte)((spr.y + 0) - 32), true, 0, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1864, (byte)((spr.x - 5) + 16), (byte)((spr.y + 13) - 32), true, 0, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1896, (byte)((spr.x - 5) + 16), (byte)((spr.y + 29) - 32), true, 0, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1864, (byte)((spr.x + 11) + 16), (byte)((spr.y + 13) - 32), true, 1, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1896, (byte)((spr.x + 11) + 16), (byte)((spr.y + 29) - 32), true, 1, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1920, (byte)((spr.x - 5) + 16), (byte)((spr.y + 38) - 32), true, 0, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1920, (byte)((spr.x + 11) + 16), (byte)((spr.y + 38) - 32), true, 1, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1922, (byte)((spr.x - 9) + 16), (byte)((spr.y + 1) - 32), true, 0, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1924, (byte)((spr.x - 13) + 16), (byte)((spr.y + 17) - 32), true, 0, 0, 12);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1926, (byte)((spr.x + 19) + 16), (byte)((spr.y + 26) - 32), true, 0, 0, 12);





                        break;
                    case 0x32: //bridge switch sprite
                        if (spr.param == 2)
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(429), (byte)(((spr.x - 8) / 8) * 8), (byte)(((spr.y - 4) / 8) * 8), false, 0, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(429), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)(((spr.y - 4) / 8) * 8), false, 1, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(445), (byte)(((spr.x - 8) / 8) * 8), (byte)(((((spr.y - 4) / 8)) * 8) + 8), false, 0, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(445), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)((((spr.y - 4) / 8) * 8) + 8), false, 1, 0, 4);
                        }
                        else if (spr.param == 4)
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(743), (byte)(((spr.x - 8) / 8) * 8), (byte)(((spr.y - 8) / 8) * 8), false, 1, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(742), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)(((spr.y - 8) / 8) * 8), false, 1, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(745), (byte)(((spr.x - 8) / 8) * 8), (byte)(((((spr.y - 8) / 8)) * 8) + 8), false, 1, 0, 4);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(744), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)((((spr.y - 8) / 8) * 8) + 8), false, 1, 0, 4);
                        }
                        else
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(992), (byte)(((spr.x) / 8) * 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(993), (byte)((((spr.x) / 8) * 8) + 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(994), (byte)(((spr.x) / 8) * 8), (byte)(((((spr.y - 8) / 8)) * 8) + 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(995), (byte)((((spr.x) / 8) * 8) + 8), (byte)((((spr.y - 8) / 8) * 8) + 8), false, 0, 0, 6);
                        }
                        break;
                    case 0x36: //Level 2 barrel thrower
                        if (spr.unkn == 1)
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1824, (byte)((spr.x - 24)), (byte)((spr.y + 24) - 24), true, 0, 0, 11);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1826, (byte)((spr.x - 24)), (byte)((spr.y + 8) - 24), true, 0, 0, 11);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1828, (byte)((spr.x - 24)), (byte)((spr.y) - 24), false, 0, 0, 11);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1844, (byte)((spr.x) - 16), (byte)((spr.y) - 24), false, 0, 0, 11);
                        }
                        else
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1824, (byte)((spr.x + 8)), (byte)((spr.y + 24) - 24), true, 1, 0, 11);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1826, (byte)((spr.x + 8)), (byte)((spr.y + 8) - 24), true, 1, 0, 11);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1828, (byte)((spr.x + 8)), (byte)((spr.y) - 24), false, 1, 0, 11);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1844, (byte)((spr.x)), (byte)((spr.y) - 24), false, 1, 0, 11);
                        }

                        break;
                    case 0x38: //Platform Moving
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1958, (byte)(spr.x), (byte)(spr.y - 16), true, 1, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1958, (byte)(spr.x - 16), (byte)(spr.y - 16), true, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1960, (byte)(spr.x), (byte)(spr.y), true, 1, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1960, (byte)(spr.x - 16), (byte)(spr.y), true, 0, 0, 14);
                        break;
                    case 0x3A: //Pete Boss
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)2024, (byte)((spr.x - 25)), (byte)((spr.y)), true, 0, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1856, (byte)((spr.x - 16)), (byte)((spr.y - 24)), true, 0, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1858, (byte)((spr.x)), (byte)((spr.y - 24)), true, 0, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1860, (byte)((spr.x - 16)), (byte)((spr.y - 8)), true, 0, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1862, (byte)((spr.x)), (byte)((spr.y - 8)), true, 0, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1864, (byte)((spr.x - 16)), (byte)((spr.y + 8)), true, 0, 0, 15);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)((spr.x)), (byte)((spr.y + 8)), true, 0, 0, 15);


                        break;
                    case 0x42://order switches
                        if (spr.param == 0x02) //O P E N Letters
                        {

                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(616 + (spr.unkn * 2)), (byte)(((spr.x - 8) / 8) * 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 2);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(617 + (spr.unkn * 2)), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 2);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(632 + (spr.unkn * 2)), (byte)(((spr.x - 8) / 8) * 8), (byte)(((((spr.y - 8) / 8)) * 8) + 8), false, 0, 0, 2);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(633 + (spr.unkn * 2)), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)((((spr.y - 8) / 8) * 8) + 8), false, 0, 0, 2);
                        }
                        else if (spr.param == 0x00) //order switches
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(992), (byte)(((spr.x - 8) / 8) * 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(993), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(994), (byte)(((spr.x - 8) / 8) * 8), (byte)(((((spr.y - 8) / 8)) * 8) + 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(995), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)((((spr.y - 8) / 8) * 8) + 8), false, 0, 0, 6);
                        }

                        break;
                    case 0x44: //switch sprite
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(496), (byte)(((spr.x - 8) / 8) * 8), (byte)(((spr.y - 8) / 8) * 8), false, 0, 0, 1);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(496), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)(((spr.y - 8) / 8) * 8), false, 1, 0, 1);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(497), (byte)(((spr.x - 8) / 8) * 8), (byte)(((((spr.y - 8) / 8)) * 8) + 8), false, 0, 0, 1);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(497), (byte)((((spr.x - 8) / 8) * 8) + 8), (byte)((((spr.y - 8) / 8) * 8) + 8), false, 1, 0, 1);
                        break;
                    case 0x46: //Platform Moving up n down
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1932, (byte)(spr.x), (byte)(spr.y - 12), true, 1, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1932, (byte)(spr.x - 16), (byte)(spr.y - 12), true, 0, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1952, (byte)(spr.x), (byte)(spr.y + 4), true, 1, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1952, (byte)(spr.x - 16), (byte)(spr.y + 4), true, 0, 0, 11);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1954, (byte)(spr.x - 24), (byte)(spr.y - 12), false, 0, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1970, (byte)(spr.x - 24), (byte)(spr.y - 4), false, 0, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1955, (byte)(spr.x - 24), (byte)(spr.y + 4), false, 0, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1971, (byte)(spr.x - 24), (byte)(spr.y + 12), false, 0, 0, 11);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1954, (byte)(spr.x + 16), (byte)(spr.y - 12), false, 1, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1970, (byte)(spr.x + 16), (byte)(spr.y - 4), false, 1, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1955, (byte)(spr.x + 16), (byte)(spr.y + 4), false, 1, 0, 11);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1971, (byte)(spr.x + 16), (byte)(spr.y + 12), false, 1, 0, 11);

                        break;

                    case 0x48: //Platform Moving randomdir
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x - 24), (byte)(spr.y - 24), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x - 8), (byte)(spr.y - 24), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x + 8), (byte)(spr.y - 24), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x - 24), (byte)(spr.y - 8), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x - 8), (byte)(spr.y - 8), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x + 8), (byte)(spr.y - 8), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x - 24), (byte)(spr.y + 8), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x - 8), (byte)(spr.y + 8), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1866, (byte)(spr.x + 8), (byte)(spr.y + 8), true, 0, 0, 13);


                        break;
                    case 0x4C: //mine cart
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1926, (byte)(spr.x), (byte)(spr.y - 28), true, 1, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1926, (byte)(spr.x - 16), (byte)(spr.y - 28), true, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1928, (byte)(spr.x), (byte)(spr.y - 12), true, 1, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1928, (byte)(spr.x - 16), (byte)(spr.y - 12), true, 0, 0, 14);
                        break;
                    case 0x52: //controllable canon
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1920, (byte)((spr.x) - 16), (byte)((spr.y) - 16), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1920, (byte)((spr.x)), (byte)((spr.y) - 16), true, 1, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1952, (byte)((spr.x) - 16), (byte)((spr.y)), true, 0, 0, 13);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1952, (byte)((spr.x)), (byte)((spr.y)), true, 1, 0, 13);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(429), (byte)(20 * 8), (byte)(22 * 8), false, 0, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(429), (byte)((20 * 8) + 8), (byte)(22 * 8), false, 1, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(445), (byte)(20 * 8), (byte)((22 * 8) + 8), false, 0, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(445), (byte)((20 * 8) + 8), (byte)((22 * 8) + 8), false, 1, 0, 4);

                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(521), (byte)(14 * 8), (byte)(22 * 8), false, 0, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(524), (byte)((14 * 8) + 8), (byte)(22 * 8), false, 0, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(525), (byte)(14 * 8), (byte)((22 * 8) + 8), false, 0, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(354), (byte)((14 * 8) + 8), (byte)((22 * 8) + 8), false, 0, 0, 4);


                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(524), (byte)(16 * 8), (byte)(22 * 8), false, 1, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(521), (byte)((16 * 8) + 8), (byte)(22 * 8), false, 1, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(354), (byte)(16 * 8), (byte)((22 * 8) + 8), false, 1, 0, 4);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(525), (byte)((16 * 8) + 8), (byte)((22 * 8) + 8), false, 1, 0, 4);

                        break;

                    case 0x54: //flag
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1994, (byte)((spr.x + 0) - 5), (byte)((spr.y - 24) - 4), true, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)1996, (byte)((spr.x + 16) - 5), (byte)((spr.y - 24) - 4), true, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)2020, (byte)((spr.x + 0) - 5), (byte)((spr.y - 32) - 4), false, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)2036, (byte)((spr.x + 0) - 5), (byte)((spr.y - 8) - 4), false, 0, 0, 14);
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)2021, (byte)((spr.x + 0) - 5), (byte)((spr.y) - 4), false, 0, 0, 14);

                        break;


                }
            }
        }

    }
}
