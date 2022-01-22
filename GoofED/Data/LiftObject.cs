using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class LiftObject
    {
        public byte id = 0;
        public ushort position = 0;
        public LiftObject(byte id, ushort position)
        {
            this.id = id;
            this.position = position;
        }

    }
}
