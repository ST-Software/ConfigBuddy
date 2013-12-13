using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConfigBuddy.Core
{
    public class Configuration
    {
        public Configuration(Dictionary<string, string> data = null)
        {
            Data = data ?? new Dictionary<string, string>();    
        }

        public Dictionary<string, string> Data { get; private set; }

        public static Configuration FromXml(XDocument xdoc)
        {
            var result = new Dictionary<string, string>();
            foreach (var param in xdoc.Elements("configuration").SelectMany(i => i.Elements("parameter")))
            {
                var keyAttr = param.Attribute("key");
                var valueAttr = param.Attribute("value");
                if (keyAttr == null)
                {
                    throw new ArgumentException("Attribute 'key' is required for element 'parameter'.");
                }

                string parameterValue = null;

                if (valueAttr != null)
                {
                    parameterValue = valueAttr.Value;
                }
                else if (param.Elements().Any())
                {
                    parameterValue = param.Elements().Aggregate("", (xmlText, el) => xmlText + el.ToString());
                }

                result.Add(keyAttr.Value, parameterValue);
            }

            return new Configuration(result);
        }

        public static Configuration FromXmlFile(string xmlConfigurationFile)
        {
            return FromXml(XDocument.Load(xmlConfigurationFile));
        }

        public static Configuration FromParams(string parametersText)
        {
            if (String.IsNullOrEmpty(parametersText))
                return null;

            var result = new Dictionary<string, string>();
            foreach (string paramKeyValue in parametersText.Split(';'))
            {
                string[] keyValueArray = paramKeyValue.Split('=');
                if (keyValueArray.Length != 2)
                    throw new ArgumentException("The parameters should be supplied in 'param1=value1;param2=value2' format");
                result.Add(keyValueArray[0], keyValueArray[1]);
            }
            return new Configuration(result);
        }

        public static Configuration Merge(List<Configuration> configurations)
        {
            var result = configurations
                .SelectMany(i => i.Data)
                .GroupBy(i => i.Key)
                .ToDictionary(i => i.Key, i => i.First().Value);
            
            return new Configuration(result);
        }



        public static Configuration FromPath(string configRoot, string configDir, string configExtension, Configuration configOverwrites)
        {
            IList<string> configurationPaths = FileUtils.GetFilesFromPathUp(configRoot, configDir, configExtension);

            var configurations = configurationPaths
                .Select(FromXmlFile)
                .ToList();

            if (configOverwrites != null)
            {
                configurations.Insert(0, configOverwrites);
            }

            return Merge(configurations);
        }

    }
}