using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public static class Utils
    {
        public static int SnesToPc(int addr)
        {
            return (addr & 0x7FFF) | ((addr & 0x7F0000) >> 1);
        }

        public static int PcToSnes(int addr)
        {
            byte[] b = BitConverter.GetBytes(addr);
            b[2] = (byte)(b[2] * 2);
            if (b[1] >= 0x80)
                b[2] += 1;
            else b[1] += 0x80;

            b[2] += 0x80; //return it back into a FastROM address
            return BitConverter.ToInt32(b, 0);
        }

        public static int AddressFromBytes(byte addr1, byte addr2, byte addr3)
        {
            return (addr1 << 16) | (addr2 << 8) | addr3;
        }

        public static short AddressFromBytes(byte addr1, byte addr2)
        {
            return (short)((addr1 << 8) | (addr2));
        }

    }
}
