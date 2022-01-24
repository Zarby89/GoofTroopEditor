using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class ROM
    {
        public byte[] data = new byte[0x100000];
        public byte ReadByte(int addr)
        {
            return (byte)(data[addr]);
        }

        public ushort ReadShort(int addr)
        {
            return (ushort)((data[addr + 1] << 8) + data[addr]);
        }

        public int ReadLong(int addr)
        {
            return ((data[addr + 2] << 16) + (data[addr + 1] << 8) + data[addr]);
        }

        public Color ReadColor(int addr)
        {
            short c = (short)((data[addr + 1] << 8) + data[addr]);
            return Color.FromArgb(((c & 0x1F) * 8), ((c & 0x3E0) >> 5) * 8, ((c & 0x7C00) >> 10) * 8);
        }

        public void WriteByte(int addr, byte v)
        {
            data[addr] = (byte)(v & 0xFF);

        }

        public void WriteBytes(int addr, byte[] v)
        {
            for(int i = 0;i<v.Length;i++)
            {
                data[addr+i] = v[i];
            }
        }

        public void WriteShort(int addr, short v)
        {
            data[addr+1] = (byte)(v >> 8);
            data[addr] = (byte)(v & 0xFF);
        }

        public void WriteUShort(int addr, ushort v)
        {
            data[addr + 1] = (byte)(v >> 8);
            data[addr] = (byte)(v & 0xFF);
        }

        public void WriteLong(int addr, int v)
        {
            data[addr + 2] = (byte)(v >> 16);
            data[addr + 1] = (byte)(v >> 8);
            data[addr] = (byte)(v & 0xFF);
        }

        public void WriteColor(int addr, Color c)
        {
            short cs = (short)((c.R / 8) + ((c.G / 8) << 5) + ((c.B / 8) << 10));
            WriteShort(addr, cs);
        }

        public ushort ReadShortI(int addr)
        {
            return (ushort)((data[addr] << 8) + data[addr + 1]);
        }
        public int ReadLongI(int addr)
        {
            return ((data[addr] << 16) + (data[addr + 1] << 8) + data[addr + 2]);
        }


    }
}
