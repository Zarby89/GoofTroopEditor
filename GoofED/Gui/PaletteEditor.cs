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
    public partial class PaletteEditor : Form
    {
        Game game;
        public PaletteEditor(Game game)
        {
            this.game = game;
            InitializeComponent();
        }
        Color[] displayColor;
        List<Color[]> palgroup0 = new List<Color[]>();
        List<Color[]> animgroup = new List<Color[]>();
        List<Color[]> sprgroup = new List<Color[]>();
        ColorDialog cd = new ColorDialog();
        private void PaletteEditor_Shown(object sender, EventArgs e)
        {

            //SRC is E400 + (SRC * 32)
            //DES SRC  Length
            //83F9C2-83FA0A moved palettes  01F9C2
            //838A0B <- rest of group00 unmoved  018A0B
            int bAddr = 0x056400;
            for (int i = 0; i< 24;i++) //24 first group from moved region
            {
                int src = game.rom.ReadByte(0x01FFB8 + (i * 3) + 1) *32;
                int length = (game.rom.ReadByte(0x01FFB8 + (i * 3) + 2)+1)/2;
                Color[] colors = new Color[length];
                for (int j = 0; j < length;j++)
                {
                    colors[j] = game.rom.ReadColor(bAddr + src + (j*2));
                }

                palgroup0.Add(colors);
            }

            for (int i = 0; i < 25; i++) //25 from vanilal region
            {

                int src = game.rom.ReadByte(0x018A0B + (i * 3) + 1) * 32;
                int length = (game.rom.ReadByte(0x018A0B + (i * 3) + 2) + 1) / 2;
                Color[] colors = new Color[length];
                for (int j = 0; j < length; j++)
                {
                    colors[j] = game.rom.ReadColor(bAddr + src + (j * 2));
                }

                palgroup0.Add(colors);
            }
            int k = 0;
            foreach(Color[] c in palgroup0)
            {
                treeView1.Nodes[0].Nodes.Add("Palette " + k.ToString("X2"));
                k++;
            }
            for (int i = 0; i < 19; i++) //animated
            {
                int ptrPc = Utils.SnesToPc(game.rom.ReadShort(0x187EC + (i * 2)) + 0x830000);
                byte nbrOfRow = game.rom.ReadByte(ptrPc);
                
                Color[] colors = new Color[nbrOfRow * 16];
                for (int n = 0; n < nbrOfRow; n++)
                {
                    int src = game.rom.ReadByte(ptrPc + 3 + (n * 2)) * 32;
                    for (int j = 0; j < 16; j++)
                    {
                        colors[j + (n*16)] = game.rom.ReadColor(bAddr + src + (j * 2));
                    }
                }
                animgroup.Add(colors);
            }
            k = 0;
            foreach (Color[] c in animgroup)
            {
                treeView1.Nodes[1].Nodes.Add("Palette " + k.ToString("X2"));
                k++;
            }



            for (int i = 0; i < 0x40; i++) //sprites
            {

                Color[] colors = new Color[16];
                for (int j = 0; j < 16; j++)
                {
                    colors[j] = game.rom.ReadColor(Constants.staticSprPalette + (i * 0x20) + (j * 2));
                }
                sprgroup.Add(colors);
            }
            k = 0;
            foreach (Color[] c in sprgroup)
            {
                treeView1.Nodes[2].Nodes.Add("Palette " + k.ToString("X2"));
                k++;
            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Parent == treeView1.Nodes[0]) //palgroup00
            {
                displayColor = palgroup0[treeView1.SelectedNode.Index];
                pictureBox1.Refresh();
            }
            else if (treeView1.SelectedNode.Parent == treeView1.Nodes[1]) //animgroup
            {
                displayColor = animgroup[treeView1.SelectedNode.Index];
                pictureBox1.Refresh();
            }
            else if (treeView1.SelectedNode.Parent == treeView1.Nodes[2]) //animgroup
            {
                displayColor = sprgroup[treeView1.SelectedNode.Index];
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (displayColor != null)
            {
                for(int i = 0;i < displayColor.Length;i++)
                {
                    e.Graphics.FillRectangle(new SolidBrush(displayColor[i]), new Rectangle((i % 16)*16,(i/16)*16,16,16));
                }
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int tx = e.X / 16;
            int ty = e.Y / 16;

            
            
            cd.Color = displayColor[tx + (ty*16)];
            if (cd.ShowDialog() == DialogResult.OK)
            {
                displayColor[tx + (ty * 16)] = cd.Color;
            }
            pictureBox1.Refresh();



        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            int bAddr = 0x056400;
            for (int i = 0; i < 24; i++) //24 first group from moved region
            {
                int src = game.rom.ReadByte(0x01FFB8 + (i * 3) + 1) * 32;
                int length = (game.rom.ReadByte(0x01FFB8 + (i * 3) + 2) + 1) / 2;
                for (int j = 0; j < length; j++)
                {
                    game.rom.WriteColor(bAddr + src + (j * 2), palgroup0[i][j]);
                }
            }

            for (int i = 0; i < 25; i++) //25 from vanilal region
            {


                int src = game.rom.ReadByte(0x018A0B + (i * 3) + 1) * 32;
                int length = (game.rom.ReadByte(0x018A0B + (i * 3) + 2) + 1) / 2;
                for (int j = 0; j < length; j++)
                {
                    game.rom.WriteColor(bAddr + src + (j * 2), palgroup0[i+24][j]);
                }
            }

            for (int i = 0; i < 19; i++) //animated
            {
                int ptrPc = Utils.SnesToPc(game.rom.ReadShort(0x187EC + (i * 2)) + 0x830000);
                byte nbrOfRow = game.rom.ReadByte(ptrPc);

                Color[] colors = new Color[nbrOfRow * 16];
                for (int n = 0; n < nbrOfRow; n++)
                {
                    int src = game.rom.ReadByte(ptrPc + 3 + (n * 2)) * 32;
                    for (int j = 0; j < 16; j++)
                    {
                        game.rom.WriteColor(bAddr + src + (j * 2), animgroup[i][j+(n*16)]);
                    }
                }
            }

            for (int i = 0; i < 0x40; i++) //sprites
            {


                Color[] colors = new Color[16];
                for (int j = 0; j < 16; j++)
                {
                    game.rom.WriteColor((Constants.staticSprPalette + (i * 0x20) + (j * 2)), sprgroup[i][j]);
                }


            }
        }

        private void PaletteEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
