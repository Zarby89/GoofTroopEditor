using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class Transition
    {

        public byte tomap;
        public ushort position;
        public byte dir;
        public byte xDest;
        public byte yDest;
        public bool BottomLayer = false;
        public Transition(byte tomap, ushort position, byte dir, byte xDest, byte yDest, bool bottomLayer)
        {
            this.tomap = tomap;
            this.position = position;
            this.dir = dir;
            this.xDest = xDest;
            this.yDest = yDest;
            BottomLayer = bottomLayer;
        }

    }
}
