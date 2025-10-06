using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofED
{
    public static class Constants
    {
        //All variables are defined into PC Addresses
        //Game has Levels and Maps     1     2      3       4      5
        //Level are the Game Level (Island, Cave, Castle, Cavern, Ship)
        //Maps are each screen in a level

        //All addresses are already defined on the data directly and may differ from snes addresses

        //ALL MAPS RELATED DATA

        //Sprites Related Variables================================================================
        //Bank + Offset is used to load another offset in that bank
        //Those are "Level" Offset Originally : 05 25 45 79 B5 also 05 15 25 3F 5D for 1 byte data
        //so data for level 1 start after 5 bytes since it's skipping those 5 bytes
        //data level 2 after 0x25 bytes since it's skipping all 18 maps of level1

        public static int Sprites_Level_Bank = 0x0142CF; //82C2CE        ;PEA #$8380       B[0x01]
        public static int Sprites_Level_Offset = 0x0142D8; //82C2D7      ;ADC $E760,X      W[0x02]
        public static int Sprites_Level_Offset2 = 0x0142DE; //82C2DD     ;LDY $E760,X      W[0x02]

        //bank 0x83
        public static int SpritesSet_Level_Offset = 0x0030E6; //80B0E5      ;LDA $8BFE,X      W[0x02]
        public static int SpritesSet_Level_Offset2 = 0x0030ED; //80B0EC     ;LDY $8BFE,X      W[0x02]
        public static int SpriteSet_Data = 0x018C7A; // 3 bytes values of data for each values
        //Items Related Variables==================================================================
        //Same as Sprites
        public static int Items_Level_Bank = 0x00656B;//80E56A           ;PEA #$8380       B[0x01]
        public static int Items_Level_Offset = 0x006574;//80E573         ;ADC $EE6A,X      W[0x02]
        public static int Items_Level_Offset2 = 0x00657A;//80E579        ;LDY $EE6A,X      W[0x02]

        //Block,Liftable Variables=================================================================
        //Same as Sprites
        public static int Objects_Level_Bank = 0x01417D;//82C17C         ;PEA #$8382       B[0x01]
        public static int Objects_Level_Offset = 0x014186;//82C185       ;ADC $C538,X      W[0x02]
        public static int Objects_Level_Offset2 = 0x01418C;//82C18B      ;LDY $C538,X      W[0x02]

        //Transitions Variables===================================================================
        //No bank change for this one (databank is setted on 0x83) might need to be changed
        public static int Transitions_Level_Offset = 0x01423B; //82C23A  ;ADC $F303,X      W[0x02]
        public static int Transitions_Level_Offset2 = 0x014241;//82C240  ;LDX $F303,Y      W[0x02]
        public static int Transitions_Level_Offset3 = 0x002792;//80A791  ;ADC $F303,X      W[0x02]
        public static int Transitions_Level_Offset4 = 0x0027A2;//80A7A1  ;ADC $F303,Y      W[0x02]

        //Palettes Groups Variables===============================================================
        //No bank change for this one (databank is setted on 0x80) might need to be changed
        public static int PalGroup_Level_Offset = 0x002AF3;//80AAF2      ;ADC $8775,X      W[0x02]
        public static int PalGroup_Level_Offset2 = 0x002AF9;//80AAF8     ;LDX $8775,Y      W[0x02]

        public static int PalGroup_DataPtr = 0x0187EA;

        public static int staticSprPalette = 0x056000;
        //Palettes Sprites Groups Variables=======================================================
        //Contains value of palette group used for each maps (databank is setted on 0x83)
        public static int PalSprGroup_Level_Offset = 0x002CFB;//80ACFA   ;LDA $8A93,X      W[0x02]
        public static int PalSprGroup_Level_Offset2 = 0x002D02;//80AD01  ;LDX $8A93,Y      W[0x02]

        //Palettes Sprites Groups DATA Variables==================================================
        //Contains value of palette data for the groups (databank is setted on 0x83)
        public static int PalSprData1_Ptr = 0x002D05;//80AD04            ;LDA $8B0A,X      W[0x02]
        public static int PalSprData2_Ptr = 0x002D0A;//80AD09            ;LDA $8B0B,X      W[0x02]
        public static int PalSprData3_Ptr = 0x002D0F;//80AD0E            ;LDA $8B0C,X      W[0x02]
        public static int PalSprData4_Ptr = 0x002D14;//80AD13            ;LDA $8B0D,X      W[0x02]
        //Sprites Gfx Variables===================================================================
        //35 valid values (0 to 34) adresses used 8E96CC to 8FD3FF
        public static int SpritesGfx_Address = 0x003252;//80B251         ;LDA $8FFF00,X    L[0x03]
        public static int SpritesGfx_Bank = 0x003257;//80B256            ;LDA $8FFF02,X    L[0x03]
        public static int SpritesGfx_Length = 0x00325D;//80B25C          ;LDA $8FFF03,X    L[0x03]
        //BG Level Selected Gfx Variables===========================================================
        //Databank is 0x83
        public static int BGGfx_Level_Ptr = 0x002020;//80A01F            ;LDA $868F,X      W[0x02]
        public static int BGGfx_Level2_Ptr = 0x00202B;//80A02A           ;CMP $868F,X      W[0x02]
        public static int BGGfx_Level3_Ptr = 0x002028;//80A027           ;LDA $8690,X      W[0x02]

        //BG Gfx Variables========================================================================
        //15 valid values (0 to 14)  adresses used 85C000 to 88FFFF
        //Bank is 0x83
        public static int BGGfxValuesAddrDest_1 = 0x0031E6;//80B1E5      ;LDA $8B7E,X      W[0x02]
        public static int BGGfxValuesAddrDest_2 = 0x0031F2;//80B1F1      ;LDA $8B7E,X      W[0x02]

        public static int BGGfxValuesPtr_1 = 0x0031FF;//80B1FE           ;LDA $8FFE00,X    L[0x03]
        public static int BGGfxValuesPtr_2 = 0x003204;//80B203           ;LDA $8FFE02,X    L[0x03]
        public static int BGGfxValuesPtr_3 = 0x003209;//80B209           ;LDA $8FFE03,X    L[0x03]

        //BG Gfx Pointers=========================================================================
        public static int BGGfx_LevelOffset = 0x002020;//80A01F          ;LDA $868F,X      W[0x02]
        public static int BGGfx_LevelOffset2 = 0x002028;//80A027         ;CMP $868F,X      W[0x02]
        public static int BGGfx_LevelOffsetA = 0x00202B;//80A02A         ;LDA $8690,X      W[0x02]
        public static int BGGfx_Length = 0x00320A;//80B209               ;LDA $8FFE03,X    L[0x03]

        //Palette Level 01 Ptr====================================================================
        //Bank is 0x83, return a pointer to dynamic pal [DES, SRC, LEN], SRC is 8AE400 + (SRC*32)
        public static int level01PalPtr = 0x002DEC;//80ADEB              ;LDX #$8A0B       W[0x02]
        public static int level02PalPtr = 0x002E11;//80AE10              ;LDX #$8A0E       W[0x02]
        public static int level03PalPtr = 0x002E29;//80AE28              ;LDX #$8A17       W[0x02]
        public static int level04PalPtr = 0x002E87;//80AE86              ;LDX #$8A23       W[0x02]
        public static int level05PalPtr = 0x002EA6;//80AEA5              ;LDX #$8A4D       W[0x02]


        public static int level02PalAlt = 0x018A56; //1 nibble per map if != 0 load 2 other pal
        public static int level03PalAlt = 0x018A5E; //1 nibble per map if != 0 load 2 other pal
        public static int level04PalAlt = 0x018A6B; //1 nibble per map if != 0 load 2 other pal
        public static int level05PalAlt = 0x018A80; //1 nibble per map if != 0 load 2 other pal
        public static int level04PalAltV = 0x018A7A; //use nibble as index for value in that table

        public static int splashscreenPalPtr = 0x002D88;//80AD87         ;ADC #$89CF       W[0x02]
        public static int sprBGPal = 0x0189D8;//8389D8                   ;ADC #$89CF       W[0x02]
        //Song Level Variables====================================================================
        public static int SongAddr = 0x00203F;//80A03E                   ;LDA $86A3,X      W[0x02]

        public static int Hooks_Level_Offset = 0x002BE8;//80ABE7         ;LDX $8959, Y     W[0x02]
        public static int HooksDataPtrs = 0x002C09; //80AC08   LDX $8977,Y 
        public static int HooksDataPtrs2 = 0x01202F; //82A02E   LDA $8977,X 



        //8977 = +0 
        public static int HooksDataPtrsP1 = 0x002C45; //80AC44  (+1)   80AC44   SBC $8978,X
        public static int HooksDataPtrsP1_2 = 0x002C4A; //80AC49  (+1) 80AC49   ADC $8978,X
        public static int HooksDataPtrsP2 = 0x002C56; //80AC55 (+2)    80AC55   SBC $8979,X
        public static int HooksDataPtrsP2_5 = 0x002C5B; //80AC5A (+2)  80AC5A   ADC $8979,X 
        public static int HooksDataPtrsP3 = 0x002C41; //80AC40  (+3)   80AC40   LDA $897A,X
        public static int HooksDataPtrsP4 = 0x002C52;//80AC51 (+4)     80AC51   LDA $897B,X

        public static int HooksDataPtrsP1_3 = 0x0120A8;//82A0A7  (+1)  82A0A7   SBC $8978,X
        public static int HooksDataPtrsP1_4 = 0x0120D0;//82A0CF  (+1)  82A0CF   SBC $8978,X

        public static int HooksDataPtrsP2_2 = 0x0120B2;//82A0B1  (+2)  82A0B1   SBC $8979,X
        public static int HooksDataPtrsP2_3 = 0x0120DA;//82A0D9  (+2)  82A0D9   SBC $8979,X

        public static int HooksDataPtrsP3_3 = 0x0120BC;//82A0BB (+3)   82A0BB   SBC $897A,X
        public static int HooksDataPtrsP3_4 = 0x0120E4;//82A0E3 (+3)   82A0E3   SBC $897A,X

        public static int HooksDataPtrsP4_2 = 0x0120C6;//82A0C5 (+4)   82A0C5   SBC $897B,X
        public static int HooksDataPtrsP4_3 = 0x0120EE;//82A0ED (+4)   82A0ED   SBC $897B,X

        public static int HooksDataPtrsP1_5 = 0x012103; //82A102 (+1)  82A102   SBC $8978,X 
        public static int HooksDataPtrsP2_4 = 0x01210C; //82A10B (+2)  82A10B   SBC $8979,X 
        public static int HooksDataPtrsP3_5 = 0x0120FF; //82A0FE (+3)  82A0FE   LDA $897A,X  
        public static int HooksDataPtrsP4_4 = 0x012108; //82A107 (+4)  82A107   LDA $897B,X  


        //public static int Hooks_Pointer_Data = 0x018977;
        public static int Planks_Level_Offset = 0x011D11;//829D10   ;LDX $B91B,Y     W[0x02]
        public static int PlankDataPtrs = 0x011D2F;//829D2E         ;LDX $B92B,Y     W[0x02]

        public static int PlankDataPtrs_1 = 0x011CBD; //2B
        public static int PlankDataPtrs_2 = 0x011D35; //2B
        public static int PlankDataPtrs_3 = 0x011CCE; //2B

        public static int PlankDataPtrsP1_1 = 0x011CD8; //2C
        public static int PlankDataPtrsP2_1 = 0x011CF8; //2D

        public static int PlankDataPtrsP4_1 = 0x011D06;  //2F

        public static int PlankDataPtrsP5_1 = 0x011CC3;  //30
        //public static int PlankDataPtrsP5_2 = 0x011CED;  //30
        public static int PlankDataPtrsP5_3 = 0x011D3B;  //30




        //011CBD
        //011CCE
        //011D2F
        //011D34




        public static int LockedDoors_Level_Offset = 0x011B7D; //829B7C   LDA $C461,X 
        public static int LockedDoor_Pointer_Data = 0x0144D7; 


        public static int Messages_Pointer = 0x05E81D;
        public static int Messages_Space = 0x5E881; //to 5F99D   111D size
        public static int Messages_Space_Limit = 0x5F99D;
        public static int Messages_ExtraSpace = 0x5E240; //to 5E800
        public static int Messages_ExtraSpace_Limit = 0x5E800;

        public static int PasswordData = 0x01C67F; //5 per level

        public static int StarBlocksPtrs = 0x01C2DD;


        //Tiles Related Data NO POINTERS
        //This contains all tiles32 in order from tile16 (upleft,upright,botleft,botright)
        public static int Tiles32Data = 0x050000;
        //(2 bytes per tiles instead of 4) with a mask on #$0F, #$F0 >> 4
        public static int Tiles32Ext = 0x054000;

        public static int Tiles32Data2 = 0x90000;
        public static int Tiles32Ext2 = 0x94000;

        public static int EditorVersion = 0x007FAF;

        //This contains all tiles16 in order from tile8 (upleft,upright,botleft,botright)
        public static int Tiles16Data = 0x058000; //3327
        public static int Tiles16DataExt = 0x0A0000; //4096

        public static int Tile16CollisionMap = 0x04F100;


        //Map Related Data
        //Each map pointers is in that
        public static int Map32_Level_Ptr = 0x018CE7; // contains level offsets map data

        public static int Map32Data = 0x48000;
        public static int Map32Ext = 0x4B700;


        public static int paletteCapcomLogoPtr = 0x02DCC; // 80ADCB   LDX #$89C3
        public static int paletteSplashScreen  = 0x02D88; // 80AD87   ADC #$89CF
        public static int paletteTitleScreen = 0x02DB9;// 80ADB8   LDX #$89D2 
        public static int paletteTitleScreen2 = 0x02DC4;// 80ADC3   LDX #$89D2 

        public static int darkiceValues = 0x04F055;

        public static int StarTilesData = 0x01C2DD;
        public static int StarTilesDataLimit = 0x01C3BC;
        public static int StarTilesData_Extra = 0x1FAE1;

        public static int fireballShootingDoor = 0x0124E6;


        public static int enemyDoor1DataStartCR = 0x01C281;
        public static int enemyDoor1DataStartAddrType = 0x01C28D;

        public static int enemyDoor2DataStartCD = 0x01C2BF;
        public static int enemyDoor2DataStartAddrType = 0x01C2CB;
    }
}
