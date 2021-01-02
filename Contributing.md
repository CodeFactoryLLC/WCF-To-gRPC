# Contributing to CodeFactory Command Automation Packages 
(under contruction)
### First off, thanks for taking the time to contribute to the WCF-to-gRPC conversion template! 

The following is a set of guidelines for contributing to CodeFactory and its open source community-sourced automation packages, like this one, which are highlighted and linked on the [Public Projects](https://github.com/CodeFactoryLLC/Public-Projects) repo on GitHub. These are mostly guidelines, not rules. Use your best judgment, and feel free to propose changes to this document in a pull request. 

### Code of Conduct 

This project and everyone participating in it is governed by the CodeFactory [Code of Conduct](https://github.com/CodeFactoryLLC/Public-Projects/blob/master/Code_Of_Conduct.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to support@codefactory.software. 

***
#### I don't want to read this whole thing I just have a question!!! 

Note: Please don't file an issue to ask a question. You'll get faster results by posting your ideas and questions on our [Discussion Forums](https://github.com/CodeFactoryLLC/WCF-To-gRPC/discussions) (located within this repo).
***

### Reporting Bugs 

This section guides you through submitting a bug report for open source content. Following these guidelines helps maintainers and the community understand your report, reproduce the behavior, and find related reports. 

Before creating bug reports, please check [this list] as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible. Fill out the required template, the information it asks for helps us resolve issues faster. 

Note: If you find a Closed issue that seems like it is the same thing that you're experiencing, open a new issue and include a link to the original issue in the body of your new one. 

### Before Submitting A Bug Report 

Check the FAQs on the forum for a list of common questions and problems. 

Determine which repository the problem should be reported in. 

Perform a cursory search to see if the problem has already been reported. If it has and the issue is still open, add a comment to the existing issue instead of opening a new one. 

# How Can I Contribute? 

### Accessing #HelpWanted Issues
We've created a few issues tagged with #HelpWanted to indicate where we think there would be significant interest once an automation has been authorided and published. We monitor the [Issue List](https://github.com/CodeFactoryLLC/Public-Projects/issues) for any questions or support needs you might have to get started building your own custom CodeFactory Automation Command packages.

### Build New or Improve Existing Packages?

A good primer on how to build a publish your own CodeFactory Automation Command is available [here](http://docs.codefactory.software/guidance/usage-intro.html).

When [Designing Software Factories with CodeFactory](http://docs.codefactory.software/guidance/design/intro.html)
the possibilities of code automation are only limited by the .NET framework and the imagination of the automation template author. 


# What should I know before I get started? 

### CodeFactory and Community-authored Automation Packages 

While the CodeFactory RT client is a commercial product, a wide variety of open-source Automation Commands are publicly available for you to use, clone, modify, etc.  

In order to author a new automation template, or modify an existing template, you will need to have both an active CodeFactory for Visual Studio client installed (as an extension into your copy of Visual Studio 2019) and the CodeFactory SDK into the same instance of Visual Studio.

* [Request Trial Copy of CodeFactory for Visual Studio and the SDK Here](https://www.codefactory.software/try-codefactory)

Once these two dependencies are taken care of, you will have the ability to create a "File -> New -> CodeFactory - Commands Library project". This template along with its sister, CodeFactory - Automation Library are included with Visual Studio from the CodeFactory SDK installer.

### What is a CodeFactory "Automation Command"?

The way in which a CodeFactory Automation template works with your code is via one or more Commands that are defined within a Command Library. Currently - there are seven (7) different kinds of commands that CodeFactory makes available to authors. 

#### Command Types
Name | Description
-----|-------
Solution | Like its name suggests, this command is used when an author wishes to begin code automation logic while starting at the top-most node (ie. Solution) of the Visual Studio Solution Explorer. Please see here for more details about this command.
Solution Document | This command is used to begin a command from any document that lives at the Solution level within Visual Studio. Please see Solution Document for more details.
Solution Folder | This command sets the context for a code automation at a folder node within the Visual Studio Explorer hierarchy. Please see Solution Folder for more details.
Project | This command sets the context for a code automation at the Project node within the Visual Studio Solution Explorer window. Please see Project Command for details.
Project Folder | This command sets the context for a code automation at Folder within a Project node in the Visual Studio Solution Explorer. Please see Project Folder for more details.
Project Document | This command is for any node within a Project hierarchy that is not a folder or the project node itself. Project documents can be anything that you are allowed to add into a project per normal Visual Studio rules. Examples include; xml, config, png, bmp, js, html, java, ps, etc. Please see Project Document for more details.
CSharp Source | This command is a special case of the Project Document case. CodeFactory will pass into your command class a real-time model of any C# source code artifacts found within a *.cs file in the target Solution. Usually these files are found under a Project Node within the Solution Explorer - but not always. Please see C# Source for more details.

[Learn more here](http://docs.codefactory.software/guidance/overview-commands-intro.html).
