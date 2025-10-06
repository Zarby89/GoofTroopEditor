using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class HookMode : EditorMode
    {

        Game game;
        bool ismouseDown = false;
        public Hook selectedObject = null;
        public byte selectedObjectX = 0;
        public Hook clipboardObject = null;
        //type  0 = <-> 96, 1 = <-> 128, 2 = ^v 96, 3 ^v 128

        ushort xC = 0;
        ushort yC = 0;
        int mxDown = 0;
        int myDown = 0;
        public HookMode(Game game) : base(game)
        {
            this.game = game;
        }


        public void mouseDown(object sender, MouseEventArgs e, int Zoom)
        {

            if (e.Button == MouseButtons.Right)
            {

                xC = (ushort)(e.X / Zoom);
                yC = (ushort)(e.Y / Zoom);
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Add Hookset");
                contextMenu.Items[0].Click += HookMode_AddHook_Click;
                contextMenu.Show((sender as PictureBox), e.Location);
                return;
            }
            foreach (Hook i in game.levels[game.selectedLevel].maps[game.selectedMap].hooks)
            {
                if (e.X >= ((i.x) * Zoom) && e.X <= ((i.x) * Zoom) + (8*Zoom))
                {
                    if (e.Y >= ((i.y) * Zoom) && e.Y <= ((i.y) * Zoom) + (8 * Zoom))
                    {
                        selectedObject = i;
                        selectedObjectX = 0;
                        ismouseDown = true;
                        break;
                    }
                }

                if (e.X >= ((i.x2) * Zoom) && e.X <= ((i.x2) * Zoom) + (8 * Zoom))
                {
                    if (e.Y >= ((i.y2 ) * Zoom) && e.Y <= ((i.y2) * Zoom) + (8 * Zoom))
                    {
                        selectedObject = i;
                        selectedObjectX = 1;
                        ismouseDown = true;
                        break;
                    }
                }
            }



        }

        private void HookMode_AddHook_Click(object sender, EventArgs e)
        {
            if (game.levels[game.selectedLevel].maps[game.selectedMap].hooks.Count == 2)
            {
                MessageBox.Show("Hooks are limited to 2 set per map");
                return;
            }
            selectedObject = new Hook((byte)xC, (byte)yC, (byte)(xC+96), (byte)(yC));
            game.levels[game.selectedLevel].maps[game.selectedMap].hooks.Add(selectedObject);

        }

        public void mouseMove(MouseEventArgs e, int Zoom)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {
                    if (selectedObjectX == 0)
                    {
                        if (GlobalOptions.movementLock8x8)
                        {
                            selectedObject.x = (byte)(((e.X / Zoom) / 8) * 8);
                            selectedObject.y = (byte)(((e.Y / Zoom) / 8) * 8);
                        }
                        else
                        {
                            selectedObject.x = (byte)((e.X / Zoom));
                            selectedObject.y = (byte)((e.Y / Zoom));
                        }
                        if (selectedObject.type == 0)
                        {
                            selectedObject.x2 = (byte)(selectedObject.x + 96);
                            selectedObject.y2 = selectedObject.y;
                        }
                        else if (selectedObject.type == 1)
                        {
                            selectedObject.x2 = (byte)(selectedObject.x + 128);
                            selectedObject.y2 = selectedObject.y;
                        }
                        else if (selectedObject.type == 2)
                        {
                            selectedObject.x2 = (byte)(selectedObject.x);
                            selectedObject.y2 = (byte)(selectedObject.y+96);
                        }
                        else if (selectedObject.type == 3)
                        {
                            selectedObject.x2 = (byte)(selectedObject.x);
                            selectedObject.y2 = (byte)(selectedObject.y+128);
                        }
                    }
                    else
                    {
                        if (GlobalOptions.movementLock8x8)
                        {
                            selectedObject.x2 = (byte)(((e.X / Zoom) / 8) * 8);
                            selectedObject.y2 = (byte)(((e.Y / Zoom) / 8) * 8);
                        }
                        else
                        {
                            selectedObject.x2 = (byte)((e.X / Zoom));
                            selectedObject.y2 = (byte)((e.Y / Zoom));
                        }
                        if (selectedObject.type == 0)
                        {
                            selectedObject.x = (byte)(selectedObject.x2 - 96);
                            selectedObject.y = selectedObject.y2;
                        }
                        else if (selectedObject.type == 1)
                        {
                            selectedObject.x = (byte)(selectedObject.x2 - 128);
                            selectedObject.y = selectedObject.y2;
                        }
                        else if (selectedObject.type == 2)
                        {
                            selectedObject.x = (byte)(selectedObject.x2);
                            selectedObject.y = (byte)(selectedObject.y2 - 96);
                        }
                        else if (selectedObject.type == 3)
                        {
                            selectedObject.x = (byte)(selectedObject.x2);
                            selectedObject.y = (byte)(selectedObject.y2 - 128);
                        }
                    }

                }
            }
        }

        public void mouseUp(MouseEventArgs e)
        {
            ismouseDown = false;
        }

        public void Delete()
        {
            game.levels[game.selectedLevel].maps[game.selectedMap].hooks.Remove(selectedObject);
            selectedObject = null;
            selectedObjectX = 0;
        }

        public void Cut()
        {
            //clipboardObject = new Item(selectedItem.id, selectedItem.ram, selectedItem.x, selectedItem.y);
            //Delete();
        }
        public void Copy()
        {
            //clipboardObject = new Item(selectedItem.id, selectedItem.ram, selectedItem.x, selectedItem.y);
        }

        public void Paste()
        {
            //Item c = new Item(clipboardObject.id, clipboardObject.ram, clipboardObject.x, clipboardObject.y);
            //game.levels[game.selectedLevel].maps[game.selectedMap].items.Add(c);
            //selectedItem = c;
            //ismouseDown = true;
        }


    }
}
