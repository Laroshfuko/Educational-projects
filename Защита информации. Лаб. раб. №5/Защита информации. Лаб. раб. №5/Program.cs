using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Защита_информации.Лаб.раб._5
{
    class Program
    {
        // Реализая метода криптоанализа зашифрованных сообщений, 
        // основанного на анализе частотности символов.
        static void Main(string[] args)
        {
            string path = "C:\\Users\\Petr\\Desktop\\Open text.txt";
            string readAllFile = File.ReadAllText(path,Encoding.Default);

            // Считывание из файла Open text (открытый текст)
            ArrayList arrText = new ArrayList();
            using (StreamReader reader = new StreamReader("C:\\Users\\Petr\\Desktop\\Open text.txt", Encoding.Default))
            {
                string sLine = "";
                while (sLine != null)
                {
                    sLine = reader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
            }
                  
            // Подсчет символов в открытом тексте
            Dictionary<char, int> firstDictionary = new Dictionary<char, int>();
            int newLineCount = 0;
            foreach (string s in arrText)
            {
                var res = s
                 .GroupBy(c => c)
                 .ToDictionary(g => g.Key, g => g.Count());
                 foreach (char c in res.Keys)
                    if (firstDictionary.ContainsKey(c))
                    {
                        firstDictionary[c] += res[c];
                    }
                    else
                    {
                        int myValue = res[c];
                        firstDictionary.Add(c, myValue);
                    }

             // Подсчёт переноса строки и возврата каретки
                    char firstChar = (char)10;
                    char secondChar = (char)13;
                    newLineCount++;
                    if (newLineCount == 1)
                    {
                        firstDictionary.Add(firstChar, 1);
                        firstDictionary.Add(secondChar, 1);
                        firstDictionary[firstChar] += newLineCount;
                        firstDictionary[secondChar] += newLineCount;
                    }
                    else
                    {
                        firstDictionary[firstChar] = newLineCount;
                        firstDictionary[secondChar] = newLineCount;
                    }
            }

            //Сортировка firstDictionary
            firstDictionary = firstDictionary.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            char[] firstDictionaryIndex = firstDictionary.Keys.ToArray();
    
            // Частота встречаемости
            Dictionary<char, double> firstDictionaryFrequency = new Dictionary<char, double>();
            int sum = 0;
            foreach (int i in firstDictionary.Values)
                sum += i;
            foreach(char c in firstDictionary.Keys)
            {
                double temp = 0;
                double procent = 0;
                temp = firstDictionary[c];
                procent = (temp / sum) * 100;
                firstDictionaryFrequency[c] = procent;
            }

            // Считывание из файла encrypt4.txt (зашифрованный текст)
            ArrayList secondArrText = new ArrayList();
            using (StreamReader reader = new StreamReader("C:\\Users\\Petr\\Desktop\\encrypt4.txt", Encoding.Default))
            {
                string sLine = "";
                while (sLine != null)
                {
                    sLine = reader.ReadLine();
                    if (sLine != null)
                            secondArrText.Add(sLine);
                }
            }

            // Подсчет символов в зашифрованном тексте
            Dictionary<char, int> secondDictionary = new Dictionary<char, int>();
            newLineCount = 0;
            foreach (string s in secondArrText)
            {
                var res = s
                 .GroupBy(c => c)
                 .ToDictionary(g => g.Key, g => g.Count());
                foreach (char c in res.Keys)
                    if (secondDictionary.ContainsKey(c))
                    {
                        secondDictionary[c] += res[c];
                    }
                    else
                    {
                        int myValue = res[c];
                        secondDictionary.Add(c, myValue);
                    }

                // Подсчёт переноса строки и возврата каретки
                char firstChar = (char)10;
                char secondChar = (char)13;
                newLineCount++;
                if (newLineCount == 1)
                {
                    secondDictionary.Add(firstChar, 1);
                    secondDictionary.Add(secondChar, 1);
                    secondDictionary[firstChar] += newLineCount;
                    secondDictionary[secondChar] += newLineCount;
                }
                else
                {
                    secondDictionary[firstChar] = newLineCount;
                    secondDictionary[secondChar] = newLineCount;
                }
            }

            //Сортировка  secondDictionary
            secondDictionary = secondDictionary.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            char[] secondDictionaryIndex = secondDictionary.Keys.ToArray();

            // Частота встречаемости
            Dictionary<char, double> secondDictionaryFrequency = new Dictionary<char, double>();
            sum = 0;
            foreach (int i in secondDictionary.Values)
                sum += i;
            foreach (char c in secondDictionary.Keys)
            {
                double temp = 0;
                double procent = 0;
                temp = secondDictionary[c];
                procent = (temp / sum) * 100;
                secondDictionaryFrequency[c] = procent;
            }

        // Расшифровка текста 
        Console.WriteLine("Расшифрованный текст \n");
        ArrayList decryptText = new ArrayList();
        foreach (string s in secondArrText)
        {
            string str = null;
            char[] ch = s.ToCharArray();
            foreach (char c in ch)
            {
                    for (int i = 0; i < secondDictionaryIndex.Length; i++)
                        if (secondDictionaryIndex[i].Equals(c))
                        {
                            str += firstDictionaryIndex[i];
                            break;
                        }
            }
            decryptText.Add(str);
        }

        // Запись в файл
        using (StreamWriter writer = File.CreateText("C:\\Users\\Petr\\Desktop\\Output text.txt"))
        {
                foreach (string s in decryptText)
                {
                    writer.WriteLine(s);
                    Console.WriteLine(s);
                }
        }
            Console.ReadKey();
        }
    }
}
