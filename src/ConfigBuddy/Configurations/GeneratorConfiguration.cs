using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ConfigBuddy.Core.Configurations
{
    [XmlRoot("ConfigBuddy")]
    public class GeneratorConfiguration
    {
        public GeneratorConfiguration()
        {
            Projects = new List<ProjectConfiguration>();
        }

        public string ConfigExtension { get; set; }
        public string TemplateExtension { get; set; }
        public string TemplateDir { get; set; }
        public string ConfigDir { get; set; }
        public string ConfigRoot { get; set; }
        public bool Debug { get; set; }
        public string OutputDir { get; set; }

        public string SetsPath { get; set; }

        [XmlArrayItem("Project")]
        public List<ProjectConfiguration> Projects { get; set; }

        public static GeneratorConfiguration FromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new ArgumentException(String.Format("Config file not found: '{0}'", fileName));
            }

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return Parse(stream);
            }
        }

        public static GeneratorConfiguration Parse(Stream reader)
        {
            var serializer = new XmlSerializer(typeof (GeneratorConfiguration));
            return (GeneratorConfiguration)serializer.Deserialize(reader);
        }

        private IEnumerable<Prop> GetStringProperties()
        {
            yield return new Prop(() => ConfigExtension, value => ConfigExtension = value);
            yield return new Prop(() => TemplateExtension, value => TemplateExtension = value);
            yield return new Prop(() => TemplateDir, value => TemplateDir = value);
            yield return new Prop(() => ConfigDir, value => ConfigDir = value);
            yield return new Prop(() => ConfigRoot, value => ConfigRoot = value);
            yield return new Prop(() => OutputDir, value => OutputDir = value);
        }

        public void ApplyProperties(Dictionary<string, string> properties)
        {
            foreach (var prop in GetStringProperties())
            {
                var value = prop.Gettter();
                if (value != null)
                {
                    value = TemplateTransformer.TransformTemplateSimple(value, properties);
                    prop.Setter(value);
                }
            }            
        }

        private class Prop 
        {
            public Prop(Func<string> getter, Action<string> setter)
            {
                Gettter = getter;
                Setter = setter;
            }

            public Func<string> Gettter { get; private set; }
            public Action<string> Setter { get; private set; }
        }
    }
}