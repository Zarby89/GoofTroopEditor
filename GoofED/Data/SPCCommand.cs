using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class SPCCommand
    {
        public byte length = 0;
        public string name = "";

        public SPCCommand(string name, byte length)
        {
            this.name = name;
            this.length = length;
        }

    }

