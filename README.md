# WCF-To-gRPC
A CodeFactory automation template that will help to migrate WCF services over to gRPC

This is an extenadable automation template designed for the purpose of migrating legacy WCF code to modern gRPC code.  This template is function and works specifically on the majority use case, which is plain vanilla 2-way WCF calls.  Please see the know limitations to understand how to extend this template.  This template is offered as open-source and anyone can download and alter it to suit their needs.

## How It Works

After compiling and running the automation template, load up any WCF solution and add a new gRPC project to the solution.  Right click on the gRPC project from the solution explorer and select Migrate Migrate WCF Services.  The automation template searches for all WCF contracts in the solution, then migrates them to gRPC.  It will create the *.proto files, the C# service classes/models.  The original WCF service code is left commented inside the new service methods for developers to finish (ie... mapping gRPC messages to WCF parameters and dealing with any WCF specific code)

## Known Limitations

The following is not included, but can easily be implemented.
- WCF security
- WCF one-way contracts or duplex contracts
- WCF Parameter to gRPC Message mapping.  Since gRPC uses messages as inputs and outputs we convert WCF parameters/return types into gRPC messages.  We do not map these messages back to the original parameter/return type as data mapping is outside the scope of this demo.
- gRPC specific *.proto files are not recognized as proto compilation files.  You must select the properties of the file, then set them to run under the proto compiler.  After that, you specify server/client/both as the output type.

## Alternative Use Cases

This implementation is designed at one time migration efforts to assist developers who will ultimately finish off the migration effort by hand.  It saves time on a majority of the code base while being simple to expand upon.  Alternatively this can also be set up as a gRPC code-first implementation.  By using attributes such as WCF attributes or custom attributes to define contracts, then use a CodeFactory template that generates the underlying code/proto files.  