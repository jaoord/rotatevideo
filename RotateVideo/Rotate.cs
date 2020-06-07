using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;

namespace RotateVideo
{
    public class Rotate : IRotate
    {

        private readonly IConfigurationRoot _config;

        public Rotate(IConfigurationRoot config)
        {
            _config = config;
        }

        public void Do(int currentRotation, string mediaFile)
        {
            var outputPath = GetOutputPath(mediaFile);

            if (!File.Exists(mediaFile)) { 
                Console.WriteLine("Input file does not exist");
                return;
            }

            if(!Directory.Exists(outputPath))
            {
                Console.WriteLine($"Output path {outputPath} does not exist");
                return;
            }

            string outputFile = Path.Combine(outputPath, Path.GetFileName(mediaFile));

            var letterBox = $"-vf \"scale=(iw*sar)*min(1280/(iw*sar)\\,720/ih):ih*min(1280/(iw*sar)\\,720/ih), pad=1280:720:(1280-iw*min(1280/iw\\,720/ih))/2:(720-ih*min(1280/iw\\,720/ih))/2\"";
            string arguments = $"-i {mediaFile} {letterBox} \"{outputFile}\"";

            using (var process = new Process())
            {
                process.StartInfo.FileName = _config.GetSection("ffmpeg").Value;
                process.StartInfo.Arguments = arguments;

                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.Start();
                process.WaitForExit(100);
            }
            Console.WriteLine($"File exported to {outputPath}");
        }

        private string GetOutputPath(string mediaFile)
        {
            string cfgOutputPath = _config.GetSection("outputpath").Value;
            string inputPathWithoutFile = Path.GetDirectoryName(mediaFile);
            
            if(string.IsNullOrEmpty(inputPathWithoutFile))
            {
                inputPathWithoutFile = Environment.CurrentDirectory;
            }

            return cfgOutputPath.Replace("{inputpath}", inputPathWithoutFile);
        }

    }
}
