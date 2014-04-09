using System.Xml.Serialization;

namespace ConfigBuddy.Core.Configurations
{
    [XmlRoot("Project")]
    public class ProjectConfiguration
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Path { get; set; }
    }
}