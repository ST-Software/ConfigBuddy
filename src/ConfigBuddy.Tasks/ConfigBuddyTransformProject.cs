using System;
using System.IO;
using ConfigBuddy.Core;
using ConfigBuddy.Core.Configurations;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Logger = ConfigBuddy.Core.Logger;

namespace ConfigBuddy.Tasks
{
    public class ConfigBuddyTransformProject : Task
    {
        public override bool Execute()
        {
            Logger.LogAction = (msg, level) =>
            {
                if (level == LogLevel.Error) Log.LogError(msg);
                if (level == LogLevel.Warning) Log.LogWarning(msg);
                if (level == LogLevel.Debug) Log.LogMessage(MessageImportance.High, msg);
            };

            var config = GeneratorConfiguration.FromFile(ConfigFile);
            config.ApplyProperties(InlineProperties.Parse(Properties));

            var globalConfig = GeneratorConfiguration.FromFile(Path.Combine(config.SetsPath, "configbuddy.configurations.xml"));
            globalConfig.OutputDir = config.OutputDir;
            globalConfig.ConfigDir = Path.Combine(config.SetsPath, globalConfig.ConfigDir);

            ConfigGenerator.ForOneSet(globalConfig.TemplateDir, globalConfig.OutputDir, globalConfig.ConfigDir, 
                config.SetsPath, globalConfig.TemplateExtension, globalConfig.ConfigExtension, globalConfig.Debug, null);

            return true;
        }

        public string ConfigFile { get; set; }
        public string Properties { get; set; }
    }
}
