using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public partial class AsmManagerNew : Form
    {

        string fn;
        bool fromForm = false;
        string configfn;
        int addr = 0x908000;
        int nbrPlugins = 0;
        List<ASMPlugin> plugins = new List<ASMPlugin>();

        //config file definition
        //int Code Address used
        //int Number of plugins in the file
        //int UNUSED
        //int UNUSED
        //int UNUSED

        //for each plugins
        //{
        //string PluginName
        //string PluginCode
        //}

        public AsmManagerNew(string fn)
        {

            InitializeComponent();

            this.fn = fn;
            configfn = fn.Substring(0, fn.Length - 3) + "gte";
            if (File.Exists(configfn))
            {
                BinaryReader br = new BinaryReader(new FileStream(configfn, FileMode.Open, FileAccess.Read));
                addr = br.ReadInt32();
                nbrPlugins = br.ReadInt32();
                br.ReadInt32(); //UNUSED
                br.ReadInt32(); //UNUSED
                br.ReadInt32(); //UNUSED

                for (int i = 0; i < nbrPlugins; i++)
                {
                    string s = br.ReadString();
                    plugins.Add(new ASMPlugin(s, br.ReadString()));
                    pluginListbox.Items.Add(s);

                }
                br.Close();
            }
        }

        private void AsmManagerNew_Load(object sender, EventArgs e)
        {

        }

        public void buildASM()
        {
            StringBuilder sbHooks = new StringBuilder();
            StringBuilder sbCodes = new StringBuilder();
            sbHooks.AppendLine("lorom"); //set rom to lorom
            sbHooks.AppendLine("");
            sbHooks.AppendLine("");
            sbHooks.AppendLine("; Generated Hooks"); //space
            sbHooks.AppendLine("");

            foreach (ASMPlugin plugin in plugins)
            {
                int hookPos = plugin.content.IndexOf(";HOOKS");
                int codePos = plugin.content.IndexOf(";CODES");

                if (hookPos == -1 && codePos == -1) //no hook nor code
                { 
                    sbHooks.Append(plugin.content); //copy the whole content
                    sbHooks.AppendLine("");
                    continue;
                }

                if (hookPos != -1 && codePos != -1) //if both are not -1
                {
                    sbHooks.Append(plugin.content.Substring(hookPos,codePos-hookPos)); //copy the whole content
                    sbHooks.AppendLine("");
                    sbCodes.Append(plugin.content.Substring(codePos)); //copy the end section in code
                    sbCodes.AppendLine(""); //copy the end section in code
                    continue;
                }

                if (hookPos != -1) 
                {
                    sbHooks.Append(plugin.content.Substring(hookPos)); //copy the whole content
                    sbHooks.AppendLine("");

                    continue;
                }

                if (codePos != -1)
                {
                    sbCodes.Append(plugin.content.Substring(codePos)); //copy the whole content
                    sbCodes.AppendLine("");

                    continue;
                }

            }
            sbHooks.AppendLine("org $" + addr.ToString("X6"));
            sbHooks.Append(sbCodes.ToString());
            File.WriteAllText("ASM//temp.asm", sbHooks.ToString());
        }

        private void pluginFileFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AsmHelp asmhelp = new AsmHelp();
            asmhelp.ShowDialog();
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AsmConfig config = new AsmConfig(addr);
            if (config.ShowDialog() == DialogResult.OK)
            {
                this.addr = config.addr;
            }

        }

        private void addexistingButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            if (of.ShowDialog() == DialogResult.OK)
            {
                string s = Path.GetFileName(of.FileName);
                plugins.Add(new ASMPlugin(s.Substring(0,s.Length-4), File.ReadAllText(of.FileName)));
                updateListBox();
            }
        }

        private void addnewButton_Click(object sender, EventArgs e)
        {
            GetPluginName gpn = new GetPluginName();
            if (gpn.ShowDialog() == DialogResult.OK)
            {
                plugins.Add(new ASMPlugin(gpn.nameText, ";HOOKS\r\n\r\n;CODES\r\n\r\n"));
                updateListBox();
            }
            
        }

        private void removeselectedButton_Click(object sender, EventArgs e)
        {
            plugins.RemoveAt(pluginListbox.SelectedIndex);
            updateListBox();
        }

        private void updateListBox()
        {
            pluginListbox.Items.Clear();
            for(int i =0;i<plugins.Count;i++)
            {
                pluginListbox.Items.Add(plugins[i].name);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(configfn, FileMode.OpenOrCreate, FileAccess.Write));
            bw.Write(addr);
            bw.Write((int)plugins.Count);
            bw.Write((int)0);
            bw.Write((int)0);
            bw.Write((int)0);


            for (int i = 0; i < plugins.Count; i++)
            {
                bw.Write(plugins[i].name);
                bw.Write(plugins[i].content);
            }
        }

        private void codeTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!fromForm)
            {
                plugins[pluginListbox.SelectedIndex].content = codeTextbox.Text;
            }
        }

        private void pluginListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pluginListbox.SelectedIndex != -1)
            {
                fromForm = true;
                codeTextbox.Text = plugins[pluginListbox.SelectedIndex].content;
                fromForm = false;
            }
        }
    }
}
