using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphVizNet.Tests
{
    [TestClass]
    public class CommandLineArgumentsTests
    {
        // ReSharper disable StringLiteralTypo
        [TestMethod]
        public void AllNullParameters()
        {
            string arguments = GraphViz.BuildCommandLineArguments(null, null, null, null);

            Assert.AreEqual(string.Empty, arguments);
        }

        [TestMethod]
        public void DotToPng()
        {
            string arguments = GraphViz.BuildCommandLineArguments("file.dot", "file.png", null, "png");

            Assert.AreEqual(" -Tpng -o\"file.png\" \"file.dot\"", arguments);
        }

        [TestMethod]
        public void DotToPngUsingLayout()
        {
            string arguments = GraphViz.BuildCommandLineArguments("file.dot", "file.png", "neato", "png");

            Assert.AreEqual(" -Kneato -Tpng -o\"file.png\" \"file.dot\"", arguments);
        }
        // ReSharper restore StringLiteralTypo
    }
}
