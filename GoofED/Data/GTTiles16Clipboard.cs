using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    [Serializable]
    public class GTTiles16Clipboard
    {
        public ushort[] tiles;
        public byte width;
        public byte height;
        public GTTiles16Clipboard(ushort[] tiles, byte width, byte height)
        {
            this.tiles = tiles;
            this.width = width;
            this.height = height;
        }


    }
}
