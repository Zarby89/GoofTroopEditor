using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    class ASMPlugin
    {
        public string content;
        public string name;
        public ASMPlugin(string name, string content)
        {
            this.content = content;
            this.name = name;
        }


    }
}
