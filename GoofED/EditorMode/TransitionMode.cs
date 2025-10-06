using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class TransitionMode
    {
        Game game;
        bool ismouseDown = false;
        public Transition selectedObject = null;
        public Transition clipboardObject = null;
        ushort xC = 0;
        ushort yC = 0;
        int mxDown = 0;
        int myDown = 0;
        
        public TransitionMode(Game game)
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
                    contextMenu.Items.Add("Add Transition");
                    contextMenu.Items[0].Click += TransitionMode_AddTransition_Click;
                    contextMenu.Show((sender as PictureBox), e.Location);
                    return;
            }
            foreach(Transition i in game.levels[game.selectedLevel].maps[game.selectedMap].transitions)
            {
                byte x = (byte)((i.position & 0x1F));
                byte y = (byte)((i.position >> 5 & 0x1F));

                if (e.X >= ((x) * (8*Zoom)) && e.X <= ((x) * (8 * Zoom)) + (32*Zoom))
                {
                    if (e.Y >= ((y) * (8 * Zoom)) && e.Y <= ((y) * (8 * Zoom)) + (32*Zoom))
                    {
                        mxDown = e.X - (x* (8 * Zoom));
                        myDown = e.Y - (y* (8 * Zoom));
                        selectedObject = i;
                        ismouseDown = true;
                        break;
                    }
                }
            }
            


        }

        private void TransitionMode_AddTransition_Click(object sender, EventArgs e)
        {
            selectedObject = new Transition(0,0, 0x0F, 0,0);
            game.levels[game.selectedLevel].maps[game.selectedMap].transitions.Add(selectedObject);
            ismouseDown = true;

        }

        public void mouseMove(MouseEventArgs e, int Zoom)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {
                    ushort x = (ushort)((e.X - mxDown) / (8*Zoom));
                    ushort y = (ushort)((e.Y - myDown) / (8*Zoom));
                    if (x > 250)
                    {
                        x = 0;
                    }
                    if (y > 250)
                    {
                        y = 0;
                    }
                    if (x > 31)
                    {
                        x = 31;
                    }
                    if (y > 27)
                    {
                        y = 27;
                    }
                    ushort pos = (ushort)((x) | (y << 5));
                    selectedObject.position = pos;
                }
            }
        }

        public void mouseUp(MouseEventArgs e)
        {
            ismouseDown = false;
        }

        public void Delete()
        {
            game.levels[game.selectedLevel].maps[game.selectedMap].transitions.Remove(selectedObject);
            selectedObject = null;
        }

        public void Cut()
        {
            clipboardObject = new Transition(selectedObject.tomap,selectedObject.position,selectedObject.dir,selectedObject.xDest,selectedObject.yDest);
            Delete();
        }
        public void Copy()
        {
            clipboardObject = new Transition(selectedObject.tomap, selectedObject.position, selectedObject.dir, selectedObject.xDest, selectedObject.yDest);
        }

        public void Paste()
        {
            Transition c = new Transition(clipboardObject.tomap, clipboardObject.position, clipboardObject.dir, clipboardObject.xDest, clipboardObject.yDest);
            game.levels[game.selectedLevel].maps[game.selectedMap].transitions.Add(c);
            selectedObject = c;
            ismouseDown = true;
        }
    }
}
