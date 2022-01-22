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
    public partial class AsmHelp : Form
    {
        public AsmHelp()
        {
            InitializeComponent();
        }

        private void AsmHelp_Load(object sender, EventArgs e)
        {
            Font nfont = asmhelpRichtextbox.Font;
            Font f = new Font(asmhelpRichtextbox.Font.FontFamily, 18);

            asmhelpRichtextbox.SelectionFont = f;
            asmhelpRichtextbox.AppendText("Plugin File Format\r\n");
            asmhelpRichtextbox.SelectionFont = nfont;
            asmhelpRichtextbox.AppendText("the plugin file format is a normal .asm file compatible with asar\r\n");
            asmhelpRichtextbox.AppendText("some minors differences you must define the HOOKS start and the CODES start\r\n");
            asmhelpRichtextbox.AppendText("\r\n");
            asmhelpRichtextbox.AppendText("we define the hooks and the code start with the following comments : \r\n");
            
            asmhelpRichtextbox.SelectionColor = Color.Green;
            asmhelpRichtextbox.AppendText(";HOOKS\r\n");
            asmhelpRichtextbox.SelectionColor = Color.Green;
            asmhelpRichtextbox.AppendText(";CODES\r\n");
            asmhelpRichtextbox.SelectionColor = Color.Black;
            asmhelpRichtextbox.AppendText("everything below the CODES section MUST NOT have any org");

        }
    }
}
