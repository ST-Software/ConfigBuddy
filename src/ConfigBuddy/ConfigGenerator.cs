﻿using System;
using System.IO;
using System.Linq;
using ConfigBuddy.Core.Configurations;

namespace ConfigBuddy.Core
{
    public class ConfigGenerator
    {

        public static void ForAllSets(GeneratorConfiguration config)
        {
            Logger.Debug("ForAllSets...");
            foreach (var project in config.Projects)
            {
                Logger.Debug("project - Name: {0}, Path: {1}", project.Name, project.Path);
                var templateDir = Path.Combine(project.Path, config.TemplateDir);
                ForAllSets(config.ConfigRoot, templateDir, config.OutputDir, project.Name, 
                    false, config.Debug, null, config.ConfigExtension, config.TemplateExtension);
            }
        }

        public static void ForAllSets(string configDir, string templateDir, string outputDir, 
            string templateOutputSubdir = "", bool cleanOutputDir = false, bool debug = false, 
            string parameters = null, string configExtension = "xml", string templateExtension = "template")
        {
            Logger.Debug("ForAllSets(configDir: {0}, templateDir: {1}, outputDir: {2}, templateOutputSubdir: {3}, cleanOutputDir: {4}, debug: {5})",
                configDir, templateDir, outputDir, templateOutputSubdir, cleanOutputDir, debug);

            DeleteOutputIf(cleanOutputDir, outputDir);

            var configFiles = FileUtils.FindFilesInLeafDirs(configDir, configExtension);

            foreach (var configFile in configFiles)
            {
                string configPath = Path.GetDirectoryName(configFile);
                string configDiff = configPath.Replace(configDir, "");
                if (configDiff.StartsWith("\\"))
                {
                    configDiff = configDiff.Substring(1, configDiff.Length - 1);
                }

                string destDir = Path.Combine(outputDir, configDiff, templateOutputSubdir);

                ForOneSet(templateDir, destDir, configFile, configDir, templateExtension,
                    configExtension, debug, Configuration.FromParams(parameters));
            }
        }

        public static void ForOneSet(string templateDir, 
                string outputDir, string configDir, string configRoot, string templateExtension, 
                string configExtension, bool debug, 
                Configuration parameters, string templatePrefix = null)
        {
            Logger.Debug("ForOneSet(templateDir: {0}, outputDir: {1}, configDir: {2}, templateExtensions: {3}, configExtensions: {4}, debug: {5})", 
                templateDir, outputDir, configDir, templateExtension, configExtension, debug);

            templateDir = Path.GetFullPath(templateDir);
            outputDir = Path.GetFullPath(outputDir);
            configDir = Path.GetFullPath(configDir);
            configRoot = Path.GetFullPath(configRoot);
            Logger.Debug(@"After transforming to absolute paths:");
            Logger.Debug(@"templateDir: '{0}'", templateDir);
            Logger.Debug(@"outputDir: '{0}'", outputDir);
            Logger.Debug(@"configDir: '{0}'", configDir);
            Logger.Debug(@"configRoot: '{0}'", configRoot);

            Logger.Debug(@"Replacing {{userDir}} with '" + Environment.UserName + "'");
            configDir = ReplaceUserDirInPath(configDir);                        
            var configuration = Configuration.FromPath(configRoot, configDir, configExtension, parameters);

            var templates = FileUtils
                .GetOwnAndChildrenFiles(templateDir, templateExtension)
                .Select(Template.FromFile);

            foreach (var template in templates)
            {
                Logger.Debug("GenerateConfigsForOneProject processing template: {0}", template.Path);
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
                Logger.Debug("Replaced path: {0}", newPath);
                if (!Directory.Exists(newPath))
                {
                    newPath = configurationsPath.Replace(userDir, String.Empty).Replace(@"\\", @"\").Replace("//", "/");
                    Logger.Debug("New path not found, non user path used instead: {0}", newPath);
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