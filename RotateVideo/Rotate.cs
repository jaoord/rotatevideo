using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            string ffmpeg = _config.GetSection("ffmpeg").Value;
            var outputPath = GetOutputPath(mediaFile);


            string transpose = string.Empty;

            if (currentRotation == 0)
            {
                // do nothing, move to output path
                File.Copy(mediaFile, outputPath);
                return;
            }
            else //if (currentRotation == 90)
            {
                transpose = "transpose=3,transpose=3";
            }

            string arguments = $"-i {mediaFile} -metadata:s:v:0 rotate=0 -vf \"{transpose}\" \"{outputPath}\"";
            var ffMpeg = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _config.GetSection("ffmpeg").Value);

            using (var process = new Process())
            {
                process.StartInfo.FileName = ffMpeg;
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

            return Path.Combine(cfgOutputPath.Replace("{inputpath}", inputPathWithoutFile), Path.GetFileName(mediaFile));
        }

    }
}
