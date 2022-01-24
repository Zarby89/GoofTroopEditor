using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class BGMode
    {
        Game game;
        bool ismouseDown = false;
        int lastTilemouseX = 0;
        int lastTilemouseY = 0;
        public ushort[] selectedTiles = new ushort[1] { 0 };
        public byte selectedWidth = 0;
        public byte selectedHeight = 0;
        int tileSelectDownX = 0;
        int tileSelectDownY = 0;
        bool selecting = false;
        
        ushort[] savedTiles;
        public BGMode(Game game)
        {
            this.game = game;
        }

        public void mouseDown(MouseEventArgs e, int bg)
        {



            if (bg == 0)
            {
                if (!GlobalOptions.viewBg1)
                {
                    MessageBox.Show("BG1 is not visible !");
                    return;
                }
            }
            else
            {
                if (!GlobalOptions.viewBg2)
                {
                    MessageBox.Show("BG2 is not visible !");
                    return;
                }
            }
            int tx = (e.X / 32);
            int ty = (e.Y / 32);

            if (e.Button == MouseButtons.Right)
            {
                tileSelectDownX = tx;
                tileSelectDownY = ty;
                selectedWidth = 1;
                selectedHeight = 1;
                if (bg == 0)
                {
                    selectedTiles = new ushort[1] { game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16[tx + ((ty) * 16)] };
                }
                else
                {
                    selectedTiles = new ushort[1] { game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16[tx + ((ty) * 16)] };
                }
                
                selecting = true;

                return;
            }


            if (game.levels[game.selectedLevel].maps[game.selectedMap].undoPos != (game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Count)) //we used undo so pop off all the values higher than the undo
            {
                game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.RemoveRange(game.levels[game.selectedLevel].maps[game.selectedMap].undoPos, game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Count - game.levels[game.selectedLevel].maps[game.selectedMap].undoPos);
                game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles2.RemoveRange(game.levels[game.selectedLevel].maps[game.selectedMap].undoPos, game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Count - game.levels[game.selectedLevel].maps[game.selectedMap].undoPos);
            }


            game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Add((ushort[])game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16.Clone());
            game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles2.Add((ushort[])game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16.Clone());

            if (game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Count >= 16)
                {
                game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.RemoveAt(0);
                game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles2.RemoveAt(0);
            }
            game.levels[game.selectedLevel].maps[game.selectedMap].undoPos = (game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Count);



            
            for (int h = 0; h < selectedHeight; h++)
            {
                for (int w = 0; w < selectedWidth; w++)
                {
                    if (tx + w + ((ty + h) * 16) < 256)
                    {
                        if (bg == 0)
                        {
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16[tx + w + ((ty + h) * 16)] = selectedTiles[w + (h * selectedWidth)];
                            Tile16 t = game.tiles16[selectedTiles[w + (h * selectedWidth)]];
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((tx + w) * 2) + (((ty + h) * 4) * 16)] = t.tile0.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((tx + w) * 2) + 1 + (((ty + h) * 4) * 16)] = t.tile1.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((tx + w) * 2) + ((((ty + h) * 4) + 2) * 16)] = t.tile2.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((tx + w) * 2) + 1 + ((((ty + h) * 4) + 2) * 16)] = t.tile3.toShort();


                        }
                        else if (bg == 1)
                        {

                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16[tx + w + ((ty + h) * 16)] = selectedTiles[w + (h * selectedWidth)];
                            Tile16 t = game.tiles16[selectedTiles[w + (h * selectedWidth)]];
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((tx + w) * 2) + (((ty + h) * 4) * 16)] = t.tile0.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((tx + w) * 2) + 1 + (((ty + h) * 4) * 16)] = t.tile1.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((tx + w) * 2) + ((((ty + h) * 4) + 2) * 16)] = t.tile2.toShort();
                            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((tx + w) * 2) + 1 + ((((ty + h) * 4) + 2) * 16)] = t.tile3.toShort();

                        }
                    }
                }
            }
            GFX.DrawBG1();
            GFX.DrawBG2();

            ismouseDown = true;
        }

        public void mouseMove(MouseEventArgs e, int bg, Graphics g)
        {
            int tx = (e.X / 32);
            int ty = (e.Y / 32);
            if (tx < 0) { tx = 0; }
            if (ty < 0) { ty = 0; }
            if (tx > 15) { tx = 15; }
            if (ty > 15) { ty = 15; }

            if (selecting == true)
            {
                if (e.Button == MouseButtons.Right)
                {
                    selectedWidth = (byte)((tx - tileSelectDownX)+1);
                    selectedHeight = (byte)((ty - tileSelectDownY)+1);
                }
                if (selectedHeight >= 48)
                {
                    selectedHeight = 1;
                }
                if (selectedWidth >= 48)
                {
                    selectedWidth = 1;
                }

                selectedTiles = new ushort[selectedWidth * selectedHeight];

                for (int h = 0; h < selectedHeight; h++)
                {
                    for (int w = 0; w < selectedWidth; w++)
                    {
                        if (bg == 0)
                        {
                            selectedTiles[w + (h*selectedWidth)] = game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16[(tileSelectDownX + w) + ((tileSelectDownY + h) * 16)];
                        }
                        else
                        {
                            selectedTiles[w + (h * selectedWidth)] = game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16[(tileSelectDownX + w) + ((tileSelectDownY + h) * 16)];
                        }
                    }
                }

            }

            if (ismouseDown == true)
            {

                if (lastTilemouseX != tx || lastTilemouseY != ty)
                {
                    for (int h = 0; h < selectedHeight; h++)
                    {
                        for (int w = 0; w < selectedWidth; w++)
                        {
                            if (tx + w + ((ty + h) * 16) < 256)
                            {
                                if (bg == 0)
                                {
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16[tx + w + ((ty + h) * 16)] = selectedTiles[w + (h * selectedWidth)];
                                    Tile16 t = game.tiles16[selectedTiles[w + (h * selectedWidth)]];
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((tx + w) * 2) + (((ty + h) * 4) * 16)] = t.tile0.toShort();
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((tx + w) * 2) + 1 + (((ty + h) * 4) * 16)] = t.tile1.toShort();
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((tx + w) * 2) + ((((ty + h) * 4) + 2) * 16)] = t.tile2.toShort();
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((tx + w) * 2) + 1 + ((((ty + h) * 4) + 2) * 16)] = t.tile3.toShort();


                                }
                                else if (bg == 1)
                                {

                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16[tx + w + ((ty + h) * 16)] = selectedTiles[w + (h * selectedWidth)];
                                    Tile16 t = game.tiles16[selectedTiles[w + (h * selectedWidth)]];
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((tx + w) * 2) + (((ty + h) * 4) * 16)] = t.tile0.toShort();
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((tx + w) * 2) + 1 + (((ty + h) * 4) * 16)] = t.tile1.toShort();
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((tx + w) * 2) + ((((ty + h) * 4) + 2) * 16)] = t.tile2.toShort();
                                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((tx + w) * 2) + 1 + ((((ty + h) * 4) + 2) * 16)] = t.tile3.toShort();

                                }
                            }
                        }
                    }

                }
                GFX.DrawBG1();
                GFX.DrawBG2();

            }
            lastTilemouseX = tx;
            lastTilemouseY = ty;
            

        }

        public void mouseUp(MouseEventArgs e, int bg)
        {
            ismouseDown = false;




            selecting = false;
        }

        public void Undo()
        {


            game.levels[game.selectedLevel].maps[game.selectedMap].undoPos -= 1;
            if (game.levels[game.selectedLevel].maps[game.selectedMap].undoPos < 0)
            {
                game.levels[game.selectedLevel].maps[game.selectedMap].undoPos = 0;
                return;
            }
            if (game.levels[game.selectedLevel].maps[game.selectedMap].undoPos == game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Count-1)
            {
                game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Add((ushort[])game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16.Clone());
                game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles2.Add((ushort[])game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16.Clone());
            }


            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16 = game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles[game.levels[game.selectedLevel].maps[game.selectedMap].undoPos];
            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16 = game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles2[game.levels[game.selectedLevel].maps[game.selectedMap].undoPos];

            redrawTiles8();
        }

        public void Redo()
        {

            game.levels[game.selectedLevel].maps[game.selectedMap].undoPos += 1;
           
            if (game.levels[game.selectedLevel].maps[game.selectedMap].undoPos >= game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Count)
            {
                game.levels[game.selectedLevel].maps[game.selectedMap].undoPos = game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles.Count-1;
                return;
            }
            game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16 = game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles[game.levels[game.selectedLevel].maps[game.selectedMap].undoPos];
            game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16 = game.levels[game.selectedLevel].maps[game.selectedMap].undoTiles2[game.levels[game.selectedLevel].maps[game.selectedMap].undoPos];

            redrawTiles8();
        }

        public void redrawTiles8()
        {
            for (int h = 0; h < 14; h++)
            {
                for (int w = 0; w < 16; w++)
                {

                    Tile16 t = game.tiles16[game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap16[w + (h*16)]];
                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((w) * 2) + ((( h) * 4) * 16)] = t.tile0.toShort();
                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((w) * 2) + 1 + (((h) * 4) * 16)] = t.tile1.toShort();
                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((w) * 2) + ((((h) * 4) + 2) * 16)] = t.tile2.toShort();
                    game.levels[game.selectedLevel].maps[game.selectedMap].bg1tilemap8[((w) * 2) + 1 + ((((h) * 4) + 2) * 16)] = t.tile3.toShort();


                    Tile16 t2 = game.tiles16[game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap16[w + (h * 16)]];
                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((w) * 2) + (((h) * 4) * 16)] = t2.tile0.toShort();
                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((w) * 2) + 1 + (((h) * 4) * 16)] = t2.tile1.toShort();
                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((w) * 2) + ((((h) * 4) + 2) * 16)] = t2.tile2.toShort();
                    game.levels[game.selectedLevel].maps[game.selectedMap].bg2tilemap8[((w) * 2) + 1 + ((((h) * 4) + 2) * 16)] = t2.tile3.toShort();
                }
            }
            GFX.DrawBG1();
            GFX.DrawBG2();
        }

        public void Copy()
        {
            savedTiles = (ushort[])selectedTiles.Clone();
            Clipboard.SetData("GTTiles16", new GTTiles16Clipboard(savedTiles, selectedWidth, selectedHeight));

        }

        public void Paste()
        {
            GTTiles16Clipboard o = (GTTiles16Clipboard)Clipboard.GetData("GTTiles16");
            if (o != null)
            {
                selectedTiles = (ushort[])o.tiles.Clone();
                selectedWidth = o.width;
                selectedHeight = o.height;
            }

        }

        public void Draw(Graphics g)
        {
            g.Clear(Color.Transparent);
            
            if (selectedWidth == 0 && selectedHeight == 0)
            {
                g.DrawRectangle(Pens.White, new Rectangle(lastTilemouseX * 32, lastTilemouseY * 32, 32, 32));
            }
            else
            {
                if (selecting)
                {
                    g.DrawRectangle(Pens.White, new Rectangle(tileSelectDownX * 32, tileSelectDownY * 32, selectedWidth * 32, selectedHeight * 32));
                }
                else
                {
                    g.DrawRectangle(Pens.White, new Rectangle(lastTilemouseX * 32, lastTilemouseY * 32, selectedWidth * 32, selectedHeight * 32));
                }

            }

        }

    }
}
