using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GraphVizNet
{
    /// <summary>
    ///     The GraphViz wrapper class.
    /// </summary>
    public class GraphViz
    {
        /// <summary>
        ///     Creates a new GraphViz wrapper instance.
        /// </summary>
        public GraphViz()
        {
            Config = GraphVizConfig.Default;
        }

        /// <summary>
        ///     The GraphViz configuration.
        /// </summary>
        public GraphVizConfig Config { get; }

        /// <summary>
        ///     Layouts and renders a graph in DOT format from a file.
        /// </summary>
        /// <param name="graphFilePath">Path to graph file.</param>
        /// <param name="outputFilePath">Path to output file.</param>
        /// <param name="outputFormat">
        ///     The output format, if it's null it will default to "dot". See
        ///     https://www.graphviz.org/doc/info/output.html for more output formats.
        /// </param>
        public void LayoutAndRenderDotGraphFromFile(string graphFilePath, string outputFilePath, string outputFormat)
        {
            if (graphFilePath == null)
            {
                throw new ArgumentNullException(nameof(graphFilePath));
            }

            if (outputFilePath == null)
            {
                throw new ArgumentNullException(nameof(outputFilePath));
            }

            LayoutAndRender(graphFilePath, null, outputFilePath, null, outputFormat);
        }

        /// <summary>
        ///     Layouts and renders a graph in DOT format from a file.
        /// </summary>
        /// <param name="graphFilePath">Path to graph file.</param>
        /// <param name="outputFormat">
        ///     The output format, if it's null it will default to "dot". See
        ///     https://www.graphviz.org/doc/info/output.html for more output formats.
        /// </param>
        /// <returns>The output.</returns>
        public byte[] LayoutAndRenderDotGraphFromFile(string graphFilePath, string outputFormat)
        {
            if (graphFilePath == null)
            {
                throw new ArgumentNullException(nameof(graphFilePath));
            }

            return LayoutAndRender(graphFilePath, null, null, null, outputFormat);
        }

        /// <summary>
        ///     Layouts and renders a graph in DOT format.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="outputFilePath">Path to output file.</param>
        /// <param name="outputFormat">
        ///     The output format, if it's null it will default to "dot". See
        ///     https://www.graphviz.org/doc/info/output.html for more output formats.
        /// </param>
        public void LayoutAndRenderDotGraph(string graph, string outputFilePath, string outputFormat)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (outputFilePath == null)
            {
                throw new ArgumentNullException(nameof(outputFilePath));
            }

            LayoutAndRender(null, graph, outputFilePath, null, outputFormat);
        }

        /// <summary>
        ///     Layouts and renders a graph in DOT format.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="outputFormat">
        ///     The output format, if it's null it will default to "dot". See
        ///     https://www.graphviz.org/doc/info/output.html for more output formats.
        /// </param>
        /// <returns>The output.</returns>
        public byte[] LayoutAndRenderDotGraph(string graph, string outputFormat)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            return LayoutAndRender(null, graph, null, null, outputFormat);
        }

        /// <summary>
        ///     Layouts and renders a graph.
        /// </summary>
        /// <param name="graphFilePath">Path to graph file, can be null but then the graph must not be null.</param>
        /// <param name="graph">The graph, can be null but then the graph file path must not be null.</param>
        /// <param name="outputFilePath">Path to output file. If it's null, the return will have the command line output.</param>
        /// <param name="layoutAlgorithm">
        ///     The layout algorithm, if it's null it will default to "dot". See
        ///     https://www.graphviz.org/pdf/dot.1.pdf for more layout algorithms.
        /// </param>
        /// <param name="outputFormat">
        ///     The output format, if it's null it will default to "dot". See
        ///     https://www.graphviz.org/doc/info/output.html for more output formats.
        /// </param>
        /// <param name="extraCommandLineFlags">Any extra command line flags. See https://www.graphviz.org/doc/info/command.html </param>
        /// <returns>The output of the command line.</returns>
        public byte[] LayoutAndRender(string graphFilePath, string graph, string outputFilePath, string layoutAlgorithm, string outputFormat,
            params string[] extraCommandLineFlags)
        {
            if (graphFilePath == null && graph == null)
            {
                throw new ArgumentException($"Arguments {nameof(graphFilePath)} and {nameof(graph)} cannot be null at the same time.");
            }

            string arguments = BuildCommandLineArguments(graphFilePath, outputFilePath, layoutAlgorithm, outputFormat, extraCommandLineFlags);

            var graphVizProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = "graphviz/dot",
                    Arguments = arguments,
                    WorkingDirectory = "graphviz",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                }
            };

            graphVizProcess.Start();

            if (graph != null)
            {
                graphVizProcess.StandardInput.Write(graph);
                graphVizProcess.StandardInput.Close();
            }

            byte[] result;
            using (Stream baseStream = graphVizProcess.StandardOutput.BaseStream)
            using (var memoryStream = new MemoryStream())
            {
                baseStream.CopyTo(memoryStream);
                result = memoryStream.ToArray();
            }

            string errorAndWarningMessages = graphVizProcess.StandardError.ReadToEnd();

            graphVizProcess.WaitForExit(Config.TimeoutInMs);

            if (graphVizProcess.ExitCode != 0)
            {
                throw new GraphVizException($"dot process exited with code: {graphVizProcess.ExitCode} and with errors: {errorAndWarningMessages}");
            }

            if (Config.TreatWarningsAsErrors && !string.IsNullOrEmpty(errorAndWarningMessages))
            {
                throw new GraphVizException($"dot process exited with code: {graphVizProcess.ExitCode} but with warnings: {errorAndWarningMessages}");
            }

            return result;
        }

        /// <summary>
        ///     Utility function to build command line arguments for the dot executable.
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="layout"></param>
        /// <param name="format"></param>
        /// <param name="extraCommandLineFlags"></param>
        /// <returns></returns>
        public static string BuildCommandLineArguments(string inputFilePath, string outputFilePath, string layout, string format,
            params string[] extraCommandLineFlags)
        {
            var argumentsBuilder = new StringBuilder();

            if (layout != null)
            {
                argumentsBuilder.Append(" -K").Append(layout);
            }

            if (format != null)
            {
                argumentsBuilder.Append(" -T").Append(format);
            }

            if (outputFilePath != null)
            {
                argumentsBuilder.Append(" -o\"").Append(outputFilePath).Append('\"');
            }

            foreach (string extraFlag in extraCommandLineFlags)
            {
                argumentsBuilder.Append(' ').Append(extraFlag);
            }

            if (inputFilePath != null)
            {
                argumentsBuilder.Append(" \"").Append(inputFilePath).Append('\"');
            }

            return argumentsBuilder.ToString();
        }
    }
}