using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoofED
{
    public static class Compression
    {
        private static int offset = 0;
        private static byte dB = 0; //data byte
        private static byte hB = 0;//header byte
        private static List<byte> copiedBytes = new List<byte>();
        private static int location = 0;
        public static byte[] DecompressGFX(byte[] ROMdata, int romPos, int length)
        {
            offset = 0;
            copiedBytes.Clear();
            location = romPos;
            //DECOMP SYSTEM
            while (copiedBytes.Count < length)
            {
                hB = ROMdata[location + offset];
                offset++;
                dB = ROMdata[location + offset];

                {
                    offset++;
                    byte b1 = (byte)((hB & 0xF0) >> 4);
                    byte b2 = (byte)((hB & 0x0F));

                    switch (b1)
                    {
                        case 0:
                            decomp00(ROMdata);
                            break;
                        case 1:
                            decomp01(ROMdata);
                            break;
                        case 2:
                            decomp02(ROMdata);
                            break;
                        case 3:
                            decomp03(ROMdata);
                            break;
                        case 4:
                            decomp04(ROMdata);
                            break;
                        case 5:
                            decomp05(ROMdata);
                            break;
                        case 6:
                            decomp06(ROMdata);
                            break;
                        case 7:
                            decomp07(ROMdata);
                            break;
                        case 8:
                            decomp08(ROMdata);
                            break;
                        case 9:
                            decomp09(ROMdata);
                            break;
                        case 10:
                            decomp10(ROMdata);
                            break;
                        case 11:
                            decomp11(ROMdata);
                            break;
                        case 12:
                            decomp12(ROMdata);
                            break;
                        case 13:
                            decomp13(ROMdata);
                            break;
                        case 14:
                            decomp14(ROMdata);
                            break;
                        case 15:
                            decomp15(ROMdata);
                            break;
                    }

                    switch (b2)
                    {
                        case 0:
                            decomp00(ROMdata);
                            break;
                        case 1:
                            decomp01(ROMdata);
                            break;
                        case 2:
                            decomp02(ROMdata);
                            break;
                        case 3:
                            decomp03(ROMdata);
                            break;
                        case 4:
                            decomp04(ROMdata);
                            break;
                        case 5:
                            decomp05(ROMdata);
                            break;
                        case 6:
                            decomp06(ROMdata);
                            break;
                        case 7:
                            decomp07(ROMdata);
                            break;
                        case 8:
                            decomp08(ROMdata);
                            break;
                        case 9:
                            decomp09(ROMdata);
                            break;
                        case 10:
                            decomp10(ROMdata);
                            break;
                        case 11:
                            decomp11(ROMdata);
                            break;
                        case 12:
                            decomp12(ROMdata);
                            break;
                        case 13:
                            decomp13(ROMdata);
                            break;
                        case 14:
                            decomp14(ROMdata);
                            break;
                        case 15:
                            decomp15(ROMdata);
                            break;
                    }
                }
            }
            return copiedBytes.ToArray();
        }

        //DataByte is always the byte after the header byte no matter where offset is
        private static void decomp00(byte[] ROMdata)//verified
        {
            //[b1, b1, b1, b1]
            for (int i = 0; i < 4; i++)
            {
                copiedBytes.Add(dB); //add 4 time the DataByte
            }
        }
        private static void decomp01(byte[] ROMdata)//verified
        {
            //[b1, b1, b1, b2]
            copiedBytes.Add(dB);
            copiedBytes.Add(dB);
            copiedBytes.Add(dB);
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;


        }
        private static void decomp02(byte[] ROMdata)//verified
        {
            //[b1, b1, b2, b1]
            copiedBytes.Add(dB);
            copiedBytes.Add(dB);
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);

        }
        private static void decomp03(byte[] ROMdata)//verified
        {
            //[b1, b1, b2, b3]
            copiedBytes.Add(dB);
            copiedBytes.Add(dB);
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

        }
        private static void decomp04(byte[] ROMdata)//verified
        {

            //[b1, b2, b1, b1]
            copiedBytes.Add(dB);
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

            copiedBytes.Add(dB);
            copiedBytes.Add(dB);
        }
        private static void decomp05(byte[] ROMdata)//verified
        {
            //[b1, b2, b1, b3]
            copiedBytes.Add(dB);
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

            copiedBytes.Add(dB);
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

        }
        private static void decomp06(byte[] ROMdata)//verified
        {
            //[b1, b2, b3, b1]
            copiedBytes.Add(dB);
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);
        }
        private static void decomp07(byte[] ROMdata)//verified
        {
            //[b1, b2, b3, b4]
            copiedBytes.Add(dB);

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
        }
        private static void decomp08(byte[] ROMdata)//verified
        {
            //[b2, b1, b1, b1]
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);
            copiedBytes.Add(dB);
            copiedBytes.Add(dB);


        }
        private static void decomp09(byte[] ROMdata)//verified
        {

            //[b1, b1, b2, b3] WRONG
            //[b2, b1, b1, b3] 
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);
            copiedBytes.Add(dB);
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
        }
        private static void decomp10(byte[] ROMdata)//verified
        {
            //[b2, b1, b3, b1]

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);
        }
        private static void decomp11(byte[] ROMdata)//verified
        {
            //[b2, b1, b3, b4]

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

        }
        private static void decomp12(byte[] ROMdata)//verified
        {

            //[b2, b3, b1, b1]

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);
            copiedBytes.Add(dB);
        }
        private static void decomp13(byte[] ROMdata)//verified
        {
            //[b2, b3, b1, b4]

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
        }
        private static void decomp14(byte[] ROMdata)//verified
        {
            //[b2, b3, b4, b1]

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;

            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(dB);
        }
        private static void decomp15(byte[] ROMdata)//verified
        {
            //[b1, b2, b3, b4] ;b1 here is not dB
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
            copiedBytes.Add(ROMdata[location + offset]);
            offset += 1;
        }



        public static byte[] CompressGfx(byte[] data)
        {


            int pos = 0;
            int nPos = pos;
            List<byte> cTemp = new List<byte>();
            List<byte> cData = new List<byte>();
            byte[] mask = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            while (pos < data.Length)
            {
                byte db = data[pos];
                byte hb = 0;
                

                byte[] tempDataArray = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    tempDataArray[i] = data[pos+i];
                }
                int highest = -1;
                for (int i = 0; i < 8; i++)
                {
                    if (tempDataArray.Where(x => x.Equals(tempDataArray[i])).Count() > 1)
                    {
                        if (highest < tempDataArray.Where(x => x.Equals(tempDataArray[i])).Count())
                        {
                            db = tempDataArray[i];
                            highest = tempDataArray.Where(x => x.Equals(tempDataArray[i])).Count();
                        }

                        
                    }
                }
                
                cTemp.Add(db);

                for (int i = 0; i < 8; i++)
                {
                    if (data[pos + i] != db)
                    { 
                        hb |= mask[i];
                        cTemp.Add(data[pos+i]);
                    }
                }

                cData.Add((byte)hb);
                cData.AddRange(cTemp);
                cTemp.Clear();
                pos += 8;

            }


            
            
            return cData.ToArray();

        }

        



    }
}
