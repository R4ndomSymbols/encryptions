// See https://aka.ms/new-console-template for more information
using encryption;


EncryptionFactory.Factory.PrintAllowedChiphers();

Console.WriteLine("Введите номер");
int num = Convert.ToInt32(Console.ReadLine());

EncryptionFactory.Factory.GetEncryption(num).Introduce();


