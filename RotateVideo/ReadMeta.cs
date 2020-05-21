using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace RotateVideo
{
    public class ReadMeta : IReadMeta
    {
        private readonly IConfigurationRoot _config;
        
        public ReadMeta(IConfigurationRoot config)
        {
            _config = config;
        }

        /// <summary>
        /// Extract rotation meta value from uploaded video
        /// </summary>
        /// <returns>0, 90, 180 or 270</returns>
        public int ReadRotation(string mediaFile)
        {
            var ffProbe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _config.GetSection("ffprobe").Value);
            var arguments = $"-loglevel error -select_streams v:0 -show_entries stream_tags=rotate -of default=nw=1:nk=1 -i {mediaFile}";
            
            List<string> result = new List<string>();

            using (var process = new Process())
            {
                process.StartInfo.FileName = ffProbe;
                process.StartInfo.Arguments = arguments;

                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = false;

                process.OutputDataReceived += (sender, data) => result.Add(data.Data);
                
                process.Start();
                process.BeginOutputReadLine();

                while (!process.HasExited && process.Responding)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }

                process.WaitForExit();

                Console.WriteLine();
            }
            var rotationStr = result.Where(x => !string.IsNullOrEmpty(x)).FirstOrDefault();
            return rotationStr != null ? int.Parse(rotationStr) : 0;
        }
    }
}
