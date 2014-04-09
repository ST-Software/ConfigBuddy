using ConfigBuddy.Core;
using ConfigBuddy.Core.Configurations;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Logger = ConfigBuddy.Core.Logger;

namespace ConfigBuddy.Tasks
{
    public class ConfigBuddyTransformConfigurations : Task    
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
            ConfigGenerator.ForAllSets(config);
            
            return true;
        }
       
        public string ConfigFile { get; set; }
        public string Properties { get; set; }
    }
}