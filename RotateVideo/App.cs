using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RotateVideo
{
    public class App
    {
        private readonly IConfigurationRoot _config;
        private readonly IReadMeta _readMeta;
        private readonly IRotate _rotate;

        public App(IConfigurationRoot config, IReadMeta readMeta, IRotate rotate)
        {
            _config = config;
            _readMeta = readMeta;
            _rotate = rotate;
        }

        public async Task Run(string mediaFile)
        {
            Console.WriteLine();

            var rotation = _readMeta.ReadRotation(mediaFile);

            _rotate.Do(rotation, mediaFile);
        }
    }
}
