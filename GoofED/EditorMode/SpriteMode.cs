using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class SpriteMode
    {
        Game game;
        bool ismouseDown = false;
        public Sprite selectedObject = null;
        public Sprite clipboardObject = null;
        int xC = 0;
        int yC = 0;
        int mxDown = 0;
        int myDown = 0;
        public SpriteMode(Game game)
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
                contextMenu.Items.Add("Add Sprite");
                contextMenu.Items[0].Click += SpriteMode_Addsprite_Click;
                contextMenu.Show((sender as PictureBox), e.Location);
                return;
            }

            foreach (Sprite spr in game.levels[game.selectedLevel].maps[game.selectedMap].sprites)
            {

                if ((e.X/2) >= ((spr.x)) && (e.X/2) <= ((spr.x))+16)
                {
                    if ((e.Y/2) >= ((spr.y)) && (e.Y/2) <= ((spr.y)) + 16)
                    {
                        mxDown = (e.X) - (spr.x*2);
                        myDown = (e.Y) - (spr.y*2);
                        selectedObject = spr;
                        ismouseDown = true;
                        break;
                    }
                }
            }
            


        }

        private void SpriteMode_Addsprite_Click(object sender, EventArgs e)
        {
            selectedObject = new Sprite(0, 0, 0, 0, 0);
            game.levels[game.selectedLevel].maps[game.selectedMap].sprites.Add(selectedObject);
            ismouseDown = true;
        }

        public void mouseMove(MouseEventArgs e)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {
                    byte x = (byte)((e.X - mxDown) / 2);
                    byte y = (byte)((e.Y - myDown) / 2);
                    if (GlobalOptions.movementLock8x8)
                    {
                        x = (byte)((((e.X - mxDown) / 2)/8)*8);
                        y = (byte)((((e.Y - myDown) / 2)/8)*8);
                    }

                    selectedObject.x = x;
                    selectedObject.y = y;
                }
            }
        }

        public void mouseUp(MouseEventArgs e)
        {
            ismouseDown = false;
        }


        public void Delete()
        {
            game.levels[game.selectedLevel].maps[game.selectedMap].sprites.Remove(selectedObject);
            selectedObject = null;
        }

        public void Cut()
        {
            clipboardObject = new Sprite(selectedObject.id, selectedObject.param,selectedObject.unkn, selectedObject.x, selectedObject.y);
            Delete();
        }
        public void Copy()
        {
            clipboardObject = new Sprite(selectedObject.id, selectedObject.param, selectedObject.unkn, selectedObject.x, selectedObject.y);
        }

        public void Paste()
        {
            Sprite c = new Sprite(clipboardObject.id, clipboardObject.param, clipboardObject.unkn, clipboardObject.x, clipboardObject.y);
            game.levels[game.selectedLevel].maps[game.selectedMap].sprites.Add(c);
            selectedObject = c;
            ismouseDown = true;
        }
    }
}
