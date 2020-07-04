using System;

namespace GraphVizNet
{
    /// <summary>
    ///     Models exceptions that happen during layout and rendering.
    /// </summary>
    public class GraphVizException : Exception
    {
        /// <summary>
        ///     Creates a GraphVizException instance with a message.
        /// </summary>
        /// <param name="message">The message.</param>
        public GraphVizException(string message) : base(message)
        {
        }
    }
}