using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnimalsFilterWithResHeader
{
    /// <summary> ResHeaderValidator class. </summary>
    internal static class ResHeaderValidator
    {
        /// <summary> The bad header </summary>
        private static readonly HashSet<string> badHeader = new HashSet<string>();

        /// <summary> Initializes the <see cref="ResHeaderValidator" /> class. </summary>
        static ResHeaderValidator()
        {
            _ = badHeader.Add("Content-Security-Policy: media-src   'self' vine.co;");
        }

        /// <summary> Validates the specified path. </summary>
        /// <param name="path"> The path. </param>
        /// <returns> </returns>
        public static bool Validate(string path)
        {
            return !File.ReadLines(path).Any(badHeader.Contains);
        }
    }
}
