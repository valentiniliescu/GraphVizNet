# GraphVizNet

.NET wrapper for the command-line GraphViz native binaries. The binaries (version 2.44.0) are included and deployed with this package.

# Usage

To render an in-memory DOT graph to a PNG file
```csharp
var graphViz = new GraphViz();
graphViz.LayoutAndRenderDotGraph(graph, outputFilePath, "png");
```
To render an in-memory DOT graph to an in-memory PNG file
```csharp
var graphViz = new GraphViz();
byte[] renderedGraph = graphViz.LayoutAndRenderDotGraph(graph, "png");
```

To render a DOT graph from a file to a PNG file
```csharp
var graphViz = new GraphViz();
graphViz.LayoutAndRenderDotGraphFromFile(inputFilePath, outputFilePath, "png");
```
To render a DOT graph from a file to an in-memory PNG file
```csharp
var graphViz = new GraphViz();
byte[] renderedGraph = graphViz.LayoutAndRenderDotGraphFromFile(inputFilePath, "png");
```


For more flexible rendering use the `LayoutAndRender` method, with the parameters:
* `graphFilePath` - Path to graph file, can be null but then the graph must not be null.
* `graph`-The graph, can be null but then the graph file path must not be null.
* `outputFilePath` - Path to output file. If it's null, the returned byte array will have the command line output.
* `layoutAlgorithm` - The layout algorithm, if it's null it will default to "dot". See [https://www.graphviz.org/pdf/dot.1.pdf](https://www.graphviz.org/pdf/dot.1.pdf) for more layout algorithms.
* `outputFormat` - The output format, if it's null it will default to "dot". See [https://www.graphviz.org/doc/info/output.html](https://www.graphviz.org/doc/info/output.html) for more output formats.
* `extraCommandLineFlags` - Any extra command line flags. See [https://www.graphviz.org/doc/info/command.html](https://www.graphviz.org/doc/info/command.html).


These are the settings for the wrapper itself:
* `TimeoutInMs` - specifies the timeout (in milliseconds) for the launched process.
* `TreatWarningsAsErrors` - throws exception on rendering if there are any warnings.
```csharp
var graphViz = new GraphViz();
graphViz.Config.TreatWarningsAsErrors = true;
```


**If you create your own library or NuGet package that references the GraphVizNet package**, set the `PrivateAssets` on the package reference to explicitly exclude `contentFiles`, that will ensure the GraphViz binaries are copied to the output of projects that reference your library/NuGet. One example would be to set it to `none`
```xml
<ItemGroup>
  <PackageReference Include="GraphVizNet" Version="1.0.0" PrivateAssets="none" />
</ItemGroup>
```


# Launch process vs PInvoke

There are two ways of integrating GraphViz native binaries: launching the "dot" tool in a new process or using PInvoke. This library uses the former approach, see [https://github.com/valentiniliescu/GraphVizNet-PInvoke](https://github.com/valentiniliescu/GraphVizNet-PInvoke) for my experimentation with the latter.
