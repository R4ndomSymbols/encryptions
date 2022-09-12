using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace encryption
{
    internal class Rails : Encryption
    {
        private string _text = string.Empty;
        private int _lineCount = 0;
        private int _columnsCount = 0;
        public override string Decrypt(string toDecrypt)
        {

            _lineCount = _text.Count(f => f == ' ') + 1;
            string[] words = _text.Split(new char[] { ' ' });
            int strIndex = 0, direction = 1;
            string processingWord = words[strIndex];
            Console.Write(words[0][0]);
            words[strIndex] = processingWord.Remove(0, 1);

            StringBuilder result = new StringBuilder();

            for (int j = 0; j < _text.Length - _lineCount; j++)
            {
                strIndex += direction;
                processingWord = words[strIndex];
                //processingWord.Remove(0, 1);
                words[strIndex] = processingWord.Remove(0, 1);
                result.Append(processingWord[0]);
                if (strIndex == _lineCount - 1 || strIndex == 0)
                {
                    direction *= -1;
                }
            }
            return result.ToString();
        }

        public override string Encrypt(string toEncrypt)
        {
            Console.WriteLine("Введите количество строк");
            _lineCount = Convert.ToInt32(Console.ReadLine());
            string correctedString = Regex.Replace(_text, "[ -.?!)(,;:']", "");
            Console.WriteLine(correctedString); //somestr
            _columnsCount = (correctedString.Length);

            string[,] nums3 = new string[_lineCount, _columnsCount];
            int PoryadokStr = 0, strNumber = 0, direction = 1;
            for (int j = 0; j < _columnsCount; j++)
            {
                nums3[strNumber, j] = correctedString[PoryadokStr].ToString();
                strNumber += direction;
                if (strNumber == _lineCount - 1 || strNumber == 0)
                {
                    direction *= -1;
                }
                PoryadokStr++;
            }
            StringBuilder sresult = new StringBuilder();

            for (int i = 0; i < _lineCount; i++)
            {
                for (int j = 0; j < _columnsCount; j++)
                {
                    sresult.Append(nums3[i, j]);
                }
                sresult.Append(' ');
            }
            return sresult.ToString();
        }

        public override void Introduce()
        {
            while (true)
            {
                Console.Write("Введите текст: ");
                _text = Console.ReadLine();

                Console.WriteLine("Введите номер операции\n"
                    + "1 - шифрование\n"
                    + "2 - дешифрование");
                int operation = Convert.ToInt32(Console.ReadLine());
                switch (operation)
                {
                    case 1:
                        Console.WriteLine(Encrypt(_text));
                        break;
                    case 2:
                        Console.WriteLine(Decrypt(_text));
                        break;
                    default:
                        Console.WriteLine("Неверный код");
                        break;

                }
            }
        }
    }
}
