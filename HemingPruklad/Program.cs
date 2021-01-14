using System;
using System.Collections;

using System.IO;

namespace HammingCoderVol2
{
    class Program
    {
        static BitArray ConvertFileToBitArray(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            BitArray messageArray = new BitArray(fileBytes);

            return messageArray;
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
            BitArray messageCoded = new BitArray(countBits, false); // новий пустий масив біт

            //for (int i = 0; i < countBits; i+=2) // якесь кодування (це заміни на те що потрібно)
            //{
            //    messageCoded[i] = messageArray[i] ^ true;
            //    messageCoded[i+1] = messageArray[i+1] ^ false;
            //}
            for (int i = 0; i < countBits; i++)
            {
                if (messageArray[i] == true)
                    messageCoded[i] = true;
                else
                    messageCoded[i] = false;
            }
            int messageInd = 0;
            int retInd = 0;
            int controlIndex = 1;
            var retArray = new BitArray(messageCoded.Length + 1 + (int)Math.Ceiling(Math.Log(messageCoded.Length, 2)));
            while (messageInd < messageCoded.Length)
            {
                if (retInd + 1 == controlIndex)
                {
                    retInd++;
                    controlIndex = controlIndex * 2;
                    continue;
                }
                retArray.Set(retInd, messageCoded.Get(messageInd));
                messageInd++;
                retInd++;
            }
            retInd = 0;
            controlIndex = 1 << (int)Math.Log(retArray.Length, 2);
            while (controlIndex > 0)
            {
                int c = controlIndex - 1;
                int counter = 0;

                while (c < retArray.Length)
                {
                    for (int i = 0; i < controlIndex && c < retArray.Length; i++)
                    {
                        if (retArray.Get(c))
                            counter++;
                        c++;
                    }
                    c += controlIndex;
                }

                if (counter % 2 != 0) retArray.Set(controlIndex - 1, true);
                controlIndex = controlIndex / 2;
            }
            return retArray;
            //return messageCoded;
        }
        static BitArray MyDeCoding(BitArray messageArray)
        {
            int countBits = messageArray.Count; // кількість біт в масиві
            BitArray messageCoded = new BitArray(countBits, false); // новий пустий масив біт

            for (int i = 0; i < countBits; i++)
            {
                if (messageArray[i] == true)
                    messageCoded[i] = true;
                else
                    messageCoded[i] = false;
            }
            var decodedArray = new BitArray((int)(messageCoded.Count - Math.Ceiling(Math.Log(messageCoded.Count, 2))), false);
            int count = 0;
            for (int i = 0; i < messageCoded.Length; i++)
            {
                for (int j = 0; j < Math.Ceiling(Math.Log(messageCoded.Count, 2)); j++)
                {
                    if (i == Math.Pow(2, j) - 1)
                        i++;
                }
                decodedArray[count] = messageCoded[i];
                count++;
            }
            var strDecodedArray = "";
            for (int i = 0; i < decodedArray.Length; i++)
            {
                if (decodedArray[i])
                    strDecodedArray += "1";
                else
                    strDecodedArray += "0";
            }
            var checkArray = MyCoding(strDecodedArray);
            byte[] failBits = new byte[checkArray.Length - decodedArray.Length];
            count = 0;
            bool isMistake = false;
            for (int i = 0; i < checkArray.Length - decodedArray.Length; i++)
            {
                if (messageCoded[(int)Math.Pow(2, i) - 1] != checkArray[(int)Math.Pow(2, i) - 1])
                {
                    failBits[count] = (byte)(Math.Pow(2, i));
                    count++;
                    isMistake = true;
                }
            }
            if (isMistake)
            {
                int mistakeIndex = 0;
                for (int i = 0; i < failBits.Length; i++)
                    mistakeIndex += failBits[i];
                mistakeIndex--;
                messageCoded.Set(mistakeIndex, !messageCoded[mistakeIndex]);
                Console.WriteLine($"Ошибка в бите №{mistakeIndex}");
                count = 0;
                for (int i = 0; i < messageCoded.Length; i++)
                {
                    for (int j = 0; j < Math.Ceiling(Math.Log(messageCoded.Count, 2)); j++)
                    {
                        if (i == Math.Pow(2, j) - 1)
                            i++;
                    }
                    decodedArray[count] = messageCoded[i];
                    count++;
                }
            }
            return decodedArray;
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
                    BitArray messageArray = ConvertFileToBitArray(path1); 
                    BitArray messageCoded = MyDeCoding(messageArray); 

                    WriteBitArrayToFile("test3.txt", messageCoded); 
                }

                if (choice == "3") return;

                Console.WriteLine();
            }
        }
    }
}