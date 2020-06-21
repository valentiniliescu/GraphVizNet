using System.IO;
using ApprovalTests;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphVizNet.Tests
{
    [TestClass]
    public class LayoutAndRenderDotGraphTests
    {
        [TestMethod]
        [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
        public void RenderFromInMemoryGraphToFile()
        {
            string outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "RenderFromInMemoryGraph.png");

            const string graph = @"digraph G { node [style=filled, shape=rect]

# Nodes
""String age"" -> ""Int64.Parse()"" -> {""Int64"", ""Collector""}


# Formatting
""String age"" [color=green]
""Int64"" [color=""#9fbff4""]
""Int64.Parse()"" [shape=invhouse]
""Collector"" [color = ""#c361f4""]

{ rank=same; ""Int64.Parse()"", ""Collector""}


}";
            var graphViz = new GraphViz();


            graphViz.LayoutAndRenderDotGraph(graph, outputFilePath, "png");

            Approvals.VerifyFile(outputFilePath);
        }

        [TestMethod]
        [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
        public void RenderFromInMemoryGraph()
        {
            const string graph = @"digraph G { node [style=filled, shape=rect]

# Nodes
""String age"" -> ""Int64.Parse()"" -> {""Int64"", ""Collector""}


# Formatting
""String age"" [color=green]
""Int64"" [color=""#9fbff4""]
""Int64.Parse()"" [shape=invhouse]
""Collector"" [color = ""#c361f4""]

{ rank=same; ""Int64.Parse()"", ""Collector""}


}";
            var graphViz = new GraphViz();


            byte[] result = graphViz.LayoutAndRenderDotGraph(graph, "png");

            Approvals.VerifyBinaryFile(result, ".png");
        }

        [TestMethod]
        [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
        public void RenderFromGraphInFileToFile()
        {
            string outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "RenderFromGraphInFile.png");

            const string graph = @"digraph G { node [style=filled, shape=rect]

# Nodes
""String age"" -> ""Int64.Parse()"" -> {""Int64"", ""Collector""}


# Formatting
""String age"" [color=green]
""Int64"" [color=""#9fbff4""]
""Int64.Parse()"" [shape=invhouse]
""Collector"" [color = ""#c361f4""]

{ rank=same; ""Int64.Parse()"", ""Collector""}


}";
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "RenderFromInGraphInFile.dot");
            File.WriteAllText(inputFilePath, graph);

            var graphViz = new GraphViz();


            graphViz.LayoutAndRenderDotGraphFromFile(inputFilePath, outputFilePath, "png");

            Approvals.VerifyFile(outputFilePath);
        }

        [TestMethod]
        [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
        public void RenderFromGraphInFile()
        {
            const string graph = @"digraph G { node [style=filled, shape=rect]

# Nodes
""String age"" -> ""Int64.Parse()"" -> {""Int64"", ""Collector""}


# Formatting
""String age"" [color=green]
""Int64"" [color=""#9fbff4""]
""Int64.Parse()"" [shape=invhouse]
""Collector"" [color = ""#c361f4""]

{ rank=same; ""Int64.Parse()"", ""Collector""}


}";
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "RenderFromInGraphInFile.dot");
            File.WriteAllText(inputFilePath, graph);

            var graphViz = new GraphViz();


            byte[] result = graphViz.LayoutAndRenderDotGraphFromFile(inputFilePath, "png");

            Approvals.VerifyBinaryFile(result, ".png");
        }
    }
}