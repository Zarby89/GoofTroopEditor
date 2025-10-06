using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class ItemMode
    {
        Game game;
        bool ismouseDown = false;
        public Item selectedObject = null;
        public Item clipboardObject = null;
        ushort xC = 0;
        ushort yC = 0;
        int mxDown = 0;
        int myDown = 0;
        public ItemMode(Game game)
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
                    contextMenu.Items.Add("Add Object");
                    contextMenu.Items[0].Click += ItemMode_AddItem_Click;
                    contextMenu.Show((sender as PictureBox), e.Location);
                    return;
            }
            foreach(Item i in game.levels[game.selectedLevel].maps[game.selectedMap].items)
            {
                if (e.X >= ((i.x-8)*Zoom) && e.X <= ((i.x-8)*Zoom)+(16*Zoom))
                {
                    if (e.Y >= ((i.y-8) * Zoom) && e.Y <= ((i.y-8) * Zoom) + (16 * Zoom))
                    {
                        mxDown = e.X - ((i.x) * Zoom);
                        myDown = e.Y - ((i.y) * Zoom);
                        selectedObject = i;
                        ismouseDown = true;
                        break;
                    }
                }
            }
            


        }

        private void ItemMode_AddItem_Click(object sender, EventArgs e)
        {
            selectedObject = new Item(8,0,(byte)xC, (byte)yC);
            game.levels[game.selectedLevel].maps[game.selectedMap].items.Add(selectedObject);

        }

        public void mouseMove(MouseEventArgs e, int Zoom)
        {
            
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {
                    if (GlobalOptions.movementLock8x8)
                    {
                        selectedObject.x = (byte)((((e.X - mxDown) / Zoom)/8)*8);
                        selectedObject.y = (byte)((((e.Y - myDown) / Zoom)/8)*8);
                    }
                    else
                    {
                        selectedObject.x = (byte)((((e.X - mxDown) / Zoom)));
                        selectedObject.y = (byte)((((e.Y - myDown) / Zoom)));
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
            game.levels[game.selectedLevel].maps[game.selectedMap].items.Remove(selectedObject);
            selectedObject = null;
        }

        public void Cut()
        {
            clipboardObject = new Item(selectedObject.id, selectedObject.ram, selectedObject.x, selectedObject.y);
            Delete();
        }
        public void Copy()
        {
            clipboardObject = new Item(selectedObject.id, selectedObject.ram, selectedObject.x, selectedObject.y);
        }

        public void Paste()
        {
            Item c = new Item(clipboardObject.id, clipboardObject.ram, clipboardObject.x, clipboardObject.y);
            game.levels[game.selectedLevel].maps[game.selectedMap].items.Add(c);
            selectedObject = c;
            ismouseDown = true;
        }
    }
}
