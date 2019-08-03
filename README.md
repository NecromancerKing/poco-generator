# Poco Generator

A command line tool to generate .net POCO(Plain Old CLR Objects) classes from a standard Json Schema.

# Goal

The motivation behind creating this CLI tool is to facilitate model object creation when a .Net project/microservice is given a Json Schema for contracts and the entity domain classes need to be created based on those schema.  
The mentioned task is not very straightforward and typically involves a lot of manual work which can be easily avoided using a tool like this. And the whole idea behind this repository is to lift that burden.

# Technical Overview

This project is a .Net core console application and will generate the output given the necessary arguments via command line.

### Command line parameters

There are 3 required parameteres to this CLI, and 2 optional switches for other purposes. The parameters are as follows:

<li>namespace: the namespace under which you want to place your classes.</li>
<li>jsonpath: the path to the Json schema file</li>
<li>folderpath: the path to folder you want to place your classes in. The folder doesn't need to exist, but the rest the path should be present.</li>
<li>version: will declare the CLI tool's versrion. This switch should be used alone</li>
<li>help: will show the list of switches, their required status, and some usage examples. This switch should be used alone.</li>

#### Sample commands

=> `PocoGenerator --help`  
PocoGenerator 1.0.0  
USAGE:  
Absolute Path:  
 PocoGenerator --folderpath C:\Folder\NamedFolder --jsonpath C:\SomeFolder\schema.json --namespace Base.Child  
Relative Path:  
 PocoGenerator --folderpath ..\..\NamedFolder --jsonpath ..\schema.json --namespace Base.Child

--namespace Required. Namespace for the Poco classes

--jsonpath Required. Path to the json schema file

--folderpath Required. Path to the output folder

--help Display this help screen.

--version Display version information.

=> `PocoGenerator --namespace=MocksService.Controllers --jsonpath=../UserInfo.json --folderpath=../UserInfo`  
done!

=> `PocoGenerator --namespace=MocksService.Controllers --jsonpath=../UserInfo.json`  
PocoGenerator 1.0.0

ERROR(S):
Required option 'folderpath' is missing.
USAGE:
Absolute Path:
PocoGenerator --folderpath C:\Folder\NamedFolder --jsonpath C:\SomeFolder\schema.json --namespace Base.Child
Relative Path:
PocoGenerator --folderpath ..\..\NamedFolder --jsonpath ..\schema.json --namespace Base.Child

--namespace Required. Namespace for the Poco classes

--jsonpath Required. Path to the json schema file

--folderpath Required. Path to the output folder

--help Display this help screen.

--version Display version information.

CommandLine.MissingRequiredOptionError

### How to publish the tool binary executable after changes

This project is developed under .net core 3.0 and is using AOT compilation mechanism to generate a single native executable for the target operating system. The condition to detect the host OS are configured in the project file and to generate the output you will only need to run the command below under the solution path. The binary file will be put under Publish folder under target OS folder. The supported runtimes are `win-x64`, `linux-x64`, and `osx-x64`.
You may or may not need C++ desktop sdk to get the native binaries. If you reveiced an error indicating that, merely install the C++ desktop package from Visual Studio setup. Installing .net core 3.0 runtime and sdk though, is a requirement. If you have the generated exe and only want to use it, you won't need to do any of the steps above since it's a native exe and has no dependency to any runtime. Hence this tool can be utilized in automation tools and docker files without hassle.

`dotnet publish -c Release`
