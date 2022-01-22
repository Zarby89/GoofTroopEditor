using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public class Map
    {
        byte global_index = 0;
        byte index = 0;
        public ushort[] bg1tilemap8 = new ushort[1024];
        public ushort[] bg2tilemap8 = new ushort[1024];

        public ushort[] bg1tilemap16 = new ushort[256];
        public ushort[] bg2tilemap16 = new ushort[256];

        public ushort[] bg1tilemap32 = new ushort[64];
        public ushort[] bg2tilemap32 = new ushort[64];
        public byte spriteset = 0;
        public byte spritepal = 0;
        public byte altPal = 0;
        public byte animatedPal = 0;
        public bool ice = false;
        public bool dark = false;
        public List<Sprite> sprites = new List<Sprite>();
        public List<LiftObject> objects = new List<LiftObject>();
        public List<Item> items = new List<Item>();
        public List<Transition> transitions = new List<Transition>();
        public List<Hook> hooks = new List<Hook>();
        public List<Plank> planks = new List<Plank>();
        public List<BlockDoor> blockDoors = new List<BlockDoor>();
        public List<LockedDoor> lockedDoors = new List<LockedDoor>();
        public List<EnemyDoor> enemyDoors = new List<EnemyDoor>();
        public List<ushort[]> undoTiles = new List<ushort[]>();
        public List<ushort[]> undoTiles2 = new List<ushort[]>();
        public int undoPos = 0;

        Game game;
        byte level;
        public Map(Game game, byte global_index, byte index, byte level)
        {
            this.global_index = global_index;
            this.index = index;
            this.level = level;
            this.game = game;
            int offsetMaps = (global_index / 8);
            //int bg1pos = Constants.Map32Data + (offsetMaps * 0x400) + ((index%8) * 0x40);
            //int bg2pos = (Constants.Map32Data + 0x200) + (offsetMaps * 0x400) + ((index % 8) * 0x40);

            int mapJumpOffset = game.rom.ReadByte(Constants.Map32_Level_Ptr + (level));
            int mapValue =  game.rom.ReadByte(Constants.Map32_Level_Ptr + mapJumpOffset + (index*2));
            int bg2pos = Utils.SnesToPc((mapValue * 64) + 0x898000);
            int bg2extpos = Utils.SnesToPc((mapValue * 32) + 0x89B700);

            int mapValue2 = game.rom.ReadByte(Constants.Map32_Level_Ptr + mapJumpOffset + (index * 2)+1);
            int bg1pos = Utils.SnesToPc((mapValue2 * 64) + 0x898000);
            int bg1extpos = Utils.SnesToPc((mapValue2 * 32) + 0x89B700);

            if (level == 1)
            {
                int m = (index / 2);
                if (index % 2 == 0) 
                { 
                    if ((game.rom.ReadByte(Constants.level02PalAlt+m) & 0xF0) != 0)
                    {
                        altPal = (byte)((game.rom.ReadByte(Constants.level02PalAlt + m) & 0xF0)>>4);
                    }
                }
                else
                {
                    if ((game.rom.ReadByte(Constants.level02PalAlt+m) & 0x0F) != 0)
                    {
                        altPal = (byte)((game.rom.ReadByte(Constants.level02PalAlt + m) & 0x0F));
                    }
                }
            }
            else if (level == 2)
            {
                int m = (index / 2);
                if (index % 2 == 0)
                {
                    if ((game.rom.ReadByte(Constants.level03PalAlt + m) & 0xF0) != 0)
                    {
                        altPal = (byte)((game.rom.ReadByte(Constants.level03PalAlt + m) & 0xF0) >> 4);
                    }
                }
                else
                {
                    if ((game.rom.ReadByte(Constants.level03PalAlt + m) & 0x0F) != 0)
                    {
                        altPal = (byte)((game.rom.ReadByte(Constants.level03PalAlt + m) & 0x0F));
                    }
                }
            }
            else if (level == 3)
            {
                int m = (index / 2);
                if (index % 2 == 0)
                {
                    if ((game.rom.ReadByte(Constants.level04PalAlt + m) & 0xF0) != 0)
                    {
                        altPal = (byte)((game.rom.ReadByte(Constants.level04PalAlt + m) & 0xF0) >> 4);
                    }
                }
                else
                {
                    if ((game.rom.ReadByte(Constants.level04PalAlt + m) & 0x0F) != 0)
                    {
                        altPal = (byte)((game.rom.ReadByte(Constants.level04PalAlt + m) & 0x0F));
                    }
                }
            }
            else if (level == 4)
            {
                int m = (index / 2);
                if (index % 2 == 0)
                {
                    if ((game.rom.ReadByte(Constants.level05PalAlt + m) & 0xF0) != 0)
                    {
                        altPal = (byte)((game.rom.ReadByte(Constants.level05PalAlt + m) & 0xF0) >> 4);
                    }
                }
                else
                {
                    if ((game.rom.ReadByte(Constants.level05PalAlt + m) & 0x0F) != 0)
                    {
                        altPal = (byte)((game.rom.ReadByte(Constants.level05PalAlt + m) & 0x0F));
                    }
                }
            }


            
            for (int i = 0; i < 64; i += 2)
            {
                bg1tilemap32[i] = (ushort)(game.rom.data[bg1pos + i] + ((game.rom.data[bg1extpos + (i / 2)] & 0x0F) << 8));
                bg1tilemap32[i + 1] = (ushort)(game.rom.data[bg1pos + i + 1] + ((game.rom.data[bg1extpos + (i / 2)] & 0xF0) << 4));

                bg2tilemap32[i] = (ushort)(game.rom.data[bg2pos + i] + ((game.rom.data[bg2extpos + (i / 2)] & 0x0F) << 8));
                bg2tilemap32[i + 1] = (ushort)(game.rom.data[bg2pos + i + 1] + ((game.rom.data[bg2extpos + (i / 2)] & 0xF0) << 4));
            }
            int t32 = 0;
            if (level < 3)
            {
                for (int y = 0; y < 16; y += 2)
                {
                    for (int x = 0; x < 16; x += 2)
                    {
                        bg1tilemap16[x + (y * 16)] = game.tiles32[bg1tilemap32[t32]].tile0;
                        bg1tilemap16[x + 1 + (y * 16)] = game.tiles32[bg1tilemap32[t32]].tile1;
                        bg1tilemap16[x + (y * 16) + 16] = game.tiles32[bg1tilemap32[t32]].tile2;
                        bg1tilemap16[x + (y * 16) + 17] = game.tiles32[bg1tilemap32[t32]].tile3;

                        bg2tilemap16[x + (y * 16)] = game.tiles32[bg2tilemap32[t32]].tile0;
                        bg2tilemap16[x + 1 + (y * 16)] = game.tiles32[bg2tilemap32[t32]].tile1;
                        bg2tilemap16[x + (y * 16) + 16] = game.tiles32[bg2tilemap32[t32]].tile2;
                        bg2tilemap16[x + (y * 16) + 17] = game.tiles32[bg2tilemap32[t32]].tile3;
                        t32++;
                    }
                }
            }
            else
            {
                for (int y = 0; y < 16; y += 2)
                {
                    for (int x = 0; x < 16; x += 2)
                    {
                        bg1tilemap16[x + (y * 16)] = game.tiles32_ex[bg1tilemap32[t32]].tile0;
                        bg1tilemap16[x + 1 + (y * 16)] = game.tiles32_ex[bg1tilemap32[t32]].tile1;
                        bg1tilemap16[x + (y * 16) + 16] = game.tiles32_ex[bg1tilemap32[t32]].tile2;
                        bg1tilemap16[x + (y * 16) + 17] = game.tiles32_ex[bg1tilemap32[t32]].tile3;

                        bg2tilemap16[x + (y * 16)] = game.tiles32_ex[bg2tilemap32[t32]].tile0;
                        bg2tilemap16[x + 1 + (y * 16)] = game.tiles32_ex[bg2tilemap32[t32]].tile1;
                        bg2tilemap16[x + (y * 16) + 16] = game.tiles32_ex[bg2tilemap32[t32]].tile2;
                        bg2tilemap16[x + (y * 16) + 17] = game.tiles32_ex[bg2tilemap32[t32]].tile3;
                        t32++;
                    }
                }
            }
            int t16 = 0;
            for (int y = 0; y < 32; y += 2)
            {
                for (int x = 0; x < 32; x += 2)
                {
                    bg1tilemap8[x + (y * 32)] = game.tiles16[bg1tilemap16[t16]].tile0.toShort();
                    bg1tilemap8[x + 1 + (y * 32)] = game.tiles16[bg1tilemap16[t16]].tile1.toShort();
                    bg1tilemap8[x + (y * 32) + 32] = game.tiles16[bg1tilemap16[t16]].tile2.toShort();
                    bg1tilemap8[x + (y * 32) + 33] = game.tiles16[bg1tilemap16[t16]].tile3.toShort();

                    bg2tilemap8[x + (y * 32)] = game.tiles16[bg2tilemap16[t16]].tile0.toShort();
                    bg2tilemap8[x + 1 + (y * 32)] = game.tiles16[bg2tilemap16[t16]].tile1.toShort();
                    bg2tilemap8[x + (y * 32) + 32] = game.tiles16[bg2tilemap16[t16]].tile2.toShort();
                    bg2tilemap8[x + (y * 32) + 33] = game.tiles16[bg2tilemap16[t16]].tile3.toShort();
                    t16++;
                }
            }



            int pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.SpritesSet_Level_Offset) + 0x830000));
            int offsetJump = game.rom.ReadByte(pcPtr + level);
            spriteset = game.rom.ReadByte(pcPtr + offsetJump + index);

            pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.PalSprGroup_Level_Offset) + 0x830000));
            offsetJump = game.rom.ReadByte(pcPtr + level);
            spritepal = game.rom.ReadByte(pcPtr + offsetJump + index);

            pcPtr = game.rom.ReadShort(Constants.PalGroup_Level_Offset); //8775
            offsetJump = game.rom.ReadByte(Utils.SnesToPc((pcPtr + level)+0x830000));
            animatedPal = game.rom.ReadByte(Utils.SnesToPc((pcPtr) + 0x830000) + offsetJump + index);


            int ptrIce = Constants.darkiceValues + game.rom.ReadByte(Constants.darkiceValues + level);

            if (game.rom.ReadByte(ptrIce + index) == 1)
            {
                ice = true;
            }
            if (game.rom.ReadByte(ptrIce + index) == 2)
            {
                dark = true;
            }
            if (game.rom.ReadByte(ptrIce + index) == 3)
            {
                ice = true;
                dark = true;
            }



            LoadHooks();

            LoadPlanks();

            LoadSprites();

            LoadObjects();

            LoadItems();

            LoadTransitions();

            LoadLockedDoors();

        }

        private void LoadLockedDoors()
        {
            //lockedDoors
            int pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.LockedDoors_Level_Offset) + 0x820000)); //82C461
            int offsetJump = game.rom.ReadByte(pcPtr + level); //$05, $06, $0D, $12, $15
            int addr = (pcPtr + offsetJump + index);

            byte lockID = game.rom.ReadByte(addr);

            if (lockID != 0) // no door if 00
            {
                byte joff = game.rom.ReadByte((Constants.LockedDoor_Pointer_Data + lockID));
                
                int addr2 = (joff + Constants.LockedDoor_Pointer_Data + 1);
                byte count = game.rom.ReadByte(addr2); // count
                addr2++;
                for(int i = 0;i<count;i++)
                {
                    bool saved = false;
                    if ((game.rom.ReadByte(addr2 + 3) & 0x80) == 0x80)
                    {
                        saved = true;
                    }
                    lockedDoors.Add(new LockedDoor(game.rom.ReadShort(addr2), (byte)(game.rom.ReadByte(addr2 + 2)), (byte)(game.rom.ReadByte(addr2 + 3) & 0x0F), saved));
                }


            }

        }

        private void LoadHooks()
        {
            int pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.Hooks_Level_Offset) + 0x830000)); //0x8959 + 0x830000
            int offsetJump = game.rom.ReadByte(pcPtr + level); //$05, $06, $0D, $12, $15
            int addr = (pcPtr + offsetJump);
            int hookptrdata = Utils.SnesToPc(game.rom.ReadShort(Constants.HooksDataPtrs) + 0x830000);

           byte count = game.rom.ReadByte(addr); //Hooks data
            addr++;
            for (int i = 0; i < count; i++)
            {
                if (game.rom.ReadByte(addr) == index) // if room == hook room
                {
                    byte h = game.rom.ReadByte(addr + 1); // data byte of hook (pointer)
                   
                    
                    byte jpos = game.rom.ReadByte((hookptrdata + h));
                    addr = hookptrdata + jpos;
                    byte count2 = game.rom.ReadByte(addr);
                    addr++;

                    for (int j = 0; j < count2; j++)
                    {
                    
                        hooks.Add(new Hook(game.rom.ReadByte(addr), game.rom.ReadByte(addr + 1), game.rom.ReadByte(addr + 2), game.rom.ReadByte(addr + 3)));
                        addr += 4;
                    }

                    break;
                }
                
                addr += 2;
            }
        }


        private void LoadPlanks()
        {
            int pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.Planks_Level_Offset) + 0x830000)); //0x8959 + 0x830000
            int offsetJump = game.rom.ReadByte(pcPtr + level); //$05, $06, $0D, $12, $15
            int addr = (pcPtr + offsetJump);
            int plankptrdata = Utils.SnesToPc(game.rom.ReadShort(Constants.PlankDataPtrs) + 0x830000);

            byte count = game.rom.ReadByte(addr); //how many map there's plank in that level
            addr++;
            for (int i = 0; i < count; i++)
            {
                if (game.rom.ReadByte(addr) == index) // if room == hook room
                {
                    byte h = game.rom.ReadByte(addr+1); // data byte of hook (pointer) plank data index

                    byte jpos = game.rom.ReadByte((plankptrdata + h)); //add position of plankdataptr + plank data index
                    addr = plankptrdata + jpos;
                    byte count2 = game.rom.ReadByte(addr); //frist byte of the plank data COUNT
                    Console.WriteLine("Count2 Addr : " + addr.ToString("X6") + " = " + count2.ToString("X2"));
                    addr++;

                    for (int j = 0; j < count2; j++)
                    {
                        Console.WriteLine("Found plank in room " + index.ToString("X2") + " at position " + game.rom.ReadByte(addr).ToString("X2") +", "+ game.rom.ReadByte(addr+1).ToString("X2"));
                        planks.Add(new Plank(game.rom.ReadByte(addr), game.rom.ReadByte(addr + 1)));
                        addr += 6;
                    }

                    break;
                }

                addr += 2;
            }
        }

        private void LoadTransitions()
        {
            int pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.Transitions_Level_Offset3) + 0x830000));
            int offsetJump = game.rom.ReadByte(pcPtr + level);
            int addr = Utils.SnesToPc(game.rom.ReadShort(pcPtr + offsetJump + (index * 2)) + 0x830000);

            byte count = game.rom.ReadByte(addr); //Transition count ; 6 bytes
            addr++;
            for (int i = 0; i < count; i++)
            {
                transitions.Add(new Transition(game.rom.ReadByte(addr), game.rom.ReadShort(addr + 1), game.rom.ReadByte(addr + 3), game.rom.ReadByte(addr + 4), game.rom.ReadByte(addr + 5)));
                addr += 6;
            }
        }

        private void LoadItems()
        {
            List<Item> itemtoremove = new List<Item>();
            int pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.Items_Level_Offset) + 0x800000));
            int offsetJump = game.rom.ReadByte(pcPtr + level);
            int addr = Utils.SnesToPc(game.rom.ReadShort(pcPtr + offsetJump + (index * 2)) + 0x800000);

            byte count = game.rom.ReadByte(addr); //Items count
            addr++;
            for (int i = 0; i < count; i++)
            {
                items.Add(new Item(game.rom.ReadByte(addr), game.rom.ReadByte(addr + 1), game.rom.ReadByte(addr + 2), game.rom.ReadByte(addr + 3)));
                addr += 4;
                if (items[i].id == 0x24) //doorblocks)
                {
                    byte blockID = items[i].x;
                    int pcPtrBlock = Utils.SnesToPc((game.rom.ReadShort(Constants.StarBlocksPtrs + blockID) + 0x830000));
                    byte countBlock = (byte)(game.rom.ReadByte(pcPtrBlock) & 0x0F);
                    bool drawswitch = true;
                    if ((game.rom.ReadByte(pcPtrBlock) & 0x80) == 0x80)
                    {
                        drawswitch = false;
                    }

                    pcPtrBlock++;
                    ushort[] addrBlocks = new ushort[countBlock];
                    for (int j = 0; j < countBlock; j++)
                    {
                        addrBlocks[j] = game.rom.ReadShort(pcPtrBlock);
                        pcPtrBlock += 2;
                    }
                    bool saved = true;
                    if ((game.rom.ReadByte(pcPtrBlock) & 0x80) == 0x80)
                    {
                        saved = false;
                    }

                    blockDoors.Add(new BlockDoor(addrBlocks, (byte)(game.rom.ReadByte(pcPtrBlock) & 0x0F), (byte)(game.rom.ReadByte(pcPtrBlock + 1) & 0x0F), game.rom.ReadShort(pcPtrBlock + 2), saved, drawswitch));
                    itemtoremove.Add(items[i]);
                }
                else if (items[i].id == 0x20) //doorblocks dumb removing them)
                {
                    byte blockID = (byte)((items[i].ram) >> 4);
                    bool drawswitch = false;
                    bool saved = false;
                    ushort doorPos = game.rom.ReadShort(0x01C275 + (blockID * 4));
                    byte doorDir = game.rom.ReadByte(0x01C275 + (blockID * 4)+2);
                    ushort blockPos = (ushort)(items[i].x + (items[i].y << 8));
                    blockDoors.Add(new BlockDoor(new ushort[] { blockPos},0x00, (byte)(doorDir & 0x0F), doorPos, saved, drawswitch));
                    itemtoremove.Add(items[i]);
                }
                else if (items[i].id == 0x21) //enemy door1 (save = true)
                {
                    //2nd item byte >> 3 == id to use in the list
                    byte dv = (byte)(items[i].ram >> 4);
                    byte pirateCount = game.rom.ReadByte(Constants.enemyDoor1DataStartCR + (dv*2));
                    byte ram = game.rom.ReadByte(Constants.enemyDoor1DataStartCR + (dv*2) + 1);

                    ushort daddr = game.rom.ReadShort(Constants.enemyDoor1DataStartAddrType + (dv * 3));
                    byte type = game.rom.ReadByte(Constants.enemyDoor1DataStartAddrType + (dv * 3) + 2);
                    bool exp = false;
                    if ((type & 0x80) == 0x80)
                    {
                        exp = true;
                    }

                    enemyDoors.Add(new EnemyDoor(pirateCount, ram, daddr, (byte)(type & 0x0F), true,exp));

                    itemtoremove.Add(items[i]);
                }
                else if (items[i].id == 0x23) //enemy door2 (save = false)
                {
                    byte dv = (byte)(items[i].ram >> 4);
                    byte pirateCount = game.rom.ReadByte(Constants.enemyDoor2DataStartCD + (dv * 2));
                    byte dir = game.rom.ReadByte(Constants.enemyDoor2DataStartCD + (dv * 2) + 1);

                    ushort daddr = game.rom.ReadShort(Constants.enemyDoor2DataStartAddrType + (dv * 3));
                    byte type = game.rom.ReadByte(Constants.enemyDoor2DataStartAddrType + (dv * 3) + 2);

                    enemyDoors.Add(new EnemyDoor(pirateCount, dir, daddr, type, false));
                    itemtoremove.Add(items[i]);
                }

            }

            foreach(Item i in itemtoremove)
            {
                items.Remove(i);
            }

            
        }

        private void LoadObjects()
        {
            int pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.Objects_Level_Offset) + 0x820000));
            int offsetJump = game.rom.ReadByte(pcPtr + level);
            int addr = Utils.SnesToPc(game.rom.ReadShort(pcPtr + offsetJump + (index * 2)) + 0x820000);

            byte count = game.rom.ReadByte(addr); //Object Count
            addr++;
            for (int i = 0; i < count; i++)
            {
                objects.Add(new LiftObject(game.rom.ReadByte(addr), game.rom.ReadShort(addr+1)));
                addr += 3;
            }
        }

        private void LoadSprites()
        {
            int pcPtr = Utils.SnesToPc((game.rom.ReadShort(Constants.Sprites_Level_Offset) + 0x800000));
            int offsetJump = game.rom.ReadByte(pcPtr + level);
            int addr = Utils.SnesToPc(game.rom.ReadShort(pcPtr + offsetJump + (index * 2)) + 0x800000);

            //load count of sprites
            byte count = game.rom.ReadByte(addr); //Sprite Count
            addr++;
            for(int i = 0;i<count;i++)
            {
                sprites.Add(new Sprite(game.rom.ReadByte(addr), game.rom.ReadByte(addr+1), game.rom.ReadByte(addr+2), game.rom.ReadByte(addr+3), game.rom.ReadByte(addr+4)));
                addr += 5;
            }
        }

        public void SaveAltPalette()
        {
            if (level == 1)
            {
                int m = (index / 2);
                byte b = 0;
                if (index % 2 == 0)
                {
                    b = (byte)((game.rom.ReadByte(Constants.level02PalAlt + m) & 0x0F));
                    b = (byte)(b + (altPal << 4));
                }
                else
                {

                    b = (byte)((game.rom.ReadByte(Constants.level02PalAlt + m) & 0xF0));
                    b = (byte)(b + (altPal));
                }

                game.rom.WriteByte(Constants.level02PalAlt + m, b);
            }
            else if (level == 2)
            {
                int m = (index / 2);
                byte b = 0;
                if (index % 2 == 0)
                {

                        b = (byte)((game.rom.ReadByte(Constants.level03PalAlt + m) & 0x0F));
                        b = (byte)(b + (altPal << 4));
                }
                else
                {

                        b = (byte)((game.rom.ReadByte(Constants.level03PalAlt + m) & 0xF0));
                        b = (byte)(b + (altPal));
                }

                game.rom.WriteByte(Constants.level03PalAlt + m, b);
            }
            else if (level == 3)
            {
                int m = (index / 2);
                byte b = 0;
                if (index % 2 == 0)
                {

                        b = (byte)((game.rom.ReadByte(Constants.level04PalAlt + m) & 0x0F));
                        b = (byte)(b + (altPal << 4));
                    
                }
                else
                {

                        b = (byte)((game.rom.ReadByte(Constants.level04PalAlt + m) & 0xF0));
                        b = (byte)(b + (altPal));
                }

                game.rom.WriteByte(Constants.level04PalAlt + m, b);
            }
            else if (level == 4)
            {
                int m = (index / 2);
                byte b = 0;
                if (index % 2 == 0)
                {
                        b = (byte)((game.rom.ReadByte(Constants.level05PalAlt + m) & 0x0F));
                        b = (byte)(b + (altPal << 4));
                }
                else
                {
                        b = (byte)((game.rom.ReadByte(Constants.level05PalAlt + m) & 0xF0));
                        b = (byte)(b + (altPal));
                }

                game.rom.WriteByte(Constants.level05PalAlt + m, b);
            }

        }


    }
}
