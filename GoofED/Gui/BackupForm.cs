using GoofTroopEditor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofTroopEditor.Gui
{
    public partial class BackupForm : Form
    {
        public BackupForm()
        {
            InitializeComponent();
        }

        private void BackupForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = (string)Settings.Default["backupPath"];
            enableCheckbox.Checked = (bool)Settings.Default["backupEnable"];
            timerUpDown.Value = (int)Settings.Default["backupTimer"];
            nbrbackupUpDown.Value = (int)Settings.Default["backupCount"];
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Settings.Default["backupPath"] = textBox1.Text;
            Settings.Default["backupEnable"] = enableCheckbox.Checked;
            Settings.Default["backupTimer"] = (int)timerUpDown.Value;
            Settings.Default["backupCount"] = (int)nbrbackupUpDown.Value;
            Settings.Default.Save();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
            }
            
        }
    }
}
