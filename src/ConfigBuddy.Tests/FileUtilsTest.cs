using ConfigBuddy.Core;
using NUnit.Framework;

namespace ConfigBuddy.Tests
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class FileUtilsTest
    {
        [Test]
        public void Should_find_files_in_leadfs_directories()
        {
            var files = FileUtils.FindFilesInLeafDirs(@".\data\configs", "config.values.xml");
            Assert.AreEqual(3, files.Count);
        }

        [Test]
        public void Should_find_all_own_and_children_files()
        {
            var files = FileUtils.GetOwnAndChildrenFiles(@".\data\configs", "config.values.xml");
            Assert.AreEqual(4, files.Count);
        }

        [Test]
        public void Should_find_all_own_and_parent_files()
        {
            var files = FileUtils.GetOwnAndParentFiles(@".\data\configs", @".\data\configs\project1\configuration1", "config.values.xml");
            Assert.AreEqual(2, files.Count);
        }
    }
}