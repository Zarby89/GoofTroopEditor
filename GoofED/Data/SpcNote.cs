using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class SpcNote
    {
        public int addr = 0;
        public string name = "";
        public byte[] bytes;

        public SpcNote(int addr, string name, byte[] bytes)
        {
            this.addr = addr;
            this.name = name;
            this.bytes = bytes;
        }


    }
