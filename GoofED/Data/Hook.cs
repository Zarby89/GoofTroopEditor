using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class Hook
    {
        public byte x, y = 0;

        public byte x2, y2 = 0;
        public byte type = 0;
        public Hook(byte x, byte y, byte x2, byte y2)
        {
            this.x = x;
            this.y = y;

            this.x2 = x2;
            this.y2 = y2;

            if (x2 - x == 96)
            {
                type = 0;
            }
            else if (x2 - x == 128)
            {
                type = 1;
            }
            else if (y2 - y == 96)
            {
                type = 2;
            }
            else if (y2 - y == 128)
            {
                type = 3;
            }
        }


    }
}
