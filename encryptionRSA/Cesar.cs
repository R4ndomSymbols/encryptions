using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    public class Cesar : Encryption
    {
        private string alphabet;

        private string message;
        private int step;
        private StringBuilder result;

        public Cesar()
        {
            alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГД" +
            "ЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ .,!?:;@$%^&*()-_+=0123456789";
            result = new StringBuilder();
        }
        public override string Decrypt(string toDecrypt)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string toEncrypt)
        {
            for(int i = 0; i < toEncrypt.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if(toEncrypt[i] == alphabet[j])
                    {
                        if(j >= 33 && j < 66)
                        {
                            result.Append(alphabet[(j + step) % 33 + 33]);
                            break;
                        }
                        else if(j >= 66)
                        {
                            result.Append(alphabet[j]);
                        }
                        else
                        {
                            result.Append(alphabet[(j + step) % 33]);
                            break;
                        }
                    }
                }
            }

            return result.ToString();

        }

        public override void Introduce()
        {
            Console.WriteLine("Введите строку для шифра");
            message = Console.ReadLine() ?? "пример";
            Console.WriteLine("Введите номер шага");
            step = int.Parse(Console.ReadLine() ?? "1") % 33;
            Console.WriteLine("Зашифрованное сообщение: ");
            Console.WriteLine(Encrypt(message));

        }
    }
}
