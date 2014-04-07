using ConfigBuddy.Core;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Logger = ConfigBuddy.Core.Logger;

namespace ConfigBuddy.Tasks
{
    public class ConfigBudyTransformProjectConfigurations : Task
    {
        public ConfigBudyTransformProjectConfigurations()
        {
            ConfigExtension = "config.xml";
            TemplateExtension = "template";
        }

        public override bool Execute()
        {
            Logger.LogAction = (msg, level) =>
            {
                if (level == LogLevel.Error) Log.LogError(msg);
                if (level == LogLevel.Warning) Log.LogWarning(msg);
                if (level == LogLevel.Debug) Log.LogMessage(MessageImportance.High, msg);
            };

            ConfigGenerator.ForOneProject(TemplateDir, OutputDir, ConfigDir,
                TemplateExtension, ConfigExtension, Debug, null, null);

            return true;
        }

        public string ConfigDir { get; set; }
        public string OutputDir { get; set; }
        public string TemplateDir { get; set; }

        public bool Debug { get; set; }
        public string ConfigExtension { get; set; }
        public string TemplateExtension { get; set; }
    }
}
