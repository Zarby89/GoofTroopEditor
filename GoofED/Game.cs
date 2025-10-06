using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public class Game //This is the main class of everything
    {
        public List<Tile16> tiles16 = new List<Tile16>();
        public List<byte> collisions = new List<byte>();
        public List<Tile32> tiles32 = new List<Tile32>();
        public List<Tile32> tiles32_ex = new List<Tile32>();
        public ROM rom = new ROM();
        public Level[] levels = new Level[5];
        public byte selectedLevel = 0;
        public byte selectedMap = 0;
        public byte[][] texts = new byte[50][];
        public byte[][] passwords = new byte[4][];
        public bool anyChange = false;
        int extraSpaceitemsprite = 0x07374;
        
        List<Item>[][] dooritemstoremove = new List<Item>[5][];
        public List<CreditLine> creditLines = new List<CreditLine>();
        public Game(string fn)
        {

            FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
            byte[] temp = new byte[fs.Length];
            fs.Read(temp, 0, (int)fs.Length);
            int header = 0;

            if ((fs.Length & 0x200) == 0x200)
            {
                MessageBox.Show("The ROM you opened is headered, the header will be removed on save");
                header = 0x200;
            }
            Array.Copy(temp, header, rom.data, 0, fs.Length - header);

            fs.Close();


            LoadTiles32();
            LoadTiles16();
            LoadTexts();
            LoadPasswords();
            LoadCredits();
            levels[0] = new Level(this, 0, 16, 0);
            levels[1] = new Level(this, 1, 16, 16);
            levels[2] = new Level(this, 2, 26, 32);
            levels[3] = new Level(this, 3, 30, 58);
            levels[4] = new Level(this, 4, 26, 88);


            byte[] palettesData = new byte[72]
            {
                0x20, 0x0C, 0x1F, 0x20, 0x0D, 0x1F, 0x20, 0x0E, 0x1F, 0x20, 0x0F, 0x1F, 0x00, 0x00, 0x1F, 0x20,
                0xA9, 0x7F, 0x20, 0x04, 0x9F, 0xE0, 0x1A, 0x1F, 0xE0, 0x03, 0x1F, 0xE0, 0x02, 0x1F, 0xC0, 0x1C,
                0x1F, 0xC0, 0x1D, 0x1F, 0xC0, 0x1E, 0x1F, 0x40, 0x91, 0xBF, 0x40, 0x97, 0xBF, 0x40, 0x9D, 0xBF,
                0x40, 0xA3, 0xBF, 0xC0, 0x4E, 0x1F, 0x20, 0xC0, 0x9F, 0x80, 0xC5, 0x1F, 0xA0, 0xC5, 0x1F, 0x20,
                0x90, 0x1F, 0xE0, 0xCE, 0x1F, 0x00, 0x7E, 0x3F
            };


            for (int i = 0; i < 12; i++)
            {
                rom.WriteByte(0x01FFB8 + i, palettesData[i]);
            }

            rom.WriteUShort(Constants.paletteCapcomLogoPtr, 0xFFB8); //move it to the end of bank83

            for (int i = 0; i < 3; i++)
            {
                rom.WriteByte((0x01FFB8 + 12) + i, palettesData[i + 12]);
            }
            rom.WriteUShort(Constants.paletteSplashScreen, (0xFFB8 + 12)); //move it to the end of bank83

            for (int i = 0; i < 57; i++)
            {

                rom.WriteByte((0x01FFB8 + 15) + i, palettesData[i + 15]);
            }
            rom.WriteUShort(Constants.paletteTitleScreen, (0xFFB8 + 15)); //move it to the end of bank83
            rom.WriteUShort(Constants.paletteTitleScreen2, (0xFFB8 + 15)); //move it to the end of bank83
            //hardcoded thing for the hookshot animation
            rom.WriteByte(0x018A07, 0x01);
            rom.WriteByte(0x018A08, 0x08);
            rom.WriteByte(0x018A09, 0x29);
            rom.WriteByte(0x018A0A, 0x2A);
            //8A07 pointer for that thing above
            rom.WriteByte(0x02CB0, 0x07);
            rom.WriteByte(0x02CB2, 0x8A);

            if (rom.ReadShort(0x01421D) == 0xB3B1)
            {
                rom.WriteUShort(0x01421D, 0xB2B0);
            }
            if (rom.ReadShort(0x014229) == 0xB3B1)
            {
                rom.WriteUShort(0x014229, 0xB2B0);
            }

        }

        public void LoadCredits()
        {
            int creditPos = 0x5F99E; //credit start position
            byte line = 0;
            byte xs = 0;
            byte count = 0;
            byte pal = 0;
            byte[] data;
            while(creditPos < 0x5FE00)
            {
                line = rom.ReadByte(creditPos);
                creditPos++;
                xs = rom.ReadByte(creditPos);
                creditPos++;
                count = rom.ReadByte(creditPos);
                creditPos++;
                pal = rom.ReadByte(creditPos);
                creditPos++;
                data = new byte[count];
                if (count != 0x80)
                {
                    for (int i = 0; i < count; i++)
                    {
                        data[i] = rom.ReadByte(creditPos);
                        creditPos++;
                    }
                    creditLines.Add(new CreditLine(line, xs, count, pal, data));
                }



                if (line == 0xFF) //this is the last one
                {
                    break;
                }
            }
        }
        public void LoadPasswords()
        {
            for (int i = 0; i < 4; i++)
            {
                passwords[i] = new byte[5];
                for (int j = 0; j < 5; j++)
                {
                    passwords[i][j] = rom.ReadByte(Constants.PasswordData + (i * 5) + j);
                }
            }

        }

        public void LoadTexts()
        {
            List<int> textsPtr = new List<int>();


            for (int i = 0; i < 50; i++)
            {

                List<byte> textbytes = new List<byte>();
                int ptrMessage = Utils.SnesToPc(rom.ReadShort((Constants.Messages_Pointer + (i * 2))) + 0x8B0000);
                int length = 0;
                if (textsPtr.Contains(ptrMessage))
                {
                    textbytes.Add(0x00);
                    texts[i] = textbytes.ToArray();
                    continue;
                }

                while (rom.ReadByte(ptrMessage + length) != 0)
                {
                    byte b = rom.ReadByte(ptrMessage + length);
                    textbytes.Add(b);
                    length++;
                }
                textbytes.Add(0x00);
                texts[i] = textbytes.ToArray();
                textsPtr.Add(ptrMessage);

            }
        }

        public void LoadTiles32()
        {
            int tpos = Constants.Tiles32Data;
            int extpos = Constants.Tiles32Ext;

            for (int i = 0; i < 4096; i += 1)
            {
                ushort t0 = rom.data[tpos];
                ushort t1 = rom.data[tpos + 1];
                ushort t2 = rom.data[tpos + 2];
                ushort t3 = rom.data[tpos + 3];

                if ((rom.data[extpos] & 0x0F) != 0)
                {
                    t0 += (ushort)((rom.data[extpos] & 0x0F) << 8);
                }
                if ((rom.data[extpos] & 0xF0) != 0)
                {
                    t1 += (ushort)((rom.data[extpos] & 0xF0) << 4);
                }
                if ((rom.data[extpos + 1] & 0x0F) != 0)
                {
                    t2 += (ushort)((rom.data[extpos + 1] & 0x0F) << 8);
                }
                if ((rom.data[extpos + 1] & 0xF0) != 0)
                {
                    t3 += (ushort)((rom.data[extpos + 1] & 0xF0) << 4);
                }

                tiles32.Add(new Tile32(t0, t1, t2, t3));


                extpos += 2;
                tpos += 4;
            }
            if (rom.ReadByte(0x00370B) == 0x5C)
            {
                tpos = Constants.Tiles32Data2;
                extpos = Constants.Tiles32Ext2;
            }
            else
            {
                //if rom has never been saved yet just resave all vanilla tiles
                tpos = Constants.Tiles32Data;
                extpos = Constants.Tiles32Ext;
            }


            for (int i = 0; i < 4096; i += 1)
            {
                ushort t0 = rom.data[tpos];
                ushort t1 = rom.data[tpos + 1];
                ushort t2 = rom.data[tpos + 2];
                ushort t3 = rom.data[tpos + 3];

                if ((rom.data[extpos] & 0x0F) != 0)
                {
                    t0 += (ushort)((rom.data[extpos] & 0x0F) << 8);
                }
                if ((rom.data[extpos] & 0xF0) != 0)
                {
                    t1 += (ushort)((rom.data[extpos] & 0xF0) << 4);
                }
                if ((rom.data[extpos + 1] & 0x0F) != 0)
                {
                    t2 += (ushort)((rom.data[extpos + 1] & 0x0F) << 8);
                }
                if ((rom.data[extpos + 1] & 0xF0) != 0)
                {
                    t3 += (ushort)((rom.data[extpos + 1] & 0xF0) << 4);
                }

                tiles32_ex.Add(new Tile32(t0, t1, t2, t3));


                extpos += 2;
                tpos += 4;
            }
        }

        public void LoadTiles16()
        {
            if (rom.data[Constants.EditorVersion] == 0x6C)
            {
                int ttpos = Constants.Tiles16Data;

                for (int i = 0; i < 0xF00; i += 1)
                {
                    GFX.scratchpadTiles[i] = 0;
                    ushort t0i = rom.ReadShort(ttpos);
                    ushort t1i = rom.ReadShort(ttpos + 2);
                    ushort t2i = rom.ReadShort(ttpos + 4);
                    ushort t3i = rom.ReadShort(ttpos + 6);
                    ttpos += 8;
                    if (i < 0xC48)
                    {
                        tiles16.Add(new Tile16(gettilesinfo(t0i), gettilesinfo(t1i), gettilesinfo(t2i), gettilesinfo(t3i)));
                        collisions.Add(rom.ReadByte(Constants.Tile16CollisionMap + i));
                    }
                    else
                    {
                        tiles16.Add(new Tile16(new TileInfo(0,0,0,0,0), new TileInfo(0, 0, 0, 0, 0), new TileInfo(0, 0, 0, 0, 0), new TileInfo(0, 0, 0, 0, 0)));
                        collisions.Add(0);
                    }
                }
            }
            else // we already modified the rom with this version
            {
                int ttpos = Constants.Tiles16DataExt;

                for (int i = 0; i < 0xF00; i += 1)
                {
                    GFX.scratchpadTiles[i] = 0;
                    ushort t0i = rom.ReadShort(ttpos);
                    ushort t1i = rom.ReadShort(ttpos + 2);
                    ushort t2i = rom.ReadShort(ttpos + 4);
                    ushort t3i = rom.ReadShort(ttpos + 6);
                    ttpos += 8;

                    tiles16.Add(new Tile16(gettilesinfo(t0i), gettilesinfo(t1i), gettilesinfo(t2i), gettilesinfo(t3i)));
                    collisions.Add(rom.ReadByte(Constants.Tile16CollisionMap + i));
                }
            }

        }

        public static TileInfo gettilesinfo(ushort tile)
        {
            //vhopppcc cccccccc
            ushort o = 0;
            ushort v = 0;
            ushort h = 0;
            ushort tid = (ushort)(tile & 0x3FF);
            byte p = (byte)((tile >> 10) & 0x07);

            o = (ushort)((tile & 0x2000) >> 13);
            h = (ushort)((tile & 0x4000) >> 14);
            v = (ushort)((tile & 0x8000) >> 15);
            return new TileInfo(tid, p, v, h, o);

        }

        /// <summary>
        /// Return true if rom needs to be restored
        /// </summary>
        /// <returns></returns>
        public bool SaveAll()
        {
            extraSpaceitemsprite = 0x07374;
            //Doors must be saved before items
            for (int l = 0; l < 5; l++)
            {
                dooritemstoremove[l] = new List<Item>[levels[l].maps.Count];
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    dooritemstoremove[l][m] = new List<Item>();
                }
            }
            if (!SaveHookshot())
            {
                return true;
            }


            if (!SaveIceDark())
            {
                return true;
            }
            if (!SaveMapsTiles())
            {
                return true;
            }
            if (!SaveMapsProperties())
            {
                return true;
            }
            if (!SaveBlockDoors())
            {
                return true;
            }
            if (!SaveEnemiesDoors())
            {
                return true;
            }
            if (!SaveItems())
            {
                return true;
            }
            if (!SaveLockedDoors())
            {
                return true;
            }
            if (!SaveSprites())
            {
                return true;
            }
            if (!SaveLiftObjects())
            {
                return true;
            }
            if (!SaveLevelProperties())
            {
                return true;
            }
            if (!SavePasswords())
            {
                return true;
            }
            if (!SaveTexts())
            {
                return true;
            }
            if (!SaveTransitions())
            {
                return true;
            }
            if (!SaveCredits())
            {
                return true;
            }
            if (!SavePlanks())
            {
                return true;
            }

            return false;

        }


        public bool SaveHookshot()
        {
            List<byte>[] hookCount = new List<byte>[5];
            hookCount[0] = new List<byte>();
            hookCount[1] = new List<byte>();
            hookCount[2] = new List<byte>();
            hookCount[3] = new List<byte>();
            hookCount[4] = new List<byte>();
            byte cHook = 0;
            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    if (levels[l].maps[m].hooks.Count > 0)
                    {
                        hookCount[l].Add((byte)m);
                    }
                }
            }

            int addrOfJumpOffsetHooks = Utils.SnesToPc(rom.ReadShort(Constants.Hooks_Level_Offset) + 0x830000); // need to write that over
            int startofDataHooks = addrOfJumpOffsetHooks + rom.ReadByte(addrOfJumpOffsetHooks);

            //5+ 1 + (2* PREVIOUSlevelhookcount)
            rom.WriteByte(addrOfJumpOffsetHooks, 5); // write value 5 since it's always 5
            int cPos = 5;
            for (int i = 1; i < 5; i++)
            {
                cPos += ((hookCount[i - 1].Count * 2) + 1);
                rom.WriteByte(addrOfJumpOffsetHooks + i, (byte)(cPos));
            }



            byte cd = 0;
            for (int i = 0; i < 5; i++)
            {
                rom.WriteByte(startofDataHooks, (byte)hookCount[i].Count);
                startofDataHooks++;
                foreach (byte b in hookCount[i])
                {
                    rom.WriteByte(startofDataHooks, (byte)b);
                    startofDataHooks++;
                    rom.WriteByte(startofDataHooks, (byte)cd);
                    startofDataHooks++;
                    cd++;
                }
            }

            int nbr = 0;
            int posh = startofDataHooks + cd;
            int startDataHooksPtrs = startofDataHooks;
            rom.WriteShort(Constants.HooksDataPtrs, (short)(Utils.PcToSnes(startofDataHooks) & 0xFFFF));
            rom.WriteShort(Constants.HooksDataPtrs2, (short)(Utils.PcToSnes(startofDataHooks) & 0xFFFF));



            rom.WriteShort(Constants.HooksDataPtrsP1, (short)(Utils.PcToSnes(startofDataHooks + 1) & 0xFFFF)); //80AC44  (+1)   80AC44   SBC $8978,X
            rom.WriteShort(Constants.HooksDataPtrsP1_2, (short)(Utils.PcToSnes(startofDataHooks + 1) & 0xFFFF)); //80AC49  (+1) 80AC49   ADC $8978,X
            rom.WriteShort(Constants.HooksDataPtrsP2, (short)(Utils.PcToSnes(startofDataHooks + 2) & 0xFFFF)); //80AC55 (+2)    80AC55   SBC $8979,X
            rom.WriteShort(Constants.HooksDataPtrsP2_5, (short)(Utils.PcToSnes(startofDataHooks + 2) & 0xFFFF)); //80AC5A (+2)  80AC5A   ADC $8979,X 
            rom.WriteShort(Constants.HooksDataPtrsP3, (short)(Utils.PcToSnes(startofDataHooks + 3) & 0xFFFF)); //80AC40  (+3)   80AC40   LDA $897A,X
            rom.WriteShort(Constants.HooksDataPtrsP4, (short)(Utils.PcToSnes(startofDataHooks + 4) & 0xFFFF));//80AC51 (+4)     80AC51   LDA $897B,X

            rom.WriteShort(Constants.HooksDataPtrsP1_3, (short)(Utils.PcToSnes(startofDataHooks + 1) & 0xFFFF));//82A0A7  (+1)  82A0A7   SBC $8978,X
            rom.WriteShort(Constants.HooksDataPtrsP1_4, (short)(Utils.PcToSnes(startofDataHooks + 1) & 0xFFFF));//82A0CF  (+1)  82A0CF   SBC $8978,X

            rom.WriteShort(Constants.HooksDataPtrsP2_2, (short)(Utils.PcToSnes(startofDataHooks + 2) & 0xFFFF));//82A0B1  (+2)  82A0B1   SBC $8979,X
            rom.WriteShort(Constants.HooksDataPtrsP2_3, (short)(Utils.PcToSnes(startofDataHooks + 2) & 0xFFFF));//82A0D9  (+2)  82A0D9   SBC $8979,X

            rom.WriteShort(Constants.HooksDataPtrsP3_3, (short)(Utils.PcToSnes(startofDataHooks + 3) & 0xFFFF));//82A0BB (+3)   82A0BB   SBC $897A,X
            rom.WriteShort(Constants.HooksDataPtrsP3_4, (short)(Utils.PcToSnes(startofDataHooks + 3) & 0xFFFF));//82A0E3 (+3)   82A0E3   SBC $897A,X

            rom.WriteShort(Constants.HooksDataPtrsP4_2, (short)(Utils.PcToSnes(startofDataHooks + 4) & 0xFFFF));//82A0C5 (+4)   82A0C5   SBC $897B,X
            rom.WriteShort(Constants.HooksDataPtrsP4_3, (short)(Utils.PcToSnes(startofDataHooks + 4) & 0xFFFF));//82A0ED (+4)   82A0ED   SBC $897B,X

            rom.WriteShort(Constants.HooksDataPtrsP1_5, (short)(Utils.PcToSnes(startofDataHooks + 1) & 0xFFFF)); //82A102 (+1)  82A102   SBC $8978,X 
            rom.WriteShort(Constants.HooksDataPtrsP2_4, (short)(Utils.PcToSnes(startofDataHooks + 2) & 0xFFFF)); //82A10B (+2)  82A10B   SBC $8979,X 
            rom.WriteShort(Constants.HooksDataPtrsP3_5, (short)(Utils.PcToSnes(startofDataHooks + 3) & 0xFFFF)); //82A0FE (+3)  82A0FE   LDA $897A,X  
            rom.WriteShort(Constants.HooksDataPtrsP4_4, (short)(Utils.PcToSnes(startofDataHooks + 4) & 0xFFFF)); //82A107 (+4)  82A107   LDA $897B,X 


            for (int l = 0; l < 5; l++)
            {

                for (int j = 0; j < hookCount[l].Count; j++)
                {
                    rom.WriteByte(startDataHooksPtrs + nbr, (byte)(posh - startDataHooksPtrs));
                    nbr++;
                    //found a hook
                    rom.WriteByte(posh, (byte)levels[l].maps[hookCount[l][j]].hooks.Count); //write the count
                    posh++;
                    for (int k = 0; k < levels[l].maps[hookCount[l][j]].hooks.Count; k++)
                    {
                        rom.WriteByte(posh, (byte)levels[l].maps[hookCount[l][j]].hooks[k].x);
                        posh++;
                        rom.WriteByte(posh, (byte)levels[l].maps[hookCount[l][j]].hooks[k].y);
                        posh++;
                        rom.WriteByte(posh, (byte)levels[l].maps[hookCount[l][j]].hooks[k].x2);
                        posh++;
                        rom.WriteByte(posh, (byte)levels[l].maps[hookCount[l][j]].hooks[k].y2);
                        posh++;
                    }

                }
            }


            if (posh > 0x018A07)
            {
                //Not enough space for hooks
                MessageBox.Show("Not enough space to save hooks\r\nFailed to save, ROM was not saved to prevent corruption");
                return false;
            }
            return true;
        }


        public bool SavePlanks()
        {
            List<byte>[] plankCount = new List<byte>[5];
            plankCount[0] = new List<byte>();
            plankCount[1] = new List<byte>();
            plankCount[2] = new List<byte>();
            plankCount[3] = new List<byte>();
            plankCount[4] = new List<byte>();

            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    if (levels[l].maps[m].planks.Count > 0)
                    {
                        plankCount[l].Add((byte)m);
                    }
                }
            }

            int addrOfJumpOffsetPlanks = Utils.SnesToPc(rom.ReadShort(Constants.Planks_Level_Offset) + 0x830000); // need to write that over
            int startofDataPlanks = addrOfJumpOffsetPlanks + rom.ReadByte(addrOfJumpOffsetPlanks);

            //5+ 1 + (2* PREVIOUSlevelhookcount)
            rom.WriteByte(addrOfJumpOffsetPlanks, 5); // write value 5 since it's always 5
            int cPos = 5;
            for (int i = 1; i < 5; i++)
            {
                cPos += ((plankCount[i - 1].Count * 2) + 1);
                rom.WriteByte(addrOfJumpOffsetPlanks + i, (byte)(cPos));
            }



            byte cd = 0;
            for (int i = 0; i < 5; i++)
            {
                rom.WriteByte(startofDataPlanks, (byte)plankCount[i].Count);
                startofDataPlanks++;
                foreach (byte b in plankCount[i])
                {
                    rom.WriteByte(startofDataPlanks, (byte)b);
                    startofDataPlanks++;
                    rom.WriteByte(startofDataPlanks, (byte)cd);
                    startofDataPlanks++;
                    cd++;
                }
            }
            if (startofDataPlanks >= 0x1B949)
            {
                //Not enough space for planks
                MessageBox.Show("Not enough space to save planks\r\nFailed to save, ROM was not saved to prevent corruption");
                return false;
            }
            rom.WriteUShort(Constants.PlankDataPtrs, 0x8550); //2B
            rom.WriteUShort(Constants.PlankDataPtrs_1, 0x8550); //2B
            rom.WriteUShort(Constants.PlankDataPtrs_2, 0x8550); //2B
            rom.WriteUShort(Constants.PlankDataPtrs_3, 0x8550); //2B

            rom.WriteUShort(Constants.PlankDataPtrsP1_1, 0x8550 + 1); //2C
            rom.WriteUShort(Constants.PlankDataPtrsP2_1, 0x8550 + 2); //2D

            rom.WriteUShort(Constants.PlankDataPtrsP4_1, 0x8550 + 4);  //2F

            rom.WriteUShort(Constants.PlankDataPtrsP5_1, 0x8550 + 5); //30
            //rom.WriteUShort(Constants.PlankDataPtrsP5_2, 0x8550 + 5); //30
            rom.WriteUShort(Constants.PlankDataPtrsP5_3, 0x8550 + 5); //30

            int nbr = 0;
            int posh = 0x018550 + cd;
            int startDataPlanksPtrs = 0x18550;

            for (int l = 0; l < 5; l++)
            {
                int pnbr = 0;
                for (int j = 0; j < plankCount[l].Count; j++)
                {
                    rom.WriteByte(startDataPlanksPtrs + nbr, (byte)(posh - startDataPlanksPtrs));
                    nbr++;
                    //found a plank
                    rom.WriteByte(posh, (byte)levels[l].maps[plankCount[l][j]].planks.Count); //write the count
                    posh++;
                    for (int k = 0; k < levels[l].maps[plankCount[l][j]].planks.Count; k++)
                    {
                        rom.WriteByte(posh, (byte)levels[l].maps[plankCount[l][j]].planks[k].x);
                        posh++;
                        rom.WriteByte(posh, (byte)levels[l].maps[plankCount[l][j]].planks[k].y);
                        posh++;
                        short s = (short)(((levels[l].maps[plankCount[l][j]].planks[k].y / 8) * 32) + (levels[l].maps[plankCount[l][j]].planks[k].x / 8));
                        rom.WriteShort(posh, s);
                        posh++;
                        posh++;
                        rom.WriteByte(posh, 0x07);//use default plank doesn't matter
                        posh++;
                        rom.WriteByte(posh, (byte)pnbr);//use default plank doesn't matter
                        posh++;
                        pnbr++;
                    }
                    pnbr++;

                }
            }



            if (posh > 0x018630)
            {
                //Not enough space for planks
                MessageBox.Show("Not enough space to save planks\r\nFailed to save, ROM was not saved to prevent corruption");
                return false;
            }
            return true;
        }

        public bool SaveIceDark()
        {
            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    byte darkice = 0;
                    if (levels[l].maps[m].dark == true)
                    {
                        darkice += 2;
                    }
                    if (levels[l].maps[m].ice == true)
                    {
                        darkice += 1;
                    }

                    int ptrIce = Constants.darkiceValues + rom.ReadByte(Constants.darkiceValues + l);

                    rom.WriteByte(ptrIce + m, darkice);
                }
            }
            return true;
        }

        public bool SaveMapsTiles()
        {
            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    int mapJumpOffset = rom.ReadByte(Constants.Map32_Level_Ptr + (l));
                    int mapValue = rom.ReadByte(Constants.Map32_Level_Ptr + mapJumpOffset + (m * 2));
                    int bg2pos = Utils.SnesToPc((mapValue * 64) + 0x898000);
                    int bg2extpos = Utils.SnesToPc((mapValue * 32) + 0x89B700);

                    int mapValue2 = rom.ReadByte(Constants.Map32_Level_Ptr + mapJumpOffset + (m * 2) + 1);
                    int bg1pos = Utils.SnesToPc((mapValue2 * 64) + 0x898000);
                    int bg1extpos = Utils.SnesToPc((mapValue2 * 32) + 0x89B700);
                    for (int i = 0; i < 64; i++)
                    {
                        rom.WriteByte(bg1pos + i, (byte)levels[l].maps[m].bg1tilemap32[i]);
                        rom.WriteByte(bg2pos + i, (byte)levels[l].maps[m].bg2tilemap32[i]);
                    }

                    for (int i = 0; i < 32; i++)
                    {
                        byte ext = (byte)(((levels[l].maps[m].bg1tilemap32[(i * 2)] & 0xF00) >> 8) | ((levels[l].maps[m].bg1tilemap32[(i * 2) + 1] & 0xF00) >> 4));
                        byte ext2 = (byte)(((levels[l].maps[m].bg2tilemap32[(i * 2)] & 0xF00) >> 8) | ((levels[l].maps[m].bg2tilemap32[(i * 2) + 1] & 0xF00) >> 4));
                        rom.WriteByte(bg1extpos + (i), (byte)ext);
                        rom.WriteByte(bg2extpos + (i), (byte)ext2);
                    }
                }
            }
            return true;
        }

        public bool SaveMapsProperties()
        {
            int addrOfJumpOffsetSprSetGrp = Utils.SnesToPc(rom.ReadShort(Constants.SpritesSet_Level_Offset) + 0x830000);
            int addrOfJumpOffsetSprPalGrp = Utils.SnesToPc(rom.ReadShort(Constants.PalSprGroup_Level_Offset) + 0x830000);
            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    int ptrPosSprPalGrp = addrOfJumpOffsetSprPalGrp + rom.ReadByte(addrOfJumpOffsetSprPalGrp + l) + (m);
                    rom.WriteByte(ptrPosSprPalGrp, (byte)(levels[l].maps[m].spritepal));

                    int ptrPosSprSetGrp = addrOfJumpOffsetSprSetGrp + rom.ReadByte(addrOfJumpOffsetSprSetGrp + l) + (m);
                    rom.WriteByte(ptrPosSprSetGrp, (byte)(levels[l].maps[m].spriteset));

                    int pcPtr = rom.ReadShort(Constants.PalGroup_Level_Offset); //8775
                    int offsetJump = rom.ReadByte(Utils.SnesToPc((pcPtr + l) + 0x830000));
                    rom.WriteByte(Utils.SnesToPc((pcPtr) + 0x830000) + offsetJump + m, levels[l].maps[m].animatedPal);

                    levels[l].maps[m].SaveAltPalette();
                }
            }
            return true;
        }

        public bool SaveBlockDoors()
        {
            int blockDoorsCount = 0;
            bool doorblockExtraSpace = false;

            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    foreach (BlockDoor bd in levels[l].maps[m].blockDoors)
                    {
                        blockDoorsCount++;
                    }
                }
            }

            int headerDoorBlockPos = Constants.StarTilesData;
            int dataDoorBlockPos = Constants.StarTilesData + (blockDoorsCount * 2);
            int blockdoorindexItem = 0;


            for (int l = 0; l < 5; l++)
            {
                byte doorCountIdk = 0;
                byte ramDoor = 0;
                //byte ramHander = 0;
                for (int m = 0; m < levels[l].maps.Count; m++)
                {

                    //BLOCK SAVE
                    if (levels[l].maps[m].blockDoors.Count != 0)
                    {
                        foreach (BlockDoor bd in levels[l].maps[m].blockDoors)
                        {
                            if (!doorblockExtraSpace)
                            {
                                if (dataDoorBlockPos + (levels[l].maps[m].blockDoors.Count * 2) + 4 + 2 >= Constants.StarTilesDataLimit)
                                {
                                    dataDoorBlockPos = Constants.StarTilesData_Extra;
                                    doorblockExtraSpace = true;
                                }
                            }

                            rom.WriteShort(headerDoorBlockPos, (short)(Utils.PcToSnes(dataDoorBlockPos) & 0xFFFF));
                            headerDoorBlockPos += 2;
                            Item doorItem = new Item(0x24, (byte)(doorCountIdk << 4), (byte)(blockdoorindexItem * 2), 0);
                            dooritemstoremove[l][m].Add(doorItem);
                            levels[l].maps[m].items.Add(doorItem);
                            blockdoorindexItem++;
                            doorCountIdk++;
                            if (bd.drawswitch)
                            {
                                rom.WriteByte(dataDoorBlockPos, (byte)bd.addrAllBlocks.Count);
                            }
                            else
                            {
                                rom.WriteByte(dataDoorBlockPos, (byte)(bd.addrAllBlocks.Count + 0x80));
                            }

                            dataDoorBlockPos += 1;
                            for (int i = 0; i < bd.addrAllBlocks.Count; i++)
                            {
                                rom.WriteShort(dataDoorBlockPos, (short)bd.addrAllBlocks[i]);
                                dataDoorBlockPos += 2;
                            }
                            //byte doorRam, byte doorDir, ushort doorAddr
                            if (bd.saved)
                            {
                                rom.WriteByte(dataDoorBlockPos, bd.doorRam);
                                ramDoor++;
                            }
                            else
                            {
                                rom.WriteByte(dataDoorBlockPos, 0x80);
                            }


                            dataDoorBlockPos += 1;
                            rom.WriteByte(dataDoorBlockPos, bd.doorDir);
                            dataDoorBlockPos += 1;
                            rom.WriteShort(dataDoorBlockPos, (short)bd.doorAddr);
                            dataDoorBlockPos += 2;

                        }
                    }
                    if (dataDoorBlockPos >= 0x01FFB8)
                    {
                        MessageBox.Show("Too many door blocks\r\nFailed to save, ROM was not saved to prevent corruption");
                        return false;
                    }

                }
            }
            return true;
        }

        public bool SaveEnemiesDoors()
        {
            int enemyDoorCount1 = 0;
            int enemyDoorCount2 = 0;

            int door1Pos1 = Constants.enemyDoor1DataStartCR;
            int door1Pos2 = Constants.enemyDoor1DataStartAddrType;

            int door2Pos1 = Constants.enemyDoor2DataStartCD;
            int door2Pos2 = Constants.enemyDoor2DataStartAddrType;

            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    if (levels[l].maps[m].enemyDoors.Count != 0)
                    {
                        foreach (EnemyDoor d in levels[l].maps[m].enemyDoors)
                        {
                            if (d.save)
                            {
                                Item doorItem = new Item(0x21, (byte)(enemyDoorCount1 << 4), 0, 0);
                                dooritemstoremove[l][m].Add(doorItem);
                                levels[l].maps[m].items.Add(doorItem);

                                rom.WriteByte(door1Pos1, d.enemyCount);
                                door1Pos1++;
                                rom.WriteByte(door1Pos1, d.doorRamDir);
                                door1Pos1++;


                                rom.WriteUShort(door1Pos2, d.doorAddr);
                                door1Pos2++;
                                door1Pos2++;
                                if (d.explosing)
                                {
                                    rom.WriteByte(door1Pos2, 0x80);
                                }
                                else
                                {
                                    rom.WriteByte(door1Pos2, d.doorSize);
                                }

                                door1Pos2++;
                                enemyDoorCount1++;

                            }
                            else
                            {
                                //Console.WriteLine(door2Pos1.ToString("X6") +  " Direction Enemy Door, Level : " + l.ToString() + " Map :" + m.ToString("X2") + "DoorAddr = " + d.doorAddr.ToString("X4") + " TypeSize = " + d.doorSize.ToString("X2"));
                                Item doorItem = new Item(0x23, (byte)(enemyDoorCount2 << 4), 0, 0);
                                dooritemstoremove[l][m].Add(doorItem);
                                levels[l].maps[m].items.Add(doorItem);


                                rom.WriteByte(door2Pos1, d.enemyCount);
                                door2Pos1++;
                                rom.WriteByte(door2Pos1, d.doorRamDir);
                                door2Pos1++;


                                rom.WriteShort(door2Pos2, (short)d.doorAddr);
                                door2Pos2++;
                                door2Pos2++;
                                rom.WriteByte(door2Pos2, d.doorSize);
                                door2Pos2++;
                                enemyDoorCount2++;
                            }

                        }
                    }


                    if (door1Pos1 > 0x1C28D)
                    {
                        MessageBox.Show("Too many Enemy doors Type 1 (Using Save) \r\nFailed to save, ROM was not saved to prevent corruption");
                        return false;
                    }

                    if (door2Pos1 > 0x01C2CB)
                    {
                        MessageBox.Show("Too many Enemy doors Type 2 (Using Direction) \r\nFailed to save, ROM was not saved to prevent corruption");
                        return false;
                    }
                }
            }
            return true;
        }
    
        public bool SaveItems()
        {
            bool itemExtraSpace = false;
            int addrOfJumpOffset = Utils.SnesToPc(rom.ReadShort(Constants.Items_Level_Offset) + 0x800000); //EE6A -> 006E6A
            int startofPtrs = addrOfJumpOffset + rom.ReadByte(addrOfJumpOffset); //006E6F
            int startItemData = startofPtrs + (114 * 2); //skip all pointers to data + 228 //item data for level1

            for (int l = 0; l < 5; l++)
            {
                byte doorCountIdk = 0;
                byte ramItem = 0;
                byte ramFruits = 0;
                byte ramDoor = 0;

                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    if (itemExtraSpace == false)
                    {
                        if (startItemData + ((levels[l].maps[m].sprites.Count * 4) + 1) >= 0x72A9)
                        {
                            startItemData = extraSpaceitemsprite;//0x07374;
                            itemExtraSpace = true;

                        }
                    }


                    int ptrPos = addrOfJumpOffset + rom.ReadByte(addrOfJumpOffset + l) + (m * 2);
                    rom.WriteShort(ptrPos, (short)(Utils.PcToSnes(startItemData) & 0xFFFF));

                    //Write Item count
                    rom.WriteByte(startItemData, (byte)levels[l].maps[m].items.Count);
                    startItemData++;
                    foreach (Item item in levels[l].maps[m].items)
                    {
                        byte ramuse = item.ram;
                        if ((item.id & 0xF0) == 0x40) // fruit
                        {
                            ramuse = ramFruits;
                            ramFruits++;
                        }
                        else if ((item.id & 0xF0) == 0) // items hookshot, shovel, etc
                        {

                            ramuse = ramItem;
                            ramItem++;
                        }
                        rom.WriteByte(startItemData, item.id);
                        startItemData++;
                        rom.WriteByte(startItemData, ramuse);
                        startItemData++;
                        rom.WriteByte(startItemData, item.x);
                        startItemData++;
                        rom.WriteByte(startItemData, item.y);
                        startItemData++;
                    }

                    foreach (Item i in dooritemstoremove[l][m])
                    {
                        levels[l].maps[m].items.Remove(i);
                    }
                    dooritemstoremove[l][m].Clear();


                    if (itemExtraSpace)
                    {
                        extraSpaceitemsprite = startItemData;
                    }

                }
                
            }
            return true;

        }

        public bool SaveSprites()
        {
            bool sprExtraSpace = false;
            int addrOfJumpOffsetSpr = Utils.SnesToPc(rom.ReadShort(Constants.Sprites_Level_Offset) + 0x800000); //E760 -> 006760
            int startofPtrsSpr = addrOfJumpOffsetSpr + rom.ReadByte(addrOfJumpOffsetSpr); //006765
            int startSprData = startofPtrsSpr + (114 * 2); //skip all pointers to data + 228 //spr data for level1 //6849

            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    if (sprExtraSpace == false)
                    {
                        if (startSprData + ((levels[l].maps[m].sprites.Count * 5) + 1) >= 0x06E6A)
                        {
                            Console.WriteLine(startSprData.ToString("X6") + "  <- changed to sprite 2");
                            startSprData = extraSpaceitemsprite;//0x07374;
                            sprExtraSpace = true;
                        }

                    }
                    //TODO NEEDS TO FIX PROBLEM WITH ITEM AND SPRITES SAVE BEING IN THE SAME LOOP DOESNT WORK


                    int ptrPosSpr = addrOfJumpOffsetSpr + rom.ReadByte(addrOfJumpOffsetSpr + l) + (m * 2);
                    //Write Pointer for that map

                    rom.WriteShort(ptrPosSpr, (short)(Utils.PcToSnes(startSprData) & 0xFFFF));

                    //Write Sprite count
                    rom.WriteByte(startSprData, (byte)levels[l].maps[m].sprites.Count);
                    startSprData++;

                    // byte id, byte param, byte unkn, byte x, byte y
                    foreach (Sprite spr in levels[l].maps[m].sprites)
                    {
                        rom.WriteByte(startSprData, spr.id);
                        startSprData++;
                        rom.WriteByte(startSprData, spr.param);
                        startSprData++;
                        rom.WriteByte(startSprData, spr.unkn);
                        startSprData++;
                        rom.WriteByte(startSprData, spr.x);
                        startSprData++;
                        rom.WriteByte(startSprData, spr.y);
                        startSprData++;
                    }
                }
            }

            if (startSprData > 0x07FAF)
            {
                MessageBox.Show("Too many sprites or items\r\nFailed to save, ROM was not saved to prevent corruption");
                return false;
            }
            return true;
        }

        public bool SaveLockedDoors()
        {


            int totalLockedDoors = 0;
            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    foreach (LockedDoor d in levels[l].maps[m].lockedDoors)
                    {
                        totalLockedDoors++;
                    }
                }
            }
            int headerDoorLockedPos = Constants.LockedDoor_Pointer_Data;
            int dataLockedDoorPos = Constants.LockedDoor_Pointer_Data + totalLockedDoors + 1;
            int lockedDoorCount = 1;



            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {


                    int leveloffset = rom.ReadByte(0x014461 + l);
                    int posData = 0x014461 + leveloffset + m;
                    //Locked Doors Save
                    if (levels[l].maps[m].lockedDoors.Count != 0)
                    {
                        Console.WriteLine("Level " + l + " Map " + m.ToString("X2") + " Contains " + levels[l].maps[m].lockedDoors.Count.ToString("X2") + " Doors " + " Written at " + posData.ToString("X6"));
                        rom.WriteByte(posData, (byte)lockedDoorCount);
                        posData++;
                        lockedDoorCount++;

                        headerDoorLockedPos += 1;
                        rom.WriteByte(headerDoorLockedPos, (byte)((Utils.PcToSnes((dataLockedDoorPos - Constants.LockedDoor_Pointer_Data)) & 0xFF) - 1));
                        rom.WriteByte(dataLockedDoorPos, (byte)levels[l].maps[m].lockedDoors.Count);
                        dataLockedDoorPos++;
                        foreach (LockedDoor d in levels[l].maps[m].lockedDoors)
                        {



                            rom.WriteShort(dataLockedDoorPos, (short)d.doorAddr);
                            dataLockedDoorPos += 2;
                            rom.WriteByte(dataLockedDoorPos, d.doorDir);
                            dataLockedDoorPos += 1;

                            // ushort doorAddr, byte doorDir, byte doorRam
                            if (d.boss)
                            {
                                rom.WriteByte(dataLockedDoorPos, (byte)(0x80 + d.doorRam));
                            }
                            else
                            {
                                rom.WriteByte(dataLockedDoorPos, d.doorRam);
                            }


                            dataLockedDoorPos += 1;



                        }
                    }
                    else
                    {
                        rom.WriteByte(posData, 0);
                        posData++;
                    }

                    if (dataLockedDoorPos > 0x014538)
                    {
                        MessageBox.Show("Too many locked doors");
                        MessageBox.Show("Failed to save, ROM was not saved to prevent corruption");
                        return false;
                    }
                }
            }
            return true;
        }

        public bool SaveLiftObjects()
        {
            int addrOfJumpOffsetObj = Utils.SnesToPc(rom.ReadShort(Constants.Objects_Level_Offset) + 0x820000);
            int startofPtrsObj = addrOfJumpOffsetObj + rom.ReadByte(addrOfJumpOffsetObj);
            int startObjData = startofPtrsObj + (114 * 2); //skip all pointers to data

            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    int ptrPosObj = addrOfJumpOffsetObj + rom.ReadByte(addrOfJumpOffsetObj + l) + (m * 2);
                    rom.WriteShort(ptrPosObj, (short)(Utils.PcToSnes(startObjData) & 0xFFFF));

                    //Write Item count
                    rom.WriteByte(startObjData, (byte)levels[l].maps[m].objects.Count);
                    startObjData++;
                    foreach (LiftObject obj in levels[l].maps[m].objects)
                    {
                        rom.WriteByte(startObjData, obj.id);
                        startObjData++;
                        rom.WriteShort(startObjData, (short)obj.position);
                        startObjData++;
                        startObjData++;
                    }
                }
            }

            return true;
        }

        public bool SaveLevelProperties()
        {
            for (int l = 0; l < 5; l++)
            {
                int songPtr = 0x830000 + rom.ReadShort(Constants.SongAddr);
                rom.WriteByte(Utils.SnesToPc(songPtr + l), levels[l].song);

                int gfxgrpPtr = 0x830000 + rom.ReadShort(Constants.BGGfx_Level_Ptr);
                rom.WriteByte(Utils.SnesToPc(gfxgrpPtr) + (l * 2), levels[l].gfx1);
                rom.WriteByte(Utils.SnesToPc(gfxgrpPtr) + (l * 2) + 1, levels[l].gfx2);
            }
            return true;
        }

        public bool SavePasswords()
        {
            //PASSWORD SAVE
            for (int i = 0; i < 4; i++)
            {
               rom.WriteBytes(Constants.PasswordData + (i * 5), passwords[i]);
            }
            return true;
        }

        public bool SaveTexts()
        {
            int pos = Constants.Messages_Space;
            bool extraSpace = false;
            for (int i = 0; i < 50; i++)
            {
                if (!extraSpace)
                {
                    if ((pos + texts[i].Length) >= Constants.Messages_Space_Limit)
                    {
                        pos = Constants.Messages_ExtraSpace;
                        extraSpace = true;
                        i--; //undo that message
                        Console.WriteLine("We jumped to extra space at message " + i.ToString("X2"));
                        continue;
                    }
                    else
                    {
                        rom.WriteBytes(pos, texts[i]);
                        rom.WriteShort(Constants.Messages_Pointer + (i * 2), (short)Utils.PcToSnes(pos));
                        pos += texts[i].Length;
                    }
                }
                else
                {
                    if (pos + texts[i].Length >= Constants.Messages_ExtraSpace_Limit)
                    {
                        pos = Constants.Messages_ExtraSpace;
                        //we're lacking space for message :Scream:
                        MessageBox.Show("Not enough space for messages!!\r\nFailed to save, ROM was not saved to prevent corruption");
                        return false;
                    }
                    else
                    {
                        rom.WriteBytes(pos, texts[i]);
                        rom.WriteShort(Constants.Messages_Pointer + (i * 2), (short)pos);
                        pos += texts[i].Length;
                    }
                }


            }
            return true;

        }

        public bool SaveTransitions()
        {
            int addrOfJumpOffsetTransition = Utils.SnesToPc(rom.ReadShort(Constants.Transitions_Level_Offset) + 0x830000);
            int startofPtrsTransition = addrOfJumpOffsetTransition + rom.ReadByte(addrOfJumpOffsetTransition);
            int startTransitionData = startofPtrsTransition + (114 * 2); //skip all pointers to data


            for (int l = 0; l < 5; l++)
            {
                for (int m = 0; m < levels[l].maps.Count; m++)
                {
                    int ptrPosTransition = addrOfJumpOffsetTransition + rom.ReadByte(addrOfJumpOffsetTransition + l) + (m * 2);
                    //Write Pointer for that map

                    rom.WriteShort(ptrPosTransition, (short)(Utils.PcToSnes(startTransitionData) & 0xFFFF));

                    //Write Sprite count
                    rom.WriteByte(startTransitionData, (byte)levels[l].maps[m].transitions.Count);
                    startTransitionData++;

                    // byte tomap, ushort position, byte dir, byte xDest, byte yDest
                    foreach (Transition transition in levels[l].maps[m].transitions)
                    {
                        rom.WriteByte(startTransitionData, transition.tomap);
                        startTransitionData++;
                        rom.WriteShort(startTransitionData, (short)transition.position);
                        startTransitionData++;
                        startTransitionData++;
                        rom.WriteByte(startTransitionData, transition.dir);
                        startTransitionData++;
                        rom.WriteByte(startTransitionData, transition.xDest);
                        startTransitionData++;
                        rom.WriteByte(startTransitionData, transition.yDest);
                        startTransitionData++;

                    }

                    //0001FAB4 maybe? not sure if used
                    if (startTransitionData > 0x01FA53)
                    {
                        MessageBox.Show("Too many transitions!\r\nFailed to save, ROM was not saved to prevent corruption");
                        return false;
                    }
                }//end of map loop
            }//end of level loop
            return true;

        }

        byte[] creditEnd = new byte[]
        {
            0xFF, 0x0B, 0x80, 0x20, 0x54, 0x4F, 0x54, 0x41, 0x4C, 0x20, 0x54, 0x49, 0x4D, 0x45, 0x02, 0x0B,
            0x81, 0x0B, 0x01, 0x80
        };

        public bool SaveCredits()
        {
            int creditPos = 0x5F99E; //credit start position
           
            for(int i = 0;i<creditLines.Count;i++)
            {
                rom.WriteByte(creditPos, creditLines[i].linesSkip);
                creditPos++;
                rom.WriteByte(creditPos, creditLines[i].xStart);
                creditPos++;
                rom.WriteByte(creditPos, creditLines[i].cCount);
                creditPos++;
                rom.WriteByte(creditPos, creditLines[i].palette);
                creditPos++;
                for(int j = 0;j< creditLines[i].cCount;j++)
                {
                    rom.WriteByte(creditPos, creditLines[i].data[j]);
                    creditPos++;
                }
            }

            for(int i = 0; i< creditEnd.Length;i++)
            {
               
                rom.WriteByte(creditPos, creditEnd[i]);
                creditPos++;
            }


            if (creditPos >= 0x5FE00)
            {
                MessageBox.Show("Not enough space for credits!");
                return false;
            }
            return true;
        }

        

    }
}
