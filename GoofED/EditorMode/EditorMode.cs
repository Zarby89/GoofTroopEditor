using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class EditorMode
    {
        Game game;
        bool ismouseDown = false;
        public object selectedObject = null;
        public object clipboardObject = null;
        public EditorMode(Game game)
        {
            this.game = game;
        }
        public virtual void mouseDown(object sender, MouseEventArgs e)
        {



        }

        public virtual void mouseMove(MouseEventArgs e)
        {

        }

        public virtual void mouseUp(MouseEventArgs e)
        {

        }

        public virtual void Delete()
        {

        }

        public virtual void Cut()
        {

        }
        public virtual void Copy()
        {

        }

        public virtual void Paste()
        {

        }
    }
}
