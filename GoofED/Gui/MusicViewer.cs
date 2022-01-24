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
    public partial class MusicViewer : Form
    {
        Game game;
        public MusicViewer(Game game)
        {
            InitializeComponent();
            for (int i = 0; i < 8; i++)
            {
                spcNotes[i] = new List<SpcNote>();
            }
            this.game = game;
            comboBox1.Enabled = true;
            int nbrOfSongs = 0x22;
            for (int i = 0; i < nbrOfSongs; i++)
            {
                comboBox1.Items.Add("Song 0x" + i.ToString("X2"));
            }
            comboBox1.SelectedIndex = 0;


            

        }


        int selectedAddr = 0;
        int selectedAddrROM = 0;
        int selectedAddrROMSNES = 0;
        int selectedLength = 0;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int songPos = 0x848000;
            songPos += (game.rom.ReadByte((0x20000) + (comboBox1.SelectedIndex * 3) + 2) << 16);
            songPos += (game.rom.ReadByte((0x20000) + (comboBox1.SelectedIndex * 3) + 1) << 8);
            songPos += (game.rom.ReadByte((0x20000) + (comboBox1.SelectedIndex * 3) + 0));

            label1.Text = songPos.ToString("X6");

            label3.Text = "Length :" + game.rom.ReadShort(Utils.SnesToPc(songPos)).ToString("X4") + "\r\n" +
                          "Pos(ARAM) : " + game.rom.ReadShort(Utils.SnesToPc(songPos + 2)).ToString("X4") + "\r\n" +
                          "Pos(ROM) : " + songPos.ToString("X4");
            selectedLength = game.rom.ReadShort(Utils.SnesToPc(songPos));
            selectedAddr = game.rom.ReadShort(Utils.SnesToPc(songPos + 2));
            selectedAddrROM = Utils.SnesToPc(songPos + 4);
            selectedAddrROMSNES = songPos;
            vScrollBar1.Maximum = (selectedLength - 512) + (selectedLength / 16);
            vScrollBar1.Minimum = 0;
            if ((selectedLength - 512) < 0)
            {
                vScrollBar1.Maximum = 0;
                vScrollBar1.Value = 0;
            }

            for (int c = 0; c < 8; c++)
            {
                readChannel(game.rom.ReadShortI(selectedAddrROM + (c * 2)), c);
            }

            channel1Listbox.Items.Clear();
            for (int i = 0; i < spcNotes[0].Count; i++)
            {
                channel1Listbox.Items.Add(spcNotes[0][i].name);
            }

            listBox1.Items.Clear();
            for (int i = 0; i < spcNotes[1].Count; i++)
            {
                listBox1.Items.Add(spcNotes[1][i].name);
            }

            listBox2.Items.Clear();
            for (int i = 0; i < spcNotes[2].Count; i++)
            {
                listBox2.Items.Add(spcNotes[2][i].name);
            }

            listBox3.Items.Clear();
            for (int i = 0; i < spcNotes[3].Count; i++)
            {
                listBox3.Items.Add(spcNotes[3][i].name);
            }
            listBox4.Items.Clear();
            for (int i = 0; i < spcNotes[4].Count; i++)
            {
                listBox4.Items.Add(spcNotes[4][i].name);
            }

            listBox5.Items.Clear();
            for (int i = 0; i < spcNotes[5].Count; i++)
            {
                listBox5.Items.Add(spcNotes[5][i].name);
            }

            listBox6.Items.Clear();
            for (int i = 0; i < spcNotes[6].Count; i++)
            {
                listBox6.Items.Add(spcNotes[6][i].name);
            }

            listBox7.Items.Clear();
            for (int i = 0; i < spcNotes[7].Count; i++)
            {
                listBox7.Items.Add(spcNotes[7][i].name);
            }

            hexBox.Refresh();

        }

        private void hexBox_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.Clear(Color.White);
            Font sysFont = new Font("Courier New", 12);
            e.Graphics.FillRectangle(Brushes.White, new RectangleF(0, 0, 512, 516));
            e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(0, 0, 64, 516));



            if (vScrollBar1.Value == 0)
            {
                e.Graphics.FillRectangle(Brushes.DarkGray, new RectangleF(64, 0, 512 - 64, 16));
                e.Graphics.FillRectangle(Brushes.DarkGoldenrod, new RectangleF(64 + tabControl1.SelectedIndex * 56, 0, 56, 16));

            }
            for (int i = 0; i < (524 / 16); i++)
            {
                e.Graphics.DrawString((selectedAddr + ((i + vScrollBar1.Value) * 0x10)).ToString("X6"), sysFont, Brushes.Gray, 0, (i * 16));
            }

            if (game.rom.data != null)
            {
                for (int i = (vScrollBar1.Value * 16); i < (vScrollBar1.Value * 16) + 512; i++)
                {
                    if (i < selectedLength)
                    {
                        e.Graphics.DrawString(game.rom.data[selectedAddrROM + i].ToString("X2"), sysFont, Brushes.Black, 68 + ((i % 16) * 28), (((i / 16) - (vScrollBar1.Value)) * 16));
                        if (i == selectedHex)
                        {
                            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 2), new Rectangle(68 + ((i % 16) * 28), (((i / 16) - (vScrollBar1.Value)) * 16), 28, 16));
                        }
                    }
                }
            }


        }

        private void hexBox_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            hexBox.Refresh();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            hexBox.Refresh();
        }
        List<SpcNote>[] spcNotes = new List<SpcNote>[8];
        private void readChannel(int pos, int chan)
        {
            spcNotes[chan].Clear();
            //selectedAddrROM is also equal selectedAddr
            //so selectedAddrROM + (pos - selectedAddr)
            int iPos = (pos - selectedAddr);
            int romPos = selectedAddrROM + (pos - selectedAddr);
            while (true)
            {
                byte b = game.rom.ReadByte(romPos);
                if (capcSpc.ContainsKey(b))
                {
                    string s = "";
                    byte[] bs = new byte[capcSpc[b].length];
                    for (int i = 0; i < capcSpc[b].length; i++)
                    {
                        s += "[" + game.rom.ReadByte(romPos + i).ToString("X2") + "]";
                        bs[i] = game.rom.ReadByte(romPos + i);
                    }

                    //channel1Listbox.Items.Add(s +" "+ capcSpc[b].name);
                    spcNotes[chan].Add(new SpcNote(iPos, s + " " + capcSpc[b].name, bs));
                    romPos += capcSpc[b].length;
                    iPos += capcSpc[b].length;
                    if (b == 0x17)
                    {
                        break;
                    }
                }
                else
                {
                    spcNotes[chan].Add(new SpcNote(iPos, "[" + b.ToString("X2") + "] Note", new byte[1] { b }));
                    romPos++;
                    iPos++;
                }

            }




        }

        Dictionary<byte, SPCCommand> capcSpc = new Dictionary<byte, SPCCommand>
        {
            {0x00, new SPCCommand("Toggle Triplet", 1)},
            {0x01, new SPCCommand("Toggle Portamento",1)},
            {0x02, new SPCCommand("Set Dotted Note",1)},
            {0x03, new SPCCommand("Toggle 2 Octave up",1)},
            {0x04, new SPCCommand("Toggle T P O",2)},
            {0x05, new SPCCommand("Set Tempo",3)},
            {0x06, new SPCCommand("Set Duration",2)},
            {0x07, new SPCCommand("Set Volume",2)},
            {0x08, new SPCCommand("Set Instrument",2)},
            {0x09, new SPCCommand("Set Octave",2)},
            {0x0A, new SPCCommand("Global Transpose",2)},
            {0x0B, new SPCCommand("Channel Transpose",2)},
            {0x0C, new SPCCommand("Tuning",2)},
            {0x0D, new SPCCommand("Portamento Time",2)},
            {0x0E, new SPCCommand("Loop 00",4)},
            {0x0F, new SPCCommand("Loop 01",4)},
            {0x10, new SPCCommand("Loop 02",4)},
            {0x11, new SPCCommand("Loop 03",4)},
            {0x12, new SPCCommand("Loop Break 00",4)},
            {0x13, new SPCCommand("Loop Break 01",4)},
            {0x14, new SPCCommand("Loop Break 02",4)},
            {0x15, new SPCCommand("Loop Break 03",4)},
            {0x16, new SPCCommand("Jump",3)},
            {0x17, new SPCCommand("End Of Channel",1)},
            {0x18, new SPCCommand("Pan",1)},
            {0x19, new SPCCommand("Master Volume",2)},
            {0x1A, new SPCCommand("LFO",3)},
            {0x1B, new SPCCommand("Echo Parameter",3)},
            {0x1C, new SPCCommand("Echo On/Off",2)},
            {0x1D, new SPCCommand("Release Rate",2)},
            {0x1E, new SPCCommand("???",2)},
            {0x1F, new SPCCommand("???",2)}
        };

        private void button2_Click(object sender, EventArgs e)
        {


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("lorom");
            sb.AppendLine("function BigEndian(n) = (((n&$ff00)>>8)|((n&$00ff)<<8))");
            sb.AppendLine("org $" + selectedAddrROMSNES.ToString("X6"));
            sb.AppendLine("!ARAMAddr = $" + selectedAddr.ToString("X4"));

            sb.AppendLine("SongStart:");
            sb.AppendLine("dw SongStart-EndOfSong");
            sb.AppendLine("dw !ARAMAddr");

            sb.AppendLine("Channels:");
            sb.AppendLine("!ARAMC = !ARAMAddr-SongStart");

            for (int i = 0; i < 8; i++)
            {
                sb.AppendLine("dw BigEndian(" + "Channel" + i.ToString("D2") + "+!ARAMC" + ")");
            }
            sb.AppendLine(";Start of song data");

            for (int i = 0; i < 8; i++)
            {
                readChannel(game.rom.ReadShortI(selectedAddrROM + (i * 2)), i);
                sb.AppendLine("Channel" + i.ToString("D2") + ":");
                for (int n = 0; n < spcNotes[i].Count; n++)
                {
                    string s = "db ";
                    for (int b = 0; b < spcNotes[i][n].bytes.Length; b++)
                    {
                        if (b == spcNotes[i][n].bytes.Length - 1)
                        {
                            s += "$" + spcNotes[i][n].bytes[b].ToString("X2");
                        }
                        else
                        {
                            s += "$" + spcNotes[i][n].bytes[b].ToString("X2") + ", ";
                        }

                    }
                    sb.AppendLine(s + " ; " + spcNotes[i][n].name);

                }


            }

            sb.AppendLine("EndOfSong:");

            SaveFileDialog sf = new SaveFileDialog();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sf.FileName, sb.ToString());
            }
            

            channel1Listbox.Items.Clear();
            for (int i = 0; i < spcNotes[0].Count; i++)
            {
                channel1Listbox.Items.Add(spcNotes[0][i].name);
            }


            //rom.ReadShortI(selectedAddrROM + (i * 2)).ToString("X4")

        }
        int selectedHex = 0;
        private void channel1Listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                selectedHex = spcNotes[0][channel1Listbox.SelectedIndex].addr;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                selectedHex = spcNotes[1][listBox1.SelectedIndex].addr;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                selectedHex = spcNotes[2][listBox2.SelectedIndex].addr;
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                selectedHex = spcNotes[3][listBox3.SelectedIndex].addr;
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                selectedHex = spcNotes[4][listBox4.SelectedIndex].addr;
            }
            else if (tabControl1.SelectedIndex == 5)
            {
                selectedHex = spcNotes[5][listBox5.SelectedIndex].addr;
            }
            else if (tabControl1.SelectedIndex == 6)
            {
                selectedHex = spcNotes[6][listBox6.SelectedIndex].addr;
            }
            else if (tabControl1.SelectedIndex == 7)
            {
                selectedHex = spcNotes[7][listBox7.SelectedIndex].addr;
            }


            hexBox.Refresh();
        }

        private void MusicViewer_Load(object sender, EventArgs e)
        {

        }
    }
}
