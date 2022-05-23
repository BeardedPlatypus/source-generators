<p align='center'><img align='center' src='https://raw.githubusercontent.com/BeardedPlatypus/source-generators/docs/setup-initial-docs/.docs/icon.svg' width='25%'></p>

# BeardedPlatypus.SourceGenerators

`BeardedPlatypus.SourceGenerators` provides several C\# source generators to
automatically extend your source code with useful software patterns.

Currently, the following patterns are implemented:

* [Visitor Pattern](BeardedPlatypus.SourceGenerators.Annotations/Visitor/README.md)

## Motivation - A small collection of C\# Source Generators to reduce boilerplate

Software patterns define proven approaches to solve common problems in
software programs. Implementing such patterns however, often requires writing some
boilerplate, which can be cumbersome or error-prone. With a bit of meta-programming
it is possible to automate the implementation of such patterns, thus reducing the 
amount of boilerplate written by a developer. 

`BeardedPlatypus.SourceGenerators` provides the implementation of several patterns
through the use of [C\# source generators](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview),
which can auto-magically generate source code based on the syntax trees build during
compilation.

This project is a first step into the world of source generators, as such it is
a learning project which has allowed me to evaluate the feasibility of integrating
such source generators in (work) projects. 

## Installation - Add the `BeardedPlatypus.SourceGenerators` NuGet packages to your project

The `BeardedPlatypus.SourceGenerators` repository consists of two NuGet packages:

* `BeardedPlatypus.SourceGenerators`: provides the source generators doing the heavy lifting
* `BeardedPlatypus.SourceGenerators.Annotations`: provides the custom [C\# Attributes](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/) used to control the behaviour of the source generators.

Both of these should be added through the NuGet manager to the project which should make use
of the source generators.

For additional information on how to add these packages to your project, please look at
[github's documentation](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry).

## Usage - Add attributes to generate source code

The different source generators can be controlled by adding the appropriate attributes to the
project. Sample usage can be found in the `BeardedPlatypus.SourceGenerators.Samples` project.

## License - MIT

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## References

* [Introducing C\# Source Generators (article)](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)
* [Source Generators Cookbook (article)](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md)
* [kant2002/SourceGeneratorsKit (repository)](https://github.com/kant2002/SourceGeneratorsKit)
