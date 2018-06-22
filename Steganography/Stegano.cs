using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Steganography
{
    public class Stegano
    {
        public static void CrossoverAndSave(string publicPath, string secretPath, string savePath)
        {
            var publicBitmap = LoadImage(publicPath);
            var secretBitmap = LoadImage(secretPath);

            SaveImage(Crossover(publicBitmap, secretBitmap), savePath);
        }

        public static void GetCrossedAndSave(string path, string savePath)
        {
            var bitmap = LoadImage(path);

            SaveImage(GetCrossed(bitmap), savePath);
        }

        public static Bitmap LoadImage(string path)
        {
            return (Bitmap)Image.FromFile(path, true);
        }

        public static Bitmap Crossover(Bitmap publicBitmap, Bitmap secretBitmap)
        {
            Bitmap crossover = new Bitmap(publicBitmap.Width, publicBitmap.Height);

            for (int x = 0; x < publicBitmap.Width; x++)
            {
                for (int y = 0; y < publicBitmap.Height; y++)
                {
                    var publicChannels = ExtractChannels(publicBitmap, x, y);
                    var publicAlpha = publicChannels.Item1;
                    var publicRed = publicChannels.Item2;
                    var publicGreen = publicChannels.Item3;
                    var publicBlue = publicChannels.Item4;

                    var secretChannels = ExtractChannels(secretBitmap, x % secretBitmap.Width, y % secretBitmap.Height);
                    var secretAlpha = secretChannels.Item1;
                    var secretRed = secretChannels.Item2;
                    var secretGreen = secretChannels.Item3;
                    var secretBlue = secretChannels.Item4;

                    var alpha = publicAlpha | (secretAlpha >> 7);
                    var red = publicRed | (secretRed >> 7);
                    var green = publicGreen | (secretGreen >> 7);
                    var blue = publicBlue | (secretBlue >> 7);
                    var newColor = Color.FromArgb(alpha, red, green, blue);

                    crossover.SetPixel(x, y, newColor);
                }
            }

            return crossover;
        }

        public static Bitmap GetCrossed(Bitmap bitmap)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var channels = ExtractChannels(bitmap, x, y);
                    var alpha = channels.Item1 << 7;
                    var red = channels.Item2 << 7;
                    var green = channels.Item3 << 7;
                    var blue = channels.Item4 << 7;
                    var newColor = Color.FromArgb(alpha, red, green, blue);

                    newBitmap.SetPixel(x, y, newColor);
                }
            }

            return bitmap;
        }

        public static void SaveImage(Bitmap image, string path)
        {
            image.Save(path, ImageFormat.Bmp);
        }

        private static Tuple<byte, byte, byte, byte> ExtractChannels(Bitmap image, int x, int y)
        {
            var pixel = image.GetPixel(x, y);
            return new Tuple<byte, byte, byte, byte>(pixel.A, pixel.R, pixel.G, pixel.B);
        }
    }
}