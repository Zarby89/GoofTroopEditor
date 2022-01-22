using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class BlockDoor
    {
        public List<ushort> addrAllBlocks = new List<ushort>();
        public ushort doorAddr;
        public byte doorRam; //will be generated
        public byte doorDir;
        public bool saved = false;
        public bool drawswitch = false;
        public BlockDoor(ushort[] allBlocks, byte doorRam, byte doorDir, ushort doorAddr, bool saved = false, bool drawswitch = false)
        {
            this.addrAllBlocks = allBlocks.ToList();
            this.doorRam = doorRam;
            this.doorDir = doorDir;
            this.doorAddr = doorAddr;
            this.drawswitch = drawswitch;
            this.saved = saved;
        }
    }
}
