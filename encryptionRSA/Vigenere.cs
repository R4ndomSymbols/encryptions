using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    internal class Vigenere : Encryption
    {
        const string alf = "аАбБвВгГдДеЕёЁжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩъЪыЫьЬэЭюЮяЯ";
        string key = string.Empty;
        string text = string.Empty;
        int indexInKey = -1; // индекс текущего символа ключа
        // пересобираем строку, выполняя операцию шифрования/дешифрования для символов входящих в алфавит (alf)
        public override string Decrypt(string toDecrypt)
        {

            return new string(text.Select(s => alf.Contains(s) ? DecryptSymbol(s) : s).ToArray());

            // расшифровать символ: (t + N - k) % N
            char DecryptSymbol(char symbol)
            {
                return alf[(alf.IndexOf(symbol) + alf.Length - alf.IndexOf(NextKeySymbol())) % alf.Length];
            }
        }

        public override string Encrypt(string toEncrypt)
        {
            return new string(text.Select(s => alf.Contains(s) ? EncryptSymbol(s) : s).ToArray());
            // зашифровать символ: (t + k) % N
            char EncryptSymbol(char symbol)
            {
                return alf[(alf.IndexOf(symbol) + alf.IndexOf(NextKeySymbol())) % alf.Length];
            }
        }
        // получить следующий символ ключа
        char NextKeySymbol()
        {
            return key[++indexInKey < key.Length ? indexInKey : indexInKey = 0];
        }

        public override void Introduce()
        {
            //while (true)
            //{
                Console.Clear();
                Console.Write("Введите текст: ");
                text = ReadText();
                Console.Write("Введите ключ: ");
                key = ReadText();

                if (CheckKeyString(key))
                {
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1 - Шифровать текст.");
                    Console.WriteLine("2 - Дешифровать текст.");
                    Console.WriteLine("ESC - Завершить программу.");

                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            Console.WriteLine("\r\nРезультат шифрования: "+ Encrypt(text));
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            Console.WriteLine("\r\nРезультат дешифрования: " + Decrypt(text));
                            break;
                        case ConsoleKey.Escape:
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("\r\nВы ввели некорректный ключ!");
                }
            //}

            string ReadText()
            {
                return (Console.ReadLine() + "").ToLower();
            }
            bool CheckKeyString(string key)
            {
                return !string.IsNullOrWhiteSpace(key) && key.All(symbol => alf.Contains(symbol));
            }
        }
    }
}
