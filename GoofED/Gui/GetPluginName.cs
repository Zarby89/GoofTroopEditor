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
    public partial class GetPluginName : Form
    {
        public GetPluginName()
        {
            InitializeComponent();
        }
        public string nameText = "New Plugin";
        private void GetPluginName_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            nameText = nameTextbox.Text;
            this.Close();
        }
    }
}
