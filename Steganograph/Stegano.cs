using System.Drawing;
using System.Drawing.Imaging;

namespace Steganograph
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

            SaveImage(ExtractSecret(bitmap), savePath);
        }

        public static Bitmap Crossover(Bitmap publicBitmap, Bitmap secretBitmap)
        {
            Bitmap crossover = new Bitmap(publicBitmap.Width, publicBitmap.Height);

            for (int x = 0; x < publicBitmap.Width; x++)
            {
                for (int y = 0; y < publicBitmap.Height; y++)
                {
                    var publicPixel = publicBitmap.GetPixel(x, y);
                    var publicRed = publicPixel.R;
                    var publicGreen = publicPixel.G;
                    var publicBlue = publicPixel.B;

                    var secretPixel = secretBitmap.GetPixel(x % secretBitmap.Width, y % secretBitmap.Height);
                    var secretRed = secretPixel.R;
                    var secretGreen = secretPixel.G;
                    var secretBlue = secretPixel.B;

                    var red = publicRed & 0xFE | (secretRed >> 7);
                    var green = publicGreen & 0xFE | (secretGreen >> 7);
                    var blue = publicBlue & 0xFE | (secretBlue >> 7);

                    crossover.SetPixel(x, y, Color.FromArgb(publicPixel.A, red, green, blue));
                }
            }

            return crossover;
        }

        public static Bitmap ExtractSecret(Bitmap bitmap)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var red = (pixel.R << 7) & 0xFF;
                    var green = (pixel.G << 7) & 0xFF;
                    var blue = (pixel.B << 7) & 0xFF;

                    newBitmap.SetPixel(x, y, Color.FromArgb(pixel.A, red, green, blue));
                }
            }

            return newBitmap;
        }

        public static Bitmap LoadImage(string path)
        {
            return (Bitmap)Image.FromFile(path);
        }

        public static void SaveImage(Bitmap image, string path)
        {
            image.Save(path, ImageFormat.MemoryBmp);
        }
    }
}