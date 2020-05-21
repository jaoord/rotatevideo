using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RotateVideo
{
    public class ReadMeta : IReadMeta
    {
        private readonly IConfigurationRoot _config;
        
        public ReadMeta(IConfigurationRoot config)
        {
            _config = config;
        }

        public string ReadRotation(string mediaFile)
        {
            var ffProbe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _config.GetSection("ffprobe").Value);
            var arguments = $"-loglevel error -select_streams v:0 -show_entries stream_tags=rotate -of default=nw=1:nk=1 -i {mediaFile}";
            
            List<string> result = new List<string>();

            using (var process = new Process())
            {
                process.StartInfo.FileName = $"{ffProbe}";
                process.StartInfo.Arguments = arguments;

                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = false;

                process.OutputDataReceived += (sender, data) => result.Add(data.Data);
                Console.WriteLine("starting");

                process.Start();
                process.BeginOutputReadLine();
                var exited = process.WaitForExit(100);
                Console.WriteLine($"exit");
            }

            return result.Where(x => !string.IsNullOrEmpty(x)).FirstOrDefault();
        }
    }
}
