using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ConfigBuddy.Core
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

        [XmlArrayItem("Project")]
        public List<ProjectConfiguration> Projects { get; set; }

        public static GeneratorConfiguration FromFile(string fileName)
        {
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
    }

    [XmlRoot("Project")]
    public class ProjectConfiguration
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Path { get; set; }
    }
}