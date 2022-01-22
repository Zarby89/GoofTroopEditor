using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class Level
    {
        public byte song = 0x00;
        public byte gfx1 = 0x00;
        public byte gfx2 = 0x00;
        public List<Map> maps = new List<Map>();
        public byte mapCount = 16;
        public byte index = 0;

        public Level(Game game,byte index, byte mapCount = 16, byte mapStart = 0)
        {
            this.index = index;
            this.mapCount = mapCount;
            int gfxgrpPtr = 0x830000 + game.rom.ReadShort(Constants.BGGfx_Level_Ptr);
            this.gfx1 = (byte)(game.rom.ReadByte(Utils.SnesToPc(gfxgrpPtr) + (index * 2)));
            this.gfx2 = (byte)(game.rom.ReadByte(Utils.SnesToPc(gfxgrpPtr + 1) + (index * 2)));

            int songPtr = 0x830000 + game.rom.ReadShort(Constants.SongAddr);
            this.song = (byte)(game.rom.ReadByte(Utils.SnesToPc(songPtr)+ (index)));

            for (int i = 0; i < mapCount; i++)
            {
                maps.Add(new Map(game, (byte)(mapStart + i), (byte)i,this.index));
            }
        }

    }
}
