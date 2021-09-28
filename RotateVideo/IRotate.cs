using System.Threading.Tasks;

namespace RotateVideo
{
    public interface IRotate
    {
        void Do(int currentRotation, string mediaFile);
    }
}
