using System.Collections.Generic;
using ConfigBuddy.Core;
using NUnit.Framework;

namespace ConfigBuddy.Tests
{
    [TestFixture]
    public class TemplateTransformerTest
    {
        // ReSharper disable InconsistentNaming
        
        [Test]
        public void Should_recursively_transform_template()
        {
            var configs = new Dictionary<string, string>
                {
                    {"a", "A"},
                    {"b", "B$(c)"},
                    {"c", "C$(d)"},
                    {"d", "$(a)"},
                    {"e", "$(x)"},// this caused endless loop
                };
            Assert.AreEqual("ABCACA", TemplateTransformer.TransformTemplateRecursive("$(a)$(b)$(c)", configs));
        }

        [Test]
        public void Should_find_config_keys_correctly()
        {
            Assert.AreEqual(1, TemplateTransformer.MatchConfigKeyRegex.Matches("$(test)").Count);
            Assert.AreEqual(1, TemplateTransformer.MatchConfigKeyRegex.Matches("B$(c)").Count);
            Assert.AreEqual(2, TemplateTransformer.MatchConfigKeyRegex.Matches("$(a)$(b)").Count);

            Assert.AreEqual(0, TemplateTransformer.MatchConfigKeyRegex.Matches("").Count);
            Assert.AreEqual(0, TemplateTransformer.MatchConfigKeyRegex.Matches(" ").Count);
            Assert.AreEqual(0, TemplateTransformer.MatchConfigKeyRegex.Matches("$").Count);
            Assert.AreEqual(0, TemplateTransformer.MatchConfigKeyRegex.Matches("dfsdfs").Count);
            Assert.AreEqual(0, TemplateTransformer.MatchConfigKeyRegex.Matches("dafa adfa").Count);
        }
    }
}