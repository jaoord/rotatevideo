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

        public App(IConfigurationRoot config, IReadMeta readMeta)
        {
            _config = config;
            _readMeta = readMeta;
        }

        public async Task Run(string mediaFile)
        {

            Console.WriteLine(mediaFile);

            Console.WriteLine(_readMeta.ReadRotation(mediaFile));


            // do not close automatically (debugging)
            Console.ReadLine();
        }
    }
}
