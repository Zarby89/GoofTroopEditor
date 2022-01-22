using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class EnemyDoor
    {
        public ushort doorAddr = 0;
        public byte doorRamDir = 0;
        public byte doorSize = 0;
        public byte enemyCount = 0;
        public bool save = false;
        public bool explosing = false;
        //door type 0x21
        //enemyCount, doorRAM, doorAddr, doorSize

        //door type 0x23
        //enemyCount, doorDir, doorAddr, doorSize

        public EnemyDoor(byte enemyCount, byte doorRamDir, ushort doorAddr, byte doorSize, bool save, bool explosion = false)
        {
            this.save = save;
            this.enemyCount = enemyCount;
            this.doorRamDir = doorRamDir;

            this.doorAddr = doorAddr;
            this.doorSize = doorSize;
            this.explosing = explosion;
        }
    }
}
