using ConfigBuddy.Core;
using Microsoft.Build.Utilities;

namespace ConfigBuddy.Tasks
{
    public class ConfigBudyTransformProjectConfigurations : Task
    {
        public ConfigBudyTransformProjectConfigurations()
        {
            ConfigExtension = "values.xml";
            TemplateExtension = "template.xml";
        }

        public override bool Execute()
        {
            Builder.GenerateConfigsForOneProject(TemplateDir, OutputDir, ConfigDir,
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
