using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnimalsImageUrlChecker
{
    /// <summary> Program class. </summary>
    internal static class Program
    {
        /// <summary> Defines the entry point of the application. </summary>
        /// <param name="args"> The arguments. </param>
        private static async Task Main(string[] args)
        {
            string csvPath = args.FirstOrDefault();
            if (!File.Exists(csvPath))
            {
                Console.WriteLine("not found.");
                Environment.Exit(1);
            }

            using (HttpClient httpClient = new HttpClient())
            {
                string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0";
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

                string[] lines = File.ReadAllLines(csvPath);
                string outputDirPath = Path.Combine(Environment.CurrentDirectory, "output");
                _ = Directory.CreateDirectory(outputDirPath);

                foreach ((string line, int i) in lines.Select((e, i) => (e, i)))
                {
                    string[] s = line.Split(',');
                    string url = s[1];
                    string outputPath = Path.Combine(outputDirPath, $"{s[0]}_{i}.txt");
                    try
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            File.WriteAllLines(outputPath, response.Headers.Select(header => $"{header.Key}: {string.Join(",", header.Value)}"));
                            Console.WriteLine($"saved:{outputPath}");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Environment.Exit(0);
        }
    }
}
