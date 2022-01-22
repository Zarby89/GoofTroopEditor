using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class EnemyDoorMode
    {

        Game game;
        bool ismouseDown = false;
        public EnemyDoor selectedObject = null;
        public EnemyDoor clipboardObject = null;
        
        ushort xC = 0;
        ushort yC = 0;
        int mxDown = 0;
        int myDown = 0;

        public EnemyDoorMode(Game game)
        {
            this.game = game;
        }


        public void mouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {

                xC = (ushort)(e.X / 2);
                yC = (ushort)(e.Y / 2);
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Add Enemy Door");
                contextMenu.Items[0].Click += LockedDoorMode_AddLockeddoor_Click;

                contextMenu.Show((sender as PictureBox), e.Location);
                return;
            }
            foreach (EnemyDoor i in game.levels[game.selectedLevel].maps[game.selectedMap].enemyDoors)
            {
                byte x = (byte)(((i.doorAddr & 0x1F)));
                byte y = (byte)(((i.doorAddr & 0x3E0) >> 5));
                if (e.X >= ((x) * 16) && e.X <= ((x) * 16) + 64)
                {
                    if (e.Y >= ((y) * 16) && e.Y <= ((y) * 16) + 64)
                    {
                        selectedObject = i;
                        mxDown = e.X - (x * 16);
                        myDown = e.Y - (y * 16);
                        ismouseDown = true;
                        break;
                    }
                }


            }



        }

        private void LockedDoorMode_AddLockeddoor_Click(object sender, EventArgs e)
        {
            selectedObject = new EnemyDoor(0, 0, 0, 0, true);
            game.levels[game.selectedLevel].maps[game.selectedMap].enemyDoors.Add(selectedObject);

        }



        public void mouseMove(MouseEventArgs e)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {

                    selectedObject.doorAddr = (ushort)((((e.Y - myDown) / 16) * 32) + ((e.X - mxDown) / 16)); ;

                }
            }
        }

        public void mouseUp(MouseEventArgs e)
        {
            ismouseDown = false;
        }

        public void Delete()
        {
            game.levels[game.selectedLevel].maps[game.selectedMap].enemyDoors.Remove(selectedObject);
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
