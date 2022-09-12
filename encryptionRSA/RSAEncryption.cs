using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    public class RSAEncryption : Encryption
    {
        // криптографический алгоритм, с открытым ключем, основывающийся на вычислительной сложности
        // факторизации больших целых чисел
        // e, n / d, n
        public (BigInteger, BigInteger) PrivateKey { get; private set; }
        public (BigInteger, BigInteger) PublicKey { get; private set;}
        // размер в байтах
        private int _keySize;
        private readonly List<BigInteger> _primes = new List<BigInteger>();
        private List<int> _firstPrimes = new List<int>() { 2 };
        public RSAEncryption()
        {

        }

        private void FindFirstPrimes()
        {
            for (int i = 2; i < 1000; i++)
            {
                if(_firstPrimes.Any(x => i % x == 0)){
                    continue;
                }
                else _firstPrimes.Add(i);            
            }
        }

        //алгоритм проверки числа на простоту посредством одноименного алгоритма
        // n - число, простоту которого необходимо доказать
        private bool MillerRabinAlgorithm(BigInteger n)
        {
            // условия
            // a^d mod n = 1
            // r exist, r < s, a^(2^r*d) mod n = n - 1
            //цикл нахождения представления числа в виде произведения (2^s)*d, где d нечетно  
            int s = 0;
            BigInteger d = n - 1;

            while (d % 2 == 0)
            {
                s++;
                d /= 2;
            }

            //число необходимых подтверждений простоты числа (r)
            int r = (int)BigInteger.Log(n, 2);

            // цикл перебора количества чисел потенциальных свидетелей простоты входного числа

            for(int i = 0; i < r; i++)
            {
                //генерация случайного числа меньше входного a
                BigInteger possiblePrimeWitness;
                do
                {
                    possiblePrimeWitness = GetRandomSizedInt(_keySize);
                }
                while (possiblePrimeWitness < 2 || possiblePrimeWitness > n-2);

                //условие простоты числа, если неверно - число составное
                BigInteger buf = BigInteger.ModPow(possiblePrimeWitness, d, n);
                if ((buf == 1 || buf == n - 1)
                    || IsRExist(s, possiblePrimeWitness, d, n))
                {
                    continue;
                }
                return false;
            }
            return true;

            // проверка существования r, удовлетворяющее второму условию алгоритма
            bool IsRExist(int power, BigInteger a, BigInteger oddRemainder, BigInteger n2)
            {
                BigInteger powerbuffer = oddRemainder;
                for(int r = 0; r < power; r++)
                {
                    BigInteger buffer = BigInteger.ModPow(a, powerbuffer, n2);
                    //проверка числа на соответсвие 2 условию алгоритма миллера рабина
                    if (buffer == 1)
                    {
                        return false;
                    }
                    else 
                    if(buffer == n - 1)
                    {
                        return true;
                    }
                    powerbuffer *= 2;
                }
                return false;
            }

        }

        // алгоритм, который находит мультипликативно обратное к открытой экспоненте
        private BigInteger ExtendedEuclidAlgorithm(BigInteger eulerFunction, // n
            BigInteger publicExponent) // a
        {
            
            // ax = eulerFunction
            // d * public exponent - 1 делится нацело на euler function
            // at = mod n
            BigInteger t = 0;
            BigInteger newt = 1;
            BigInteger r = eulerFunction;
            BigInteger newr = publicExponent;

            // алгоритм стырен, но не понят
            while(newr != 0)
            {
                BigInteger quotient = r / newr;
                (t, newt) = (newt, t - quotient * newt);
                (r, newr) = (newr, r - quotient * newr);
            }

            //if(r > 1)
            //{
                //throw new ArithmeticException();
            //}
            if (t < 0)
            {
                t += eulerFunction;
            }
            return t;
        }

        // генерация и установка ключей
        public void GenerateKeyPair()
        {
            if(_keySize <= 20)
            {
                FindPrimes(3);
            }
            else
            {
                FindPrimesAsync().Wait();
            }

            BigInteger pPrime = _primes[0]; //29
            BigInteger qPrime = _primes[1]; //_primes[1];11
            BigInteger publicExponent = _primes[2]; //_primes[2];
            BigInteger module_n = pPrime * qPrime;
            BigInteger eulerFunction = (pPrime - 1) * (qPrime - 1);
            BigInteger privateExponent = ExtendedEuclidAlgorithm(eulerFunction, publicExponent);
            PublicKey = (publicExponent, module_n);
            PrivateKey = (privateExponent, module_n);

            if (eulerFunction % publicExponent == 0) throw new Exception();
        }

        // найти определенное количество возможно простых чисел
        private void FindPrimes(int count)
        {
            while (_primes.Count <= count)
            {
                AddPossiblePrimeNumberToList();
            }
        }
        private async Task FindPrimesAsync()
        {
            var tasks = Enumerable.Range(1, 16)
               .Select(x => Task.Run(AddPossiblePrimeNumberToList));
            await Task.Factory.
               ContinueWhenAny(tasks.ToArray(), (dummytask) => FindPrimes(3));
        }
        //получение случайного целого числа заданной длины
        private BigInteger GetRandomSizedInt(int bytesCount)
        {
            byte[] byteBigIntRepresentation = new byte[bytesCount + 1];
            new Random().NextBytes(byteBigIntRepresentation);
            byteBigIntRepresentation[^1] = 0;
            return new BigInteger(byteBigIntRepresentation);
        }

        //получение возможно простого числа
        private void AddPossiblePrimeNumberToList()
        {
            bool IsPrime = false;
            BigInteger number = 0;
            while (!IsPrime )
            {
                number = GetRandomSizedInt(_keySize);
                number += number.IsEven ? 1 : 0;
                if(!_firstPrimes.Any(x => number % x == 0))
                {
                    IsPrime = MillerRabinAlgorithm(number);
                }
                
            }
            if (!_primes.Contains(number))
            {
                _primes.Add(number);
            }
            else AddPossiblePrimeNumberToList();
        }

        public override string Encrypt(string toEncrypt)
        {
            var chunks = ChunkMessageInUnicode(toEncrypt.Trim(), PublicKey.Item2);
            StringBuilder sb = new StringBuilder();
            foreach (BigInteger chunk in chunks)
            {
                var c = BigInteger.ModPow(chunk, PublicKey.Item1, PublicKey.Item2);
                sb.Append(c.ToString() + " ");
            }
            var s = sb.ToString().Trim();
            return s;
        }
       
        public override string Decrypt(string toDecrypt)
        {
            // получение чисел из строки
            var chunks = ReadNumbers(toDecrypt.Trim());        
            // расшифровка до чисел
            List<BigInteger> decrypted = new List<BigInteger>();

            foreach (BigInteger chunk in chunks)
            {
                decrypted.Add(BigInteger.ModPow(chunk, PrivateKey.Item1, PrivateKey.Item2));
            }
            return ConvertToString(decrypted);

        }

        public override void Introduce()
        {
            Console.WriteLine("Введите размер ключа в байтах");

            int size = Convert.ToInt32(Console.ReadLine());

            _keySize = size > 256 ? 16 : size == 0 ? 16 : size;
            FindFirstPrimes();
            GenerateKeyPair();

            Console.WriteLine("Ключи RSA были успешно сгенерированы");

            Console.WriteLine("Введите строку шифрования");

            string toEncrypt = Console.ReadLine();

            Console.WriteLine("Результат шифрования:");

            string encrypted = Encrypt(toEncrypt);
            Console.WriteLine(encrypted.Replace(" ", "\n"));

            string decrypted = Decrypt(encrypted);
            Console.WriteLine("Результат дешифрования:\n" + decrypted);
        }

        // метод чанкинга, разбивает текст на чанки, которые могут быть обработаны данным ключем
        private List<BigInteger> ChunkMessageInUnicode(string message, BigInteger n)
        {
            //строка раскладывается на массив байт
            byte[] unicodeMessage = Encoding.Unicode.GetBytes(message);
            //определение максимального размера чанка в битах
            int chunkSizeInBits = (int)Math.Ceiling(BigInteger.Log(n, 2));
            //значение чанка не может быть больше, чем n - 1 
            List<BigInteger> chunks = new List<BigInteger>();
            //представление строки как массива бит
            BitArray stringInBits = new BitArray(unicodeMessage);
            // буферное число, которое можно зашифровать 
            BigInteger buffer = 1;
            int bitCount = 0;

            for (int i = 0; i < stringInBits.Count; i++)
            {
                //проверяет, не является ли полученное значение большим, чем n-1

                if (((buffer << 1) + (stringInBits[i] ? 1 : 0) << 1) + 1 > n - 1)
                {
                    chunks.Add((buffer << 1) + 1);
                    buffer = 1;
                    bitCount = 0;
                    i--;
                }
                else
                {
                    buffer <<= 1;
                    buffer += (stringInBits[i] ? 1 : 0);
                    bitCount++;
                }
            }
            if (bitCount > 0) chunks.Add((buffer << 1) + 1);

            return chunks;
        }

        // метод, который читает массив чисел 
        private List<BigInteger> ReadNumbers(string message)
        {
            return message.Split(' ').Select(x => BigInteger.Parse(x)).ToList();
        }
        // преобразует массив считанных чисел в одну строку - дешифрованное сообщение
        private string ConvertToString(List<BigInteger> numberArray)
        {
            List<bool> bitResult = new List<bool>();
            // конвертирует одно число и добавляет чистые биты в массив
            for(int i = 0; i < numberArray.Count; i++)
            {
                bitResult.AddRange(ConvertOneNumber(numberArray[i]));
            }
            bitResult.Reverse();
            List<byte> chars = new List<byte>();
            byte buffer = 0;
            int bitCount = 0;

            for(int j = 0; j<bitResult.Count; j++)
            {
                bitCount++;
                buffer = (byte)((buffer << 1) + (bitResult[j] ? 1 : 0));
                
                if (bitCount > 7)
                {
                    chars.Add(buffer);
                    buffer = 0;
                    bitCount = 0;
                }
            }
            chars.Reverse();
            return Encoding.Unicode.GetString(chars.ToArray());

            // функция, которая парсит отдельное число

            List<bool> ConvertOneNumber(BigInteger num)
            {
                List<bool> significant = new List<bool>();
                var bytes = num.ToByteArray(true, false);
                BitArray temp = new BitArray(bytes);
                int startPosition = temp.Length - 2;
                while (!temp[startPosition+1])
                {
                    startPosition--;
                }
                for(; startPosition > 0; startPosition--)
                {
                    significant.Add(temp[startPosition]);
                }
                return significant;

            }
        }

    }
}
