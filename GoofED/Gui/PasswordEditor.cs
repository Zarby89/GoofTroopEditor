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
    public partial class PasswordEditor : Form
    {
        Game game;
        public PasswordEditor(Game game)
        {
            this.game = game;
            InitializeComponent();
            
        }

        private void PasswordEditor_Load(object sender, EventArgs e)
        {
            levelCombobox.SelectedIndex = 0;
        }

        private void passwordBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.Clear(Color.DarkGray);
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            GFX.ClearBGPassword();
            for (int i = 0; i < 5; i++)
            {
                if (game.passwords[levelCombobox.SelectedIndex][i] == 0)
                {
                    GFX.DrawFromVRAM(GFX.passwordBuffer, 1546, (byte)(2 + (i*24)), 0, true,0,0,10); //cherry
                }
                if (game.passwords[levelCombobox.SelectedIndex][i] == 1)
                {
                    GFX.DrawFromVRAM(GFX.passwordBuffer, 1548, (byte)(2 + (i * 24)), 0, true, 0, 0, 10); //banana
                }
                if (game.passwords[levelCombobox.SelectedIndex][i] == 2)
                {
                    GFX.DrawFromVRAM(GFX.passwordBuffer, 1550, (byte)(2 + (i * 24)), 0, true, 0, 0, 10); //red gem
                }
                if (game.passwords[levelCombobox.SelectedIndex][i] == 3)
                {
                    GFX.DrawFromVRAM(GFX.passwordBuffer, 1544, (byte)(2 + (i * 24)), 0, true, 0, 0, 10); //blue gem
                }

            }

            e.Graphics.DrawImage(GFX.passwordBitmap, new Rectangle(0, 0, 232, 32), 0, 0, 116, 16, GraphicsUnit.Pixel);

        }

        private void up1Button_Click_1(object sender, EventArgs e)
        {
            int id = 0;
            if (sender == up1Button)
            {
                id = 0;
            }
            else if (sender == up2Button)
            {
                id = 1;
            }
            else if (sender == up3Button)
            {
                id = 2;
            }
            else if (sender == up4Button)
            {
                id = 3;
            }
            else if (sender == up5Button)
            {
                id = 4;
            }

            if (game.passwords[levelCombobox.SelectedIndex][id] == 0)
            {
                game.passwords[levelCombobox.SelectedIndex][id] = 3;
            }
            else
            {
                game.passwords[levelCombobox.SelectedIndex][id] -= 1;
            }

            passwordBox.Refresh();

        }

        private void down1Button_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (sender == down1Button)
            {
                id = 0;
            }
            else if (sender == down2Button)
            {
                id = 1;
            }
            else if (sender == down3Button)
            {
                id = 2;
            }
            else if (sender == down4Button)
            {
                id = 3;
            }
            else if (sender == down5Button)
            {
                id = 4;
            }

            game.passwords[levelCombobox.SelectedIndex][id] += 1;
            if (game.passwords[levelCombobox.SelectedIndex][id] == 4)
            {
                game.passwords[levelCombobox.SelectedIndex][id] = 0;
            }

            passwordBox.Refresh();
        }

        private void levelCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            passwordBox.Refresh();
        }
    }
}
