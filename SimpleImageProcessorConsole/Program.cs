using System.Drawing;
using SimpleImageProcessor;

namespace SimpleImageProcessorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ISimpleImageProcessor processor = new SimpleImageProcessor.SimpleImageProcessor();
            var bitmap = (Bitmap) Image.FromFile("/home/bgaliev/Downloads/sample.jpg");
            var output = processor.ModifyImageContrast(bitmap, 45);
            output.Save("/home/bgaliev/Downloads/output.jpg");
        }
    }
}