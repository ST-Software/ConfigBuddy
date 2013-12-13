using System;
using System.IO;

namespace ConfigBuddy.Tests
{
    public class TestUtils
    {
        static TestUtils()
        {
            string root = Environment.CurrentDirectory;
            DataDir = Path.Combine(root, "data");
            OutputDir = Path.Combine(root, "output");
            if (Directory.Exists(OutputDir))
            {
                Directory.Delete(OutputDir, true);
            }
            Directory.CreateDirectory(OutputDir);

        }

        public static string OutputDir { get; set; }
        public static string DataDir { get; set; }
    }
}