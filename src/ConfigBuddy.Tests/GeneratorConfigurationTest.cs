using ConfigBuddy.Core;
using NUnit.Framework;

namespace ConfigBuddy.Tests
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
    }
}