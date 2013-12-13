using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConfigBuddy.Core
{
    public class TemplateTransformer
    {
        private static string StartTemplateKey { get; set; }
        private static string EndTemplateKey { get; set; }

        static TemplateTransformer()
        {
            StartTemplateKey = "$(";
            EndTemplateKey = ")";
        }                

        private static readonly Regex _matchConfigKeyRegex = new Regex(@"\$\(\w+\)");

        public static Regex MatchConfigKeyRegex { get { return _matchConfigKeyRegex; }}

        public static IList<string> GetAllConfigKeysFromTemplate(string template)
        {
            return _matchConfigKeyRegex.Matches(template)
                .Cast<Match>()
                .Select(i => i.ToString())
                .ToList();
        }

        public static string TransformTemplateSimple(string template, Dictionary<string, string> configuration)
        {
            return configuration.Aggregate(template, (current, keyValuePair) => current.Replace(GetTemplateKey(keyValuePair.Key), keyValuePair.Value));
        }

        private static string GetTemplateKey(string key)
        {
            return String.Format("{0}{1}{2}", StartTemplateKey, key, EndTemplateKey);
        }

        public static string GetTemplateKeyWithoutAfixes(string key)
        {
            return key.Replace(StartTemplateKey, String.Empty).Replace(EndTemplateKey, String.Empty);
        }

        public static string TransformTemplateRecursive(string templateContent, Dictionary<string, string> mergedConfiguration)
        {
            var preprocessedConfigs = new Dictionary<string, string>(mergedConfiguration);

            preprocessedConfigs = TransformConfigurations(preprocessedConfigs);
            return TransformTemplateSimple(templateContent, preprocessedConfigs);
        }

        private static Dictionary<string, string> TransformConfigurations(Dictionary<string, string> preprocessedConfigs, int maxConfigRecursion = 50)
        {
            for (var i = 0; i < maxConfigRecursion; i++)
            {
                var copy = new Dictionary<string, string>(preprocessedConfigs);
                bool replaced = false;
                foreach (var keyValuePair in preprocessedConfigs)
                {
                    if (_matchConfigKeyRegex.IsMatch(keyValuePair.Value))
                    {
                        copy[keyValuePair.Key] = TransformTemplateSimple(keyValuePair.Value, preprocessedConfigs);
                        replaced = true;
                    }
                }

                if (!replaced)
                    break;

                preprocessedConfigs = copy;
            }
            return preprocessedConfigs;
        }
    }
}