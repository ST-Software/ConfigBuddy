using System.Collections.Generic;
using System.Linq;
using ConfigBuddy.Core.Configurations;
using NUnit.Framework;

namespace ConfigBuddy.Tests.Configurations
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class InlinePropertiesTest
    {
        [Test]
        public void Should_parse_properties_from_one_line_text()
        {
            var properties = InlineProperties.Parse("TargetDir=$(TargetDir);ProjectName=$(ProjectName)").ToList();
            Assert.AreEqual(2, properties.Count);
            Assert.AreEqual("TargetDir", properties[0].Key);
            Assert.AreEqual("$(TargetDir)", properties[0].Value);
            Assert.AreEqual("ProjectName", properties[1].Key);
            Assert.AreEqual("$(ProjectName)", properties[1].Value);
        }

    }
}