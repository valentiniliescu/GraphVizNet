namespace GraphVizNet
{
    public class GraphVizConfig
    {
        public static GraphVizConfig Default
        {
            get { return new GraphVizConfig {TimeoutInMs = 30000, TreatWarningsAsErrors = false}; }
        }

        public int TimeoutInMs { get; set; }
        public bool TreatWarningsAsErrors { get; set; }
    }
}