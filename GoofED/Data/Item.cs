using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class Item
    {
        public byte id;
        public byte x;
        public byte y;
        public byte ram;
        public Item(byte id, byte ram, byte x, byte y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.ram = ram;
        }

    }
}
