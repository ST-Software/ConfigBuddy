using System;
using System.IO;
using System.Text.RegularExpressions;
using ConfigBuddy.Core;
using NUnit.Framework;

namespace ConfigBuddy.Tests
{
    [TestFixture]
    public class ConfigurationBuilderTest
    {

        [Test]
        public void ProjectConfigsTest()
        {
            string valuesDir = Path.Combine(TestUtils.DataDir, "configs");
            string templateDir = Path.Combine(TestUtils.DataDir, "templates", "app1");
            Builder.GenerateConfigsForAllProjects(valuesDir, templateDir, TestUtils.OutputDir, templateOutputSubdir: "myconfigs", cleanOutputDir: true);

            string app1Path = Path.Combine(TestUtils.OutputDir, @"project1\configuration1\myconfigs");
            Assert.AreEqual(true, Directory.Exists(app1Path));
            string text1 = File.ReadAllText(Path.Combine(app1Path, "web.config")).Trim();
            Assert.AreEqual("app1=project1configuration1project1configuration1", text1);
            string text2 = File.ReadAllText(Path.Combine(app1Path, @"subapp1\web.config")).Trim();
            Assert.AreEqual("subapp1=project1configuration1", text2);
        }

        [Test]
        public void ProjectConfigsWithParamsTest()
        {
            string valuesDir = Path.Combine(TestUtils.DataDir, "configs");
            string templateDir = Path.Combine(TestUtils.DataDir, "templates", "app1");
            Builder.GenerateConfigsForAllProjects(valuesDir, templateDir, TestUtils.OutputDir, templateOutputSubdir: "myconfigs", cleanOutputDir: true, debug: false, parameters: "Param1=value1;Test=testvalue");

            string app1Path = Path.Combine(TestUtils.OutputDir, @"project1\configuration1\myconfigs");
            Assert.AreEqual(true, Directory.Exists(app1Path));
            string text1 = File.ReadAllText(Path.Combine(app1Path, "web.config")).Trim();
            Assert.AreEqual("app1=testvaluetestvalue", text1);
            string text2 = File.ReadAllText(Path.Combine(app1Path, @"subapp1\web.config")).Trim();
            Assert.AreEqual("subapp1=testvalue", text2);
        }

        [Test]
        public void ProjectConfigsWithParamsAndFlatStructureTest()
        {
            string valuesDir = Path.Combine(TestUtils.DataDir, "configs");
            string templateDir = Path.Combine(TestUtils.DataDir, "templates", "app1");
            Builder.GenerateConfigsForAllProjects(valuesDir, templateDir, TestUtils.OutputDir, templateOutputSubdir: "myconfigs", cleanOutputDir: true, debug: false, parameters: "Param1=value1;Test=testvalue", flatOutput: true);

            string app1Path = Path.Combine(TestUtils.OutputDir, @"myconfigs");
            Assert.AreEqual(true, Directory.Exists(app1Path));
            Assert.AreEqual(true, File.Exists(Path.Combine(app1Path, "project1.configuration1.web.config")));
            Assert.AreEqual(true, File.Exists(Path.Combine(app1Path, "project1.configuration2.web.config")));
            Assert.AreEqual(true, File.Exists(Path.Combine(app1Path, "project2.configuration1b.web.config")));
        }

        [Test]
        public void RegexTest()
        {
            const string regexPattern = @"\$\(\w*\)";
            MatchCollection result1 = Regex.Matches("$(RootUrl)", regexPattern, RegexOptions.None);
            Assert.AreEqual(1, result1.Count);
            MatchCollection result2 = Regex.Matches("$(RootUrl)$(RegistrationUrlSuffix)", regexPattern, RegexOptions.None);
            Assert.AreEqual(2, result2.Count);
        }

        [Test]
        public void ReplaceUserDirTest()
        {
            string rootDir = Path.Combine(TestUtils.DataDir, "configs", @"project1\configuration2\");
            string rootPath = Path.Combine(rootDir, "values.txt");
            string dir = Path.Combine(rootDir, Environment.UserName);
            string path = Path.Combine(dir, "values.txt");
            var directory = new DirectoryInfo(dir);            
            directory.Create();
            File.WriteAllText(path, "");
            Assert.AreEqual(path, Builder.ReplaceUserDirInPath(Path.Combine(rootDir, "{{userDir}}", "values.txt")));
            directory.Delete(true);
            Assert.AreEqual(rootPath, Builder.ReplaceUserDirInPath(Path.Combine(rootDir, "{{userDir}}", "values.txt")));
        }
    }
}
