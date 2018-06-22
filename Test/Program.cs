using Steganography;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Crossover: 1");
            Console.WriteLine("GetCrossed: 2");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Crossover();
                    break;
                case "2":
                    GetCrossed();
                    break;
            }
        }

        static void Crossover()
        {
            Console.WriteLine("Public image path");
            var publicPath = Console.ReadLine();

            Console.WriteLine("Secret image path");
            var secretPath = Console.ReadLine();

            Console.WriteLine("Save image path");
            var savePath = Console.ReadLine();

            Stegano.CrossoverAndSave(publicPath, secretPath, savePath);
        }

        static void GetCrossed()
        {
            Console.WriteLine("Image path");
            var publicPath = Console.ReadLine();

            Console.WriteLine("Save image path");
            var savePath = Console.ReadLine();

            Stegano.GetCrossedAndSave(publicPath, savePath);
        }
    }
}
