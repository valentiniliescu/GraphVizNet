# GraphVizNet

Wrapper for the command-line GraphViz native binaries (version 2.44.0). The native binaries are included and deployed with this package.

# Usage

See unit tests for usage

# Launch process vs PInvoke

There are two ways of integrating GraphViz native binaries: launching the "dot" tool in a new process or using PInvoke. This library uses the former approach, see [https://github.com/valentiniliescu/GraphVizNet-PInvoke](https://github.com/valentiniliescu/GraphVizNet-PInvoke) for my experimentation with the latter.
