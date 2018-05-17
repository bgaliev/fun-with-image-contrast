using System.Drawing;
using System.Threading.Tasks;

namespace SimpleImageProcessor
{
    public interface ISimpleImageProcessor
    {
        Bitmap ModifyImageContrast(Bitmap source, int value);
        Task<Bitmap> ModifyImageContrastAsync(Bitmap source, int value);
    }
}