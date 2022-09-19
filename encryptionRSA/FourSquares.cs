using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace encryption
{
    internal class FourSquares : Encryption
    {
        private char[][] fourSquares = new char[12].Select(x => new char[12]).ToArray();
        private char _startSymbol = (char)int.Parse("0410", NumberStyles.HexNumber);
        private char _endSymbol = (char)int.Parse("042F", NumberStyles.HexNumber);
        private List<char> alphabet;
        private (int, int) sq1 = (0, 0); // x y
        private (int, int) sq2 = (6, 0);
        private (int, int) sq3 = (0, 6);
        private (int, int) sq4 = (6, 6);

        public FourSquares()
        {
            alphabet = new List<char>();
        }
        public override string Decrypt(string toDecrypt)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < toDecrypt.Length; i += 2)
            {
                result.Append(Getpair((toDecrypt[i], toDecrypt[i + 1]), (sq2, sq3)));
            }
            return result.ToString();
        }

        public override string Encrypt(string toEncrypt)
        {
            string corrected = toEncrypt.ToUpper() + (toEncrypt.Length % 2 != 0 ? "А" : ""); 
            StringBuilder result = new StringBuilder();
            for(int i = 0; i<corrected.Length; i += 2)
            {
                result.Append(Getpair((corrected[i], corrected[i + 1]), (sq1, sq4)));
            }
            return result.ToString();
        }

        public override void Introduce()
        {
            AddAlphabetSquare(sq1);
            AddAlphabetSquare(sq4);

            Console.WriteLine("Введите текст для шифрования");

            var t = Console.ReadLine() ?? "ПРИВЕТ";

            Console.WriteLine("Введите два ключа шифрования через пробел");

            var keys = (Console.ReadLine() ?? "кодовое слово").Split(" ");

            AddCodeSquare(sq2, keys[0]);
            AddCodeSquare(sq3, keys[1]);

            Console.WriteLine("Шифрованный текст");

            var enc = Encrypt(t);

            Console.WriteLine(enc);

            Console.WriteLine("Дешифрованное сообщение");
            
            Console.WriteLine(Decrypt(enc));
        }

        private void AddAlphabetSquare((int, int) startPoint) // x y
        {
            char temp = _startSymbol;
            for (int i = startPoint.Item1; i < startPoint.Item1 + 6; i++)
            {
                for (int j = startPoint.Item2; j < startPoint.Item2 + 6; j++)
                {
                    if (temp <= _endSymbol)
                    {
                        fourSquares[i][j] = temp;
                        if (alphabet.All(x => x != temp))
                        {
                            alphabet.Add(temp);
                        }
                        temp++;
                    }
                    else
                    {
                        fourSquares[i][j] = ' ';
                        alphabet.Add(' ');
                        break;
                    }
                }
            }
        }
        private void AddCodeSquare((int, int) startPoint, string word) // x y
        {
            int index = 0;
            string correctWord = word.ToUpper();
            char temp = _startSymbol;
            for (int i = startPoint.Item1; i < startPoint.Item1 + 6; i++)
            {
                for (int j = startPoint.Item2; j < startPoint.Item2 + 6; j++)
                {
                    if (index < correctWord.Length)
                    {
                        fourSquares[i][j] = correctWord[index];
                        index++;
                    }
                    else if (temp <= _endSymbol)
                    {
                        if (correctWord.Contains(temp))
                        {
                            j--;
                        }
                        else
                        {
                            fourSquares[i][j] = temp;
                        }
                        temp++;
                    }
                    else
                    {
                        temp = _startSymbol;
                        fourSquares[i][j] = temp++;
                    }
                }
            }

        }

        private string Getpair((char,char) pair, ((int, int), (int, int)) squaresPair)
        {
            (int, int) first = FindInSquare(squaresPair.Item1, pair.Item1);
            (int , int) second = FindInSquare(squaresPair.Item2, pair.Item2);
            return fourSquares[second.Item1][first.Item2].ToString() + fourSquares[first.Item1][second.Item2].ToString();
        }

        private (int,int) FindInSquare((int, int) startPoint, char c)
        {
            for (int i = startPoint.Item1; i < startPoint.Item1 + 6; i++)
            {
                for (int j = startPoint.Item2; j < startPoint.Item2 + 6; j++)
                {
                    if (c == fourSquares[i][j])
                    {
                        return (i,j);
                    }
                }
            }
            return (-1, -1);
        }


    }
}



        /*class Program
        {
            static void Main(string[] args)
            {
                char[,] m1 = new char[6, 6];
                char[,] m2 = new char[6, 6];
                char[,] m3 = new char[6, 6];
                char[,] m4 = new char[6, 6];
                int count = 0;

                char[] alp = new string("АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ").ToCharArray();

                for (int i = 0; i < m1.GetLength(0); i++)
                {
                    for (int j = 0; j < m1.GetLength(1); j++)
                    {
                        if (count < 33)
                        {
                            m1[i, j] = alp[count];
                            count++;
                        }
                    }

                }
                m2 = m1;
                string key1 = Console.ReadLine().ToUpper();
                char[] z1 = new string(key1 + "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ").ToCharArray();

                count = 0;
                for (int i = 0; i < m3.GetLength(0); i++)
                {

                    for (int j = 0; j < m3.GetLength(1); j++)
                    {
                        if (count < 36)
                        {
                            m3[i, j] = z1[count];
                            count++;
                        }

                    }
                }
                string key2 = Console.ReadLine().ToUpper();
                char[] z2 = new string(key2 + "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ").ToCharArray();

                count = 0;
                for (int i = 0; i < m4.GetLength(0); i++)
                {

                    for (int j = 0; j < m4.GetLength(1); j++)
                    {
                        if (count < 36)
                        {
                            m4[i, j] = z2[count];
                            count++;
                        }

                    }
                }
                /*for (int i = 0; i < m3.GetLength(0); i++)
                {
                    for (int j = 0; j < m3.GetLength(1); j++)
                    {
                        Console.Write(m3[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
                for (int i = 0; i < m4.GetLength(0); i++)
                {
                    for (int j = 0; j < m4.GetLength(1); j++)
                    {
                        Console.Write(m4[i, j] + "\t");
                    }
                    Console.WriteLine();*/

               