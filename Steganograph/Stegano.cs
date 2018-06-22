using System.Drawing;
using System.Drawing.Imaging;

namespace Steganograph
{
    public class Stegano
    {
        public static void CrossoverAndSave(string publicPath, string secretPath, string savePath, int privateBitCount = 1)
        {
            if (privateBitCount > 8) return;

            var publicBitmap = LoadImage(publicPath);
            var secretBitmap = LoadImage(secretPath);

            SaveImage(Crossover(publicBitmap, secretBitmap, privateBitCount), savePath);
        }

        public static void GetCrossedAndSave(string path, string savePath, int privateBitCount = 1)
        {
            if (privateBitCount > 8) return;

            var bitmap = LoadImage(path);

            SaveImage(ExtractSecret(bitmap, privateBitCount), savePath);
        }

        public static Bitmap Crossover(Bitmap publicBitmap, Bitmap secretBitmap, int privateBitCount)
        {
            int publicBitCount = 8 - privateBitCount;

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

                    var red = publicRed & 0xFF << privateBitCount & 0xFF | (secretRed >> publicBitCount);
                    var green = publicGreen & 0xFF << privateBitCount & 0xFF | (secretGreen >> publicBitCount);
                    var blue = publicBlue & 0xFF << privateBitCount & 0xFF | (secretBlue >> publicBitCount);

                    crossover.SetPixel(x, y, Color.FromArgb(publicPixel.A, red, green, blue));
                }
            }

            return crossover;
        }

        public static Bitmap ExtractSecret(Bitmap bitmap, int privateBitCount)
        {
            int publicBitCount = 8 - privateBitCount;

            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var red = (pixel.R << publicBitCount) & 0xFF;
                    var green = (pixel.G << publicBitCount) & 0xFF;
                    var blue = (pixel.B << publicBitCount) & 0xFF;

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