using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public partial class AsmConfig : Form
    {
        public int addr = 0x908000;
        public AsmConfig(int addr)
        {
            InitializeComponent();
            this.addr = addr;
            addrTextbox.Text = addr.ToString("X6");
        }

        private void AsmConfig_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int b = 0;
            int.TryParse(addrTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out b);
            this.addr = b;
            this.Close();
        }
    }
}
