using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConfigBuddy.Core
{
    public static class FileUtils
    {
        public static List<string> GetOwnAndParentFiles(string root, string path, string extension)
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

                if (Directory.Exists(dir))
                {
                    files.AddRange(Directory.GetFiles(dir).FilterExtensions(extension));
                }
                if (dir == root)
                {
                    break;
                }
            }
            return files;
        }

        public static List<string> GetOwnAndChildrenFiles(string path, string extension)
        {
            var files = new List<string>();
            var stack = new Stack<string>();

            stack.Push(path);

            while (stack.Count > 0)
            {
                string dir = stack.Pop();
                
                files.AddRange(Directory.GetFiles(dir).FilterExtensions(extension));    

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
                result.AddRange(Directory.GetFiles(valuesDir).FilterExtensions(extension));
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

        public static List<string> FilterExtensions(this IEnumerable<string> list, string extension)
        {
            if (String.IsNullOrWhiteSpace(extension))
            {
                return list.ToList();
            }
            extension = extension.ToLowerInvariant();
            return list
                .Where(i => i.ToLowerInvariant().EndsWith(extension))
                .ToList();
        }
    }
}