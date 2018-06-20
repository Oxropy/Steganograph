using Steganography;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Public image path");
            var publicPath = Console.ReadLine();

            Console.WriteLine("Secret image path");
            var secretPath = Console.ReadLine();

            Console.WriteLine("Save image path");
            var savePath = Console.ReadLine();

            Stegano.CrossoverAndSave(publicPath, secretPath, savePath);
        }
    }
}
