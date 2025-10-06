using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class ObjectMode : EditorMode
    {
        Game game;
        bool ismouseDown = false;
        public LiftObject selectedObject = null;
        public LiftObject clipboardObject = null;
        public ObjectMode(Game game) : base(game)
        {
            this.game = game;
        }

        ushort posContext = 0;
        public void mouseDown(object sender, MouseEventArgs e, int Zoom)
        {
            
            if (e.Button == MouseButtons.Right)
            {
                ushort x = (ushort)(e.X / (8*Zoom));
                ushort y = (ushort)(e.Y / (8 * Zoom));
                posContext = (ushort)((x << 1) | y << 6);
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Add Object");
                contextMenu.Items[0].Click += ObjectMode_AddObject_Click;
                contextMenu.Show((sender as PictureBox),e.Location);
                return;
            }

            foreach(LiftObject obj in game.levels[game.selectedLevel].maps[game.selectedMap].objects)
            {
                byte x = (byte)(((obj.position & 0x3F) >> 1));
                byte y = (byte)(((obj.position & 0x3FC0) >> 6));

                if (e.X >= ((x)* (8 * Zoom)) && e.X <= ((x)* (8 * Zoom)) +(Zoom*16))
                {
                    if (e.Y >= ((y) * (8 * Zoom)) && e.Y <= ((y) * (8 * Zoom)) + (Zoom * 16))
                    {
                        selectedObject = obj;
                        ismouseDown = true;
                        break;
                    }
                }
            }
            


        }

        private void ObjectMode_AddObject_Click(object sender, EventArgs e)
        {
            
            selectedObject = new LiftObject(0, posContext);
            game.levels[game.selectedLevel].maps[game.selectedMap].objects.Add(selectedObject);

        }

        public void mouseMove(MouseEventArgs e, int Zoom)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {
                    ushort x = (ushort)(e.X / (8 * Zoom));
                    ushort y = (ushort)(e.Y / (8 * Zoom));
                    ushort pos = (ushort)((x << 1) | y << 6);
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
            game.levels[game.selectedLevel].maps[game.selectedMap].objects.Remove(selectedObject);
            selectedObject = null;
        }

        public void Cut()
        {
            clipboardObject = new LiftObject(selectedObject.id, selectedObject.position);
            Delete();
        }
        public void Copy()
        {
            clipboardObject = new LiftObject(selectedObject.id, selectedObject.position);
        }

        public void Paste()
        {
            LiftObject c = new LiftObject(clipboardObject.id, clipboardObject.position);
            game.levels[game.selectedLevel].maps[game.selectedMap].objects.Add(c);
            selectedObject = c;
            ismouseDown = true;
        }

    }
}
