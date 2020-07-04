namespace GraphVizNet
{
    /// <summary>
    ///     GraphViz configuration.
    /// </summary>
    public class GraphVizConfig
    {
        /// <summary>
        ///     Default GraphViz configuration.
        /// </summary>
        public static GraphVizConfig Default
        {
            get { return new GraphVizConfig {TimeoutInMs = 30000, TreatWarningsAsErrors = false}; }
        }

        /// <summary>
        ///     Specifies the timeout (in milliseconds) for the launched process.
        /// </summary>
        public int TimeoutInMs { get; set; }

        /// <summary>
        ///     True if rendering throws exception if there are any warnings, false otherwise.
        /// </summary>
        public bool TreatWarningsAsErrors { get; set; }
    }
}