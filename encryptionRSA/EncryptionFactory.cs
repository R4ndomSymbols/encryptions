using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    public class EncryptionFactory
    {
        public static readonly EncryptionFactory Factory = new EncryptionFactory();

        private Dictionary<int, (string, Func<Encryption>)> chiphers = new Dictionary<int, (string, Func<Encryption>)>()
        {
            {1, ("Алгоритм шифрования RSA", () => new RSAEncryption()) },
            {2, ("Шифр Виженера", () => new Vigenere()) },
            {3, ("Рельсовый шифр", () => new Rails()) },
            {4, ("Шифр цезаря", () => new Cesar()) },
            {5, ("AES", ()=> new AES()) },
            {6, ("Шифр 4 квадратов", () => new FourSquares()) }

        };

        private EncryptionFactory()
        {

        }

        public Encryption GetEncryption(int number)
        {
            return chiphers[number].Item2.Invoke();
        }
        // печать информации
        public void PrintAllowedChiphers()
        {
            Console.WriteLine(string.Join("\n", chiphers.Select(x => x.Key.ToString() + " " + x.Value.Item1)));
        }



    }
}
