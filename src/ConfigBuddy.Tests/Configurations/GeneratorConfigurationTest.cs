using System.Collections.Generic;
using ConfigBuddy.Core.Configurations;
using NUnit.Framework;

namespace ConfigBuddy.Tests.Configurations
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GeneratorConfigurationTest
    {
        [Test]
        public void Should_read_configuration_from_file()
        {
            var config = GeneratorConfiguration.FromFile("data/configbuddy.sets.xml");
            Assert.AreEqual("config-dir", config.ConfigDir);
            Assert.AreEqual("config-extension", config.ConfigExtension);
            Assert.AreEqual("config-root", config.ConfigRoot);
            Assert.AreEqual(true, config.Debug);
            Assert.AreEqual("output-dir", config.OutputDir);
            Assert.AreEqual("template-dir", config.TemplateDir);
            Assert.AreEqual("template-extension", config.TemplateExtension);
            Assert.AreEqual(2, config.Projects.Count);
            Assert.AreEqual("project1-name", config.Projects[0].Name);
            Assert.AreEqual("project1-path", config.Projects[0].Path);
        }

        [Test]
        public void Should_apply_property_transformation_for_its_properties()
        {
            var config = new GeneratorConfiguration();
            config.ConfigDir = "aaa";
            config.ConfigExtension = "$(aaa)";
            config.ConfigRoot = "aaa $(bbb)";

            var properties = new Dictionary<string, string> {{"aaa", "AAA"}, {"bbb", "BBB"}};
            config.ApplyProperties(properties);
            Assert.AreEqual("aaa", config.ConfigDir);
            Assert.AreEqual("AAA", config.ConfigExtension);
            Assert.AreEqual("aaa BBB", config.ConfigRoot);
        }
    }
}