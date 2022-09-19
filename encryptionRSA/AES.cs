using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Globalization;

namespace encryption
{
    public class AES : Encryption
    {
        private Aes? _aes = Aes.Create();
        public override string Decrypt(string toDecrypt)
        {
            var slittedBytes = toDecrypt.Split(' ');
            byte[] convertedBytes = slittedBytes.Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray();
            return Encoding.Unicode.GetString(
                _aes.DecryptCbc(convertedBytes, _aes.IV));
        }

        public override string Encrypt(string toEncrypt)
        {
            var rawResult = _aes.EncryptCbc(Encoding.Unicode.GetBytes(toEncrypt), _aes.IV);

            return string.Join(" ", rawResult.Select(x => $"{x:X}"));
                
        }

        public override void Introduce()
        {
            //Console.WriteLine("Введите ключ (преобразуется в unicode последовательность)");
            //_aes.Key = Encoding.Unicode.GetBytes(Console.ReadLine() ?? "key");
            //_aes.KeySize = key.Length * 8;
            //_aes.BlockSize = _blockSize;

            Console.WriteLine("Введите строку шифрования");

            string toEncrypt = Console.ReadLine() ?? "привет";

            Console.WriteLine("Результат шифрования:");

            string encrypted = Encrypt(toEncrypt);
            Console.WriteLine(encrypted);

            string decrypted = Decrypt(encrypted);
            Console.WriteLine("Результат дешифрования:\n" + decrypted);

            Console.WriteLine($"\nКлюч: {InHex(_aes.Key)}" +
                $"\nIV вектор: {InHex(_aes.IV)}");

        }

        private string InHex(byte[] toConvert)
        {
            return string.Join(" ", toConvert.Select(x => $"{x:X}"));
        }

    }
}
