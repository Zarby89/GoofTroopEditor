using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class LockedDoor
    {
        public ushort doorAddr;
        public byte doorRam;
        public byte doorDir;
        public bool boss = false;
        public LockedDoor(ushort doorAddr, byte doorDir,  byte doorRam, bool boss = false)
        {
            this.doorRam = doorRam;
            this.doorDir = doorDir;
            this.doorAddr = doorAddr;
            this.boss = boss;
        }

    }
}
