using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Защита_информации.Лаб.раб._4
{
    // Простейшие симметричные алгоритмы шифрования.
    // Реализация алгоритма шифрования простой перестановки с ключом K = 632514.
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("Алгоритм шифрования <Простая перестановка>" + '\n');
            Console.Write("Введите сообщение для шифрования: ");
            string message = Console.ReadLine();
            int[] key = { 6, 3, 2, 5, 1, 4 };
            Console.WriteLine('\n');

            // Шифрование
            List<string> messageBlocks =  Divider(message, key.Length);
            string outputMessage = null;
            for (int i = 0; i < messageBlocks.Count; i++)
            {
                string[] charArray = messageBlocks[i].Select(ch => ch.ToString()).ToArray();
                string str = null;
                for(int j = 0; j < key.Length; j++)
                {
                    int index = key[j] - 1;
                    str += charArray[index];
                }
                outputMessage += str;
            }
            Console.Write("Результат шифрования: " + outputMessage + '\n');
            Console.WriteLine('\n');

            // Дешифрование
            List<string> encodedMessageBlocks = Divider(outputMessage, key.Length);
            string originalMessage = null;
            for (int i = 0; i < encodedMessageBlocks.Count; i++)
            {
                string[] charArray = encodedMessageBlocks[i].Select(ch => ch.ToString()).ToArray();
                string str = null;
                for (int j = 1; j <= key.Length; j++)
                {
                    int index = Array.IndexOf(key, j);
                    str += charArray[index];
                }
                originalMessage += str;
            }
            Console.Write("Результат дешифрования: " + originalMessage);

            Console.ReadKey();
        }

        // Метод разделения сообщения на блоки
        public static List<string> Divider(string str, int blockLength) 
        {
            List<string> Blocks = new List<string> (str.Length / blockLength + 1);
            for (int i = 0; i < str.Length; i += blockLength)
            {
                if (str.Length - i > blockLength)
                    Blocks.Add(str.Substring(i, blockLength));
                else
                    Blocks.Add(str.Substring(i, str.Length - i) + new String('\0', blockLength - (str.Length - i)));
            }
            return Blocks;
        }
    }
}
