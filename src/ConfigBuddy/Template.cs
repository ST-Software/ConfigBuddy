using System.IO;

namespace ConfigBuddy.Core
{
    public class Template
    {
        public Template(string path, string content)
        {
            Path = path;
            Content = content;
        }

        public string Path { get; set; }
        public string Content { get; set; }

        public string Run(Configuration configuration)
        {
            CheckConfigKeys(configuration);
            return TemplateTransformer.TransformTemplateRecursive(Content, configuration.Data);
        }

        private void CheckConfigKeys(Configuration configuration)
        {
            var configKeys = TemplateTransformer.GetAllConfigKeysFromTemplate(Content);
            foreach (var configKey in configKeys)
            {
                var key = TemplateTransformer.GetTemplateKeyWithoutAfixes(configKey);
                var configurationHasKey = configuration.Data.ContainsKey(key);
                if (!configurationHasKey)
                {
                    Logger.Warning("Warning: '{0}' - Is not in configuration values", configKey);
                }
                else
                {
                    Logger.Debug("{0}: {1}", configKey, configuration.Data[key]);
                }
            }
        }

        public static Template FromFile(string file)
        {
            return new Template(file, File.ReadAllText(file));
        }
    }
}