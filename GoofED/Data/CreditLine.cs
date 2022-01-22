using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class CreditLine
    {
        public byte linesSkip;
        public byte xStart;
        public byte cCount;
        public byte palette;
        public byte[] data;
        public int yPos = 0;
        public CreditLine(byte linesSkip, byte xStart, byte cCount, byte palette, byte[] data)
        {
            this.linesSkip = linesSkip;
            this.xStart = xStart;
            this.cCount = cCount;
            this.palette = palette;
            this.data = data;
        }


    }
}
