using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{

    public class Sprite
    {
        public byte id;
        public byte param;
        public byte unkn;
        public byte x;
        public byte y;
        //$0E $00 $00 $68 $24
        public Sprite(byte id, byte param, byte unkn, byte x, byte y)
        {
            this.x = x;
            this.y = y;
            this.param = param;
            this.id = id;
            this.unkn = unkn;
        }
    }
}
