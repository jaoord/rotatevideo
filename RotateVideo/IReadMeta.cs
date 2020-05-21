using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RotateVideo
{
    public interface IReadMeta
    {
        string ReadRotation(string mediaFile);
    }
}
