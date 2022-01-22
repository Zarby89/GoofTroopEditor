using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    class BlockDoorMode
    {

        Game game;
        bool ismouseDown = false;
        public BlockDoor selectedObject = null;
        public BlockDoor clipboardObject = null;
        int selectedIndex = -1;
        ushort xC = 0;
        ushort yC = 0;
        int mxDown = 0;
        int myDown = 0;
        public BlockDoorMode(Game game)
        {
            this.game = game;
        }


        public void mouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {

                xC = (ushort)(e.X);
                yC = (ushort)(e.Y);
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Add Door and Block");
                if(selectedObject != null)
                {
                    contextMenu.Items.Add("Add Block for selected door");
                    contextMenu.Items[1].Click += BlockDoorMode_AddBlock_Click;
                }


                contextMenu.Items[0].Click += BlockDoorMode_AddBlockdoor_Click;
                
                contextMenu.Show((sender as PictureBox), e.Location);
                return;
            }
            foreach (BlockDoor i in game.levels[game.selectedLevel].maps[game.selectedMap].blockDoors)
            {
                byte x = (byte)(((i.doorAddr & 0x1F)));
                byte y = (byte)(((i.doorAddr & 0x3E0) >> 5));
                if (e.X >= ((x) * 16) && e.X <= ((x) * 16) + 64)
                {
                    if (e.Y >= ((y) * 16) && e.Y <= ((y) * 16) + 64)
                    {
                        mxDown = e.X - (x * 16);
                        myDown = e.Y - (y * 16);
                        selectedObject = i;
                        selectedIndex = 100; //100 is the door
                        ismouseDown = true;
                        break;
                    }
                }

                for(int j = 0;j < i.addrAllBlocks.Count;j++)
                {
                    byte x2 = (byte)(((i.addrAllBlocks[j] & 0x1F)));
                    byte y2 = (byte)(((i.addrAllBlocks[j] & 0x3E0) >> 5));
                    if (e.X >= ((x2) * 16) && e.X <= ((x2) * 16) + 32)
                    {
                        if (e.Y >= ((y2) * 16) && e.Y <= ((y2) * 16) + 32)
                        {
                            mxDown = e.X - (x2 * 16);
                            myDown = e.Y - (y2 * 16);
                            selectedObject = i;
                            selectedIndex = j; //100 is the door
                            ismouseDown = true;
                            break;
                        }
                    }


                }
            }



        }

        private void BlockDoorMode_AddBlockdoor_Click(object sender, EventArgs e)
        {
            ushort p = (ushort)(((yC / 16) * 32) + (xC / 16));
            selectedObject = new BlockDoor(new ushort[] { p }, 0, 0, p);
            game.levels[game.selectedLevel].maps[game.selectedMap].blockDoors.Add(selectedObject);

        }

        private void BlockDoorMode_AddBlock_Click(object sender, EventArgs e)
        {
            ushort p = (ushort)(((yC / 16) * 32) + (xC / 16));
            selectedObject.addrAllBlocks.Add(p);
            selectedIndex = selectedObject.addrAllBlocks.Count - 1;

        }

        public void mouseMove(MouseEventArgs e)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {
                    if (selectedIndex == 100) //door
                    {
                        selectedObject.doorAddr = (ushort)((((e.Y-myDown) / 16) * 32) + ((e.X-mxDown) / 16)); ;
                    }
                    else
                    {
                        selectedObject.addrAllBlocks[selectedIndex] = (ushort)((((e.Y-myDown)/16)*32) + ((e.X-mxDown) / 16));
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
            if (selectedIndex == 100)
            {
                game.levels[game.selectedLevel].maps[game.selectedMap].blockDoors.Remove(selectedObject);
                selectedObject = null;
            }
            else
            {
                selectedObject.addrAllBlocks.RemoveAt(selectedIndex);
            }
            
            selectedIndex = 100;
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
