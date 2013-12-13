using System;
using System.IO;
using System.Linq;

namespace ConfigBuddy.Core
{
    public class Builder
    {     

        public const string DefaultTemplateExntesion = "template";
        public const string DefaultConfigExtension = "values.xml";
        
        public static void GenerateConfigsForAllProjects(string configDir, string templateDir, string outputDir, string templateOutputSubdir = "", bool cleanOutputDir = false, bool debug = false, string parameters = null, bool flatOutput = false)
        {
            DeleteOutputIf(cleanOutputDir, outputDir);

            var configFiles = FileUtils.FindFilesInLeafDirs(configDir, DefaultConfigExtension);

            foreach (var configFile in configFiles)
            {
                string configPath = Path.GetDirectoryName(configFile);
                string configDiff = configPath.Replace(configDir, "");
                if (configDiff.StartsWith("\\"))
                {
                    configDiff = configDiff.Substring(1, configDiff.Length - 1);
                }

                string destDir = flatOutput ?
                    Path.Combine(outputDir, templateOutputSubdir)
                    : Path.Combine(outputDir, configDiff, templateOutputSubdir);

                var templatePrefix = flatOutput ? configDiff.Replace(Path.DirectorySeparatorChar, '.') + "." : null;
                GenerateConfigsForOneProject(templateDir, destDir, configFile, DefaultTemplateExntesion,
                    DefaultConfigExtension, debug, Configuration.FromParams(parameters), templatePrefix);
            }
        }

        public static void GenerateConfigsForOneProject(string templateDir, 
                string outputDir, string configDir, string templateExtension, 
                string configExtension, bool debug, 
                Configuration parameters, string templatePrefix = null)
        {
            configDir = ReplaceUserDirInPath(configDir);                        
            var configuration = Configuration.FromPath(null, configDir, configExtension, parameters);

            var templates = FileUtils
                .GetFilesFromPathDown(templateDir, templateExtension)
                .Select(Template.FromFile);

            foreach (var template in templates)
            {
                var result = template.Run(configuration);

                if (debug) { Console.WriteLine("Config compiled"); }

                var templatePath = template.Path.Replace(templateDir, String.Empty)
                    .Replace(Path.GetFileName(template.Path), String.Empty)
                    .Trim(Path.DirectorySeparatorChar);

                var templateFile = Path.GetFileName(template.Path)
                    .Replace(String.Format(".{0}", templateExtension), String.Empty)
                    .Trim(Path.DirectorySeparatorChar);

                FileUtils.SaveCompiledFile(result, Path.Combine(outputDir, templatePath, templatePrefix + templateFile));

                if (debug) { Console.WriteLine("Config saved"); }
                
            }

            //if (returnCode != 0)
            //{
            //    Console.WriteLine("ERROR INFO ===============================");
            //    Console.WriteLine("templateDir: {0}", templateDir);
            //    Console.WriteLine("outputDir: {0}", outputDir);
            //    Console.WriteLine("configDir: {0}", configDir);
            //    Console.WriteLine("templateExtension: {0}", templateExtension);
            //    Console.WriteLine("configExtension: {0}", configExtension);
            //    Console.WriteLine("debug: {0}", debug);
            //    Console.WriteLine("==========================================");
            //}
            //return returnCode;
        }

        public static string ReplaceUserDirInPath(string configurationsPath)
        {
            const string userDir = @"{{userDir}}";
            if (configurationsPath.Contains(userDir))
            {
                string newPath =
                    configurationsPath.Replace(userDir, Environment.UserName).Replace(@"\\", @"\").Replace("//", "/");
                if (!File.Exists(newPath))
                {
                    newPath = configurationsPath.Replace(userDir, String.Empty).Replace(@"\\", @"\").Replace("//", "/");
                }
                configurationsPath = newPath;
            }
            return configurationsPath;
        }       

        private static void DeleteOutputIf(bool cleanOutputDir, string outputDir)
        {
            if (cleanOutputDir)
            {
                try
                {
                    Directory.Delete(outputDir, true);
                }
                catch (Exception)
                {
                    ErrorLog("The output directory could not be deleted.");
                }
            }
        }

        private static void ErrorLog(string message)
        {
            Console.WriteLine(message);
        }

    }
}