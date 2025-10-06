using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    class LockedDoorMode
    {

        Game game;
        bool ismouseDown = false;
        public LockedDoor selectedObject = null;
        public LockedDoor clipboardObject = null;
 
        ushort xC = 0;
        ushort yC = 0;
        int mxDown = 0;
        int myDown = 0;
        public LockedDoorMode(Game game)
        {
            this.game = game;
        }


        public void mouseDown(object sender, MouseEventArgs e, int Zoom)
        {

            if (e.Button == MouseButtons.Right)
            {

                xC = (ushort)(e.X / 2);
                yC = (ushort)(e.Y / 2);
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Add Locked Door");
                contextMenu.Items[0].Click += LockedDoorMode_AddLockeddoor_Click;
                
                contextMenu.Show((sender as PictureBox), e.Location);
                return;
            }
            foreach (LockedDoor i in game.levels[game.selectedLevel].maps[game.selectedMap].lockedDoors)
            {
                byte x = (byte)(((i.doorAddr & 0x1F)));
                byte y = (byte)(((i.doorAddr & 0x3E0) >> 5));
                if (e.X >= ((x) * (8 * Zoom)) && e.X <= ((x) * (8 * Zoom)) + (32 * Zoom))
                {
                    if (e.Y >= ((y) * (8 * Zoom)) && e.Y <= ((y) * (8 * Zoom)) + (32 * Zoom))
                    {
                        selectedObject = i;
                        mxDown = e.X - (x * (8 * Zoom));
                        myDown = e.Y - (y * (8 * Zoom));
                        ismouseDown = true;
                        break;
                    }
                }


            }



        }

        private void LockedDoorMode_AddLockeddoor_Click(object sender, EventArgs e)
        {
            selectedObject = new LockedDoor(0, 0, 0, true);
            game.levels[game.selectedLevel].maps[game.selectedMap].lockedDoors.Add(selectedObject);

        }

 

        public void mouseMove(MouseEventArgs e, int Zoom)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {

                        selectedObject.doorAddr = (ushort)((((e.Y-myDown) / (8 * Zoom)) * 32) + ((e.X-mxDown) / (8 * Zoom))); ;

                }
            }
        }

        public void mouseUp(MouseEventArgs e)
        {
            ismouseDown = false;
        }

        public void Delete()
        {
            game.levels[game.selectedLevel].maps[game.selectedMap].lockedDoors.Remove(selectedObject);
            selectedObject = null;
        }

        public void Cut()
        {
            //clipboardObject = new Item(selectedObject.id, selectedObject.ram, selectedObject.x, selectedObject.y);
            Delete();
        }
        public void Copy()
        {
            //clipboardObject = new Item(selectedObject.id, selectedObject.ram, selectedObject.x, selectedObject.y);
        }

        public void Paste()
        {
            /*Item c = new Item(clipboardObject.id, clipboardObject.ram, clipboardObject.x, clipboardObject.y);
            game.levels[game.selectedLevel].maps[game.selectedMap].items.Add(c);
            selectedObject = c;
            ismouseDown = true;*/
        }
    }
}
