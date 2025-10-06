using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class PlankMode : EditorMode
    {

        Game game;
        bool ismouseDown = false;
        public Plank selectedObject = null;
        public Plank clipboardObject = null;
        //type  0 = <-> 96, 1 = <-> 128, 2 = ^v 96, 3 ^v 128

        ushort xC = 0;
        ushort yC = 0;
        int mxDown = 0;
        int myDown = 0;
        public PlankMode(Game game) : base(game)
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
                contextMenu.Items.Add("Add PlankHole");
                contextMenu.Items[0].Click += HookMode_AddHook_Click;
                contextMenu.Show((sender as PictureBox), e.Location);
                return;
            }
            foreach (Plank i in game.levels[game.selectedLevel].maps[game.selectedMap].planks)
            {
                if (e.X >= ((i.x) * Zoom) && e.X <= ((i.x) * Zoom) + (16*Zoom))
                {
                    if (e.Y >= ((i.y) * Zoom) && e.Y <= ((i.y) * Zoom) + (16*Zoom))
                    {
                        selectedObject = i;;
                        ismouseDown = true;
                        break;
                    }
                }
            }



        }

        private void HookMode_AddHook_Click(object sender, EventArgs e)
        {

            selectedObject = new Plank((byte)xC, (byte)yC);
            game.levels[game.selectedLevel].maps[game.selectedMap].planks.Add(selectedObject);

        }

        public void mouseMove(MouseEventArgs e, int Zoom)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
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
                }
            }
        }

        public void mouseUp(MouseEventArgs e)
        {
            ismouseDown = false;
        }

        public void Delete()
        {
            game.levels[game.selectedLevel].maps[game.selectedMap].planks.Remove(selectedObject);
            selectedObject = null;
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
