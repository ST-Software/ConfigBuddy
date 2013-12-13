using System;
using System.IO;
using System.Xml.Linq;
using ConfigBuddy.Core;
using NUnit.Framework;

namespace ConfigBuddy.Tests
{
    [TestFixture]
    public class ConfigurationTest
    {
        // ReSharper disable InconsistentNaming

        [Test]
        public void Should_load_configuration_from_xml_document()
        {
            var xdoc = XDocument.Parse(@"
                <configuration>
                  <parameter key='Test'>
                    <test>
                      <partOne>
                        TestingValue
                      </partOne>
                    </test>
                  </parameter>
                </configuration>");
            var configurationValueFromXml = Configuration.FromXml(xdoc).Data;

            Assert.AreEqual(1, configurationValueFromXml.Count);
            Assert.AreEqual("<test><partOne>TestingValue</partOne></test>", configurationValueFromXml["Test"].Trim().Replace(Environment.NewLine, "").Replace(" ", ""));
        }

        [Test]
        public void Should_load_configuration_from_xml_file()
        {
            string configurationFile = Path.Combine(TestUtils.DataDir, "configs", @"project1\configuration2\config.values.xml");
            var configurationValueFromXml = Configuration.FromXmlFile(configurationFile).Data;

            Assert.AreEqual(1, configurationValueFromXml.Count);
            Assert.AreEqual("<test><partOne>TestingValue</partOne></test>", configurationValueFromXml["Test"].Trim().Replace(Environment.NewLine, "").Replace(" ", ""));
        }
 
    }
}