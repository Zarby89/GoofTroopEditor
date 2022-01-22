using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    //;XX  YY  ADDR   POS RAM
    //$82 $72 $D0 $01 $07 $00 

    //Generate addr from X,Y
    //(X / 8) + ((Y / 8)*32)

    public class Plank
    {
            public byte x, y = 0;
            public Plank(byte x, byte y)
            {
                this.x = x;
                this.y = y;
            }

    }
}
