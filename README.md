# template-repo
Recommended GitHub configurations

# .Net Code Analysis

This document gives an introduction to how to setup .net core projects with default code analysis settings and configuration setup with visual studio IDE and CI pipelines.


## [# Overview of .NET source code analysis ](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview?tabs=net-7)

.NET compiler platform (Roslyn) analyzers inspect your C# or Visual Basic code for code quality and style issues. Starting in .NET 5, these analyzers are included with the .NET SDK and you don't need to install them separately. If your project targets .NET 5 or later, code analysis is enabled by default. If your project targets a different .NET implementation, for example, .NET Core, .NET Standard, or .NET Framework, you must manually enable code analysis by setting the [EnableNETAnalyzers](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#enablenetanalyzers) property to `true`.

### How it works ?

If rule violations are found by an analyzer, they're reported as a suggestion, warning, or error, depending on how each rule is [configured](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-options). Code analysis violations appear with the prefix "CA" or "IDE" to differentiate them from compiler errors.

-   For a list of rules that are included with each .NET SDK version, see  [Analyzer releases](https://github.com/dotnet/roslyn-analyzers/blob/main/src/NetAnalyzers/Core/AnalyzerReleases.Shipped.md).
-   For a list of all the code quality rules, see  [Code quality rules](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/).

### [What is AnalysisMode  & Analysis Category](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#analysismode)

Introduced in .NET 6, this property is the same as [AnalysisMode](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#analysismode), except that it only applies to a specific [category of code-analysis rules](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories). This property allows you to enable or disable rules at a different level to the other rule categories. If you omit this property for a particular category of rules, it defaults to the [AnalysisMode](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#analysismode) value. The available values are the same as those for [AnalysisMode](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#analysismode).
 
 
#### How to Setup  ?

 - open project file (.csproj) , setup below properties for apply a code style category analysis. 
 
 - set below settings on project file for apply a AnalysisModeStyle (IDE rules) 
 - please review this [link](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#analysismode) for what is use of each below properties.
 
```
<EnforceCodeStyleInBuild>True</EnforceCodeStyleInBuild>
<EnableNETAnalyzers>true</EnableNETAnalyzers>
<CodeAnalysisTreatWarningsAsErrors>true</CodeAnalysisTreatWarningsAsErrors>
<TreatWarningsAsErrors>true</TreatWarningsAsErrors> 
<AnalysisModeStyle>Minimum</AnalysisModeStyle>
```
 
#### Example Repository  ?

here is example repository has setup github repository actions which failed a  pull request if any code style error with minimum analysis mode is violated.

https://github.com/Thoughtfocus-arb/aws-lamda-code-style

#### GitHub Actions 

below are basic build action of dotnet build action which automatically trigger a any code style errors as per settings we did setup. 

```

    # Run .NET Code Analysis
    - name: Run .NET Code Build
      run : dotnet build .\aws_lambda.sln --configuration Release
 

```

### Important Notes and References 

1. .NET analyzers are target-framework agnostic. That is, your project does not need to target a specific .NET implementation. The analyzers work for projects that target .NET 5+ as well as earlier .NET versions, such as .NET Core 3.1 and .NET Framework 4.7.2. However, to enable code analysis using the [EnableNETAnalyzers](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#enablenetanalyzers) property, your project must reference a [project SDK](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/overview).

2. you can add any rules which is not applicable then add it into exception list ( it will be generate .editorconfig file and use same)

References 

https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview?tabs=net-7#enable-additional-rules

https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#analysismode

https://stackoverflow.com/questions/69680054/how-do-i-run-code-analysis-for-net-application-using-microsoft-codeanalysis-net


https://endjin.com/blog/2022/01/raising-coding-standard-dotnet-analyzers
