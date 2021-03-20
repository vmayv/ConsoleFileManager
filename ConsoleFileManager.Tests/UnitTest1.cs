using NUnit.Framework;

namespace ConsoleFileManager.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var result = Program.parseInputString("cd C:\\Test");
            Assert.AreEqual(result[0], "cd");
            Assert.AreEqual(result[1], "C:\\Test");
        }
    }
}