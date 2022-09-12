using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    public abstract class Encryption
    {
        // метод-стратегия, определяет, как будет осуществляться работа с шифром
        public abstract void Introduce();
        public abstract string Encrypt(string toEncrypt);
        public abstract string Decrypt(string toDecrypt);

    }
}
