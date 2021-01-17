using System;
using System.Collections;

using System.IO;

namespace HammingCoderVol2
{
    class Program
    {
      static  BitArray messageArray1;
        static BitArray messageDeCoded;
        static int dovj;
        static int pochatok;
        static BitArray ConvertFileToBitArray(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            BitArray messageArray = new BitArray(fileBytes);


            return messageArray;
        }
        static BitArray Convert1FileToBitArray(string path1)
        {
            byte[] fileBytes = File.ReadAllBytes(path1);
            BitArray messageArray1 = new BitArray(fileBytes);


            return messageArray1;
        }

        static void WriteBitArrayToFile(string fileName, BitArray bitArray)
        {
            byte[] bytes = new byte[bitArray.Length / 8 + (bitArray.Length % 8 == 0 ? 0 : 1)];
            bitArray.CopyTo(bytes, 0);

            string path = Directory.GetCurrentDirectory() + "\\" + fileName;

            //File.Create(path);
            using (FileStream fs = File.Create(path))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        static BitArray MyCoding(BitArray messageArray)
        {
            int countBits = messageArray.Count; // кількість біт в масиві
            pochatok = messageArray.Count;
            dovj = countBits;
            int newBits = (int)Math.Ceiling(countBits / 5.0) * 4;

            int lastBits = countBits + newBits;
            for (int d = 0; d < 100; d++)
            {
                if (lastBits % 9 == 0)
                {
                    break;
                }
                else
                {
                    lastBits++;
                }
            }
            BitArray resultat = new BitArray(lastBits);
            BitArray messageCoded = new BitArray(lastBits); // новий пустий масив біт
            for (int i = 0; i < countBits; i += 5)
            {


                BitArray pol = new BitArray(5);

                for (int j = 0; j < 5; j++)
                {
                    if (j + i >= countBits)
                    {
                        pol[j] = false;
                    }
                    else
                    {
                        pol[j] = messageArray[j + i];
                    }
                }

                for (int a = 0; a < 1; a++)
                {
                    resultat[2] = pol[4];
                    resultat[4] = pol[3];
                    resultat[5] = pol[2];
                    resultat[6] = pol[1];
                    resultat[8] = pol[0];
                }
                BitArray pol1 = new BitArray(9);
                for (int a = 0; a < 1; a++)
                {
                    pol1[0] = false;
                    pol1[1] = false;
                    pol1[2] = pol[4];
                    pol1[3] = false;
                    pol1[4] = pol[3];
                    pol1[5] = pol[2];
                    pol1[6] = pol[1];
                    pol1[7] = false;
                    pol1[8] = pol[0];
                }
                BitArray m1 = new BitArray(9);     //(1, 0, 1, 0, 1, 0, 1, 0, 1);
                m1[0] = true; m1[1] = false; m1[2] = true; m1[3] = false; m1[4] = true; m1[5] = false; m1[6] = true; m1[7] = false; m1[8] = true;
                BitArray m2 = new BitArray(9);       //(0, 1, 1, 0, 0, 1, 1, 0, 0);
                m2[0] = false; m2[1] = true; m2[2] = true; m2[3] = false; m2[4] = false; m2[5] = true; m2[6] = true; m2[7] = false; m2[8] = false;
                BitArray m3 = new BitArray(9);       //(0, 0, 0, 1, 1, 1, 1, 0, 0);
                m3[0] = false; m3[1] = false; m3[2] = false; m3[3] = true; m3[4] = true; m3[5] = true; m3[6] = true; m3[7] = false; m3[8] = false;
                BitArray m4 = new BitArray(9);      //(0, 0, 0, 0, 0, 0, 0, 1, 1);
                m4[0] = false; m4[1] = false; m4[2] = false; m4[3] = false; m4[4] = false; m4[5] = false; m4[6] = false; m4[7] = true; m4[8] = true;
                BitArray sohran = new BitArray(4);
                int provirka = 0;
                int provirka1 = 0;
                int provirka2 = 0;
                int provirka3 = 0;
                for (int k = 0; k < 4; k++)
                {
                    if (k == 0)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if (pol1[l] & m1[l] == true)
                            {
                                provirka++;
                            }
                            else { }
                        }
                    }
                    if (k == 1)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if (pol1[l] & m2[l] == true)
                            {
                                provirka1++;
                            }
                            else { }
                        }
                    }
                    if (k == 2)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if (pol1[l] & m3[l] == true)
                            {
                                provirka2++;
                            }
                            else { }
                        }
                    }
                    if (k == 3)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if (pol1[l] & m4[l] == true)
                            {
                                provirka3++;
                            }
                            else { }
                        }
                    }
                }
                if (provirka % 2 == 0)
                {
                    resultat[0] = false;
                }
                else { resultat[0] = true; }
                if (provirka1 % 2 == 0)
                {
                    resultat[1] = false;
                }
                else { resultat[1] = true; }
                if (provirka2 % 2 == 0)
                {
                    resultat[3] = false;
                }
                else { resultat[3] = true; }
                if (provirka3 % 2 == 0)
                {
                    resultat[7] = false;
                }
                else { resultat[7] = true; }




                for (int k = 0; k < 9; k++)
                {
                    messageCoded[k + (9 * (i / 5))] = resultat[k];
                }

            }

            return messageCoded;
        }

        static BitArray MyDeCoding(BitArray messageArray1)
        {
            int countBits = messageArray1.Count; // кількість біт в масиві
            BitArray messageDeCoded = new BitArray(dovj); // новий пустий масив біт
            int schet = countBits;
            int a = 8;
            int b = 6;
            int r = 5;
            int c = 4;
            int m = 2;
            for (int i = 0; i< schet ; i+=5)
            {
                if (i + 3 >= dovj || i + 4 >= dovj || i + 2 >= dovj || i + 1 >= dovj || i  >= dovj) { break; }
                messageDeCoded[i] = messageArray1[a];
                messageDeCoded[i+1] = messageArray1[b];
                messageDeCoded[i+2] = messageArray1[r];
                messageDeCoded[i+3] = messageArray1[c];
                messageDeCoded[i+4] = messageArray1[m];
                 a += 9;
                 b += 9;
                 r += 9;
                 c += 9;
                 m += 9;
            }
            return messageDeCoded;
        }

            static void Main(string[] args)
        {
            string choice = null;

            while (choice != "3")
            {
                Console.Write("4. Закодировать файл\n5. Декодировать файл\n");
                choice = Console.ReadLine();

                if (choice == "4")
                {
                    //File.Create(Directory.GetCurrentDirectory() + "\\test1.txt");
                    string path1 = Directory.GetCurrentDirectory() + "\\test1.txt";
                    if (File.Exists(path1) == false) return; // перевірка на наявність файла
                    BitArray messageArray = ConvertFileToBitArray(path1); // читаємо файл і записуємо у BitArray
                    BitArray messageCoded = MyCoding(messageArray); // кодуємо bitArray

                    WriteBitArrayToFile("test2.txt", messageCoded); // записуємо bitArray у файл
                }
                if (choice == "5")
                {
                        string path1 = Directory.GetCurrentDirectory() + "\\test2.txt";
                        if (File.Exists(path1) == false) return;
                        BitArray messageArray1 = Convert1FileToBitArray(path1);
                        BitArray messageDeCoded = MyDeCoding(messageArray1);

                        WriteBitArrayToFile("test3.txt", messageDeCoded);
                    }

                if (choice == "3") return;

                Console.WriteLine();
            }
        }
    }
}