using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnimalsFilterWithResHeader
{
    /// <summary> Program class. </summary>
    internal static class Program
    {
        /// <summary> Defines the entry point of the application. </summary>
        /// <param name="args"> The arguments. </param>
        private static void Main(string[] args)
        {
            string srcPath = args.FirstOrDefault();
            string resHeaderDirPath = args.ElementAtOrDefault(1);
            string dstPath = args.ElementAtOrDefault(2);

            if (new[] { srcPath, resHeaderDirPath, dstPath }.Any(string.IsNullOrEmpty))
            {
                Console.WriteLine("invalid args.");
                Console.WriteLine(string.Join(Environment.NewLine, new[] { ":Usage", "AnimalsFilterWithResHeader [srcPath] [resHeaderDirPath] [dstPath]" }));
                Environment.Exit(1);
            }

            (string ind, int Index)[] items = Directory.GetFiles(resHeaderDirPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(ResHeaderValidator.Validate)
                .Select(Path.GetFileNameWithoutExtension)
                .Select(e => e.Split('_'))
                .Select(s => (ind: s[0], Index: int.Parse(s[1])))
                .ToArray();

            Dictionary<string, HashSet<int>> map = new Dictionary<string, HashSet<int>>();
            foreach ((string kind, int index) in items)
            {
                if (!map.ContainsKey(kind))
                {
                    map[kind] = new HashSet<int>();
                }

                _ = map[kind].Add(index);
            }

            HashSet<string> list = new HashSet<string>();
            string[] lines = File.ReadAllLines(srcPath);
            foreach ((string line, int i) in lines.Select((e, i) => (e, i)))
            {
                try
                {
                    string[] s = line.Split(',');
                    if (map[s[0]].Contains(i))
                    {
                        _ = list.Add(line);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception:{ex}");
                }
            }

            File.WriteAllLines(dstPath, list.OrderBy(e => e));
        }
    }
}
