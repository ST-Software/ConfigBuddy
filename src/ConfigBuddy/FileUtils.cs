using System;
using System.Collections.Generic;
using System.IO;

namespace ConfigBuddy.Core
{
    public static class FileUtils
    {
        public static List<string> GetFilesFromPathUp(string root, string path, string extension)
        {
            var files = new List<string>();
            var stack = new Stack<string>();

            stack.Push(path);

            while (stack.Count > 0)
            {
                var dir = stack.Pop();                
                var directoryInfo = Directory.GetParent(dir);

                if(directoryInfo != null)
                {
                    stack.Push(directoryInfo.ToString());    
                }

                if (dir != root)
                {
                    files.AddRange(Directory.GetFiles(dir, extension != null ? String.Format("*.{0}", extension) : "*.*"));    
                }
            }
            return files;
        }

        public static List<string> GetFilesFromPathDown(string path, string extension)
        {
            var files = new List<string>();
            var stack = new Stack<string>();

            stack.Push(path);

            while (stack.Count > 0)
            {
                string dir = stack.Pop();
                
                files.AddRange(Directory.GetFiles(dir, extension != null ? String.Format("*.{0}", extension) : "*.*"));    

                foreach (string dn in Directory.GetDirectories(dir))
                {
                    stack.Push(dn);
                }
                
            }
            return files;
        }

        public static List<string> FindFilesInLeafDirs(string valuesDir, string extension)
        {
            var result = new List<string>();
            var directories = Directory.GetDirectories(valuesDir);
            if (directories.Length == 0)
            {
                result.AddRange(Directory.GetFiles(valuesDir, "*." + extension));
            }
            else
            {
                foreach (var subdir in directories)
                {
                    result.AddRange(FindFilesInLeafDirs(subdir, extension));
                }
            }
            return result;
        }

        public static void SaveCompiledFile(string compilationResult, string destinationPath)
        {
            var destination = new FileInfo(destinationPath);
            if (destination.Directory != null && !destination.Directory.Exists)
            {
                destination.Directory.Create();
            }
            File.WriteAllText(destination.FullName, compilationResult);
        }
    }
}