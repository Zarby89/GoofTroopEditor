using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    class ItemsDraw
    {
        public void Draw(Graphics g, Game game, Item selectedItem, bool debug)
        {

            //Draw Items
            foreach (Item item in game.levels[game.selectedLevel].maps[game.selectedMap].items)
            {
                if (debug)
                {
                    g.DrawRectangle(Pens.Aqua, new Rectangle((item.x * 2) - 16, (item.y * 2) - 16, 32, 32));
                    g.DrawString(item.id.ToString("X2") + " " + item.ram.ToString("X2") + "\r\n" + item.x, new Font("arial", 16, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, new Point(item.x * 2, item.y * 2));
                }
                if (item == selectedItem)
                {
                    g.DrawRectangle(new Pen(Brushes.Aqua, 2), new Rectangle((item.x * 2) - 16, (item.y * 2) - 16, 32, 32));
                }

                switch (item.id)
                {
                    case 00:
                        break;

                    case 0x09:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1612), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 11);
                        break;
                    case 0x0A:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1604), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 11);
                        break;
                    case 0x0B:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1606), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 11);
                        break;
                    case 0x0C:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1600), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 11);
                        break;
                    case 0x0D:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1610), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 10);
                        break;
                    case 0x0E:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1608), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 11);
                        break;
                    case 0x22:
                        g.DrawRectangle(Pens.Aqua, new Rectangle((item.x * 2) - 16, (item.y * 2) - 16, 32, 32));
                        if ((item.ram & 0xF0) != 0)
                        {
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(992), (byte)(((item.x - 8) / 8) * 8), (byte)(((item.y + 8) / 8) * 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(993), (byte)((((item.x - 8) / 8) * 8) + 8), (byte)(((item.y + 8) / 8) * 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(994), (byte)(((item.x - 8) / 8) * 8), (byte)(((((item.y + 8) / 8)) * 8) + 8), false, 0, 0, 6);
                            GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(995), (byte)((((item.x - 8) / 8) * 8) + 8), (byte)((((item.y + 8) / 8) * 8) + 8), false, 0, 0, 6);
                        }
                        break;
                    case 0x2E:
                        g.DrawRectangle(Pens.Aqua, new Rectangle((item.x * 2) - 16, (item.y * 2) - 16, 32, 32));
                        break;
                    case 0x08:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1602), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 11);
                        break;
                    case 0x40:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1546), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 10);
                        break;
                    case 0x42:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1548), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 10);
                        break;
                    case 0x44:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1550), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 10);
                        break;
                    case 0x46:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1544), (byte)((item.x - 8)), (byte)((item.y - 8)), true, 0, 0, 10);
                        break;
                    case 0x63:
                        GFX.DrawFromVRAM(GFX.SprBuffer, (ushort)(1996), (byte)(item.x), (byte)(item.y), true, 0, 0, 12);
                        break;
                    default:
                        g.DrawRectangle(Pens.Aqua, new Rectangle((item.x * 2) - 16, (item.y * 2) - 16, 32, 32));
                        break;

                }

            }
        }
    }
}
