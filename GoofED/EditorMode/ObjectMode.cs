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
        public void mouseDown(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Right)
            {
                ushort x = (ushort)(e.X / 16);
                ushort y = (ushort)(e.Y / 16);
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

                if (e.X >= ((x)*16) && e.X <= ((x)*16)+32)
                {
                    if (e.Y >= ((y) * 16) && e.Y <= ((y) * 16) + 32)
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

        public void mouseMove(MouseEventArgs e)
        {
            if (ismouseDown == true)
            {
                if (selectedObject != null)
                {
                    ushort x = (ushort)(e.X / 16);
                    ushort y = (ushort)(e.Y / 16);
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
