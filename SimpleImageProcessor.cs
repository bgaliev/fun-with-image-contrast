using System.Drawing;
using System.Threading;
using System.Threading.Tasks;


namespace SimpleImageProcessor
{
    public class SimpleImageProcessor : ISimpleImageProcessor
    {
        public Bitmap ModifyImageContrast(Bitmap source, int value)
        {
            double coefficient = 1.0 + (value / 100.0);

            Bitmap target = new Bitmap(source);

            byte[] palette = CreatePalette(source, coefficient);

            Parallel.For(0, source.Height, i =>
            {
                Parallel.For(0, source.Width, k =>
                {
                    Color sourcePixel = source.GetPixel(i, k);
                    byte r = palette[sourcePixel.R];
                    byte g = palette[sourcePixel.G];
                    byte b = palette[sourcePixel.B];
                    byte a = palette[sourcePixel.A];
                    var targetPixel = Color.FromArgb(a, r, g, b);
                    target.SetPixel(i, k, targetPixel);
                });
            });
            return target;
        }

        public Task<Bitmap> ModifyImageContrastAsync(Bitmap source, int value) =>
            Task.Run(() => ModifyImageContrast(source, value));


        private int CalculateBrightness(Color color)
        {
            return (int) (color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
        }

        private int CalculateBrightness(Bitmap bitmap)
        {
            long brightness = 0;
            Parallel.For(0, bitmap.Height, (i) =>
            {
                Parallel.For(0, bitmap.Width, (k) =>
                {
                    var pixel = bitmap.GetPixel(i, k);
                    Interlocked.Add(ref brightness, CalculateBrightness(pixel));
                });
            });
            return (int) (brightness / (bitmap.Height * bitmap.Width));
        }

        private byte[] CreatePalette(Bitmap bitmap, double coefficient)
        {
            byte[] palette = new byte[256];
            var brightness = CalculateBrightness(bitmap);
            Parallel.For(0, 256, i =>
            {
                int delta = i - brightness;
                int buffer = (int) (brightness + (coefficient * delta));
                buffer = (buffer < 0) ? 0 : buffer;
                buffer = (buffer > 255) ? 255 : buffer;
                palette[i] = (byte) buffer;
            });
            return palette;
        }
    }
}