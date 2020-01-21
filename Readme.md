.NET Core Plugins Host Web API Service
=================
AECCWebAPI is a web service which dynamically can load and host multiple plugin libraries which can themselves act as isolated Web API services.
This project is useful if you have multiple Web API services which are developed independently and small enough to not need to be deployed them separately.

## Getting started
There are four projects in the source code:
- AECCWebAPI: this is the main application. This is main web service which will dynamically load the plugins on the startup.
- McMaster.NETCore.Plugins: this project provides API for loading .NET Core assemblies dynamically, executing them as extensions to the main application, and finding and isolating the dependencies of the plugin from the main application. See https://github.com/natemcmaster/DotNetCorePlugins. 
- Plugin.Abstraction: this project provides the base classes of for the plugins.
- AECC.Plugin1 is a demo project to show how to create a simple plugin to host inside the parent web service.

## Creating a new plugin
You can use the provided demo as a template for your new plugins. ALternatively,  The following steps provide a guide to create a new plugin:
- Create a new project named AECC.[pluginname] (e.g. AECC.Plugin1) using “ASP.Net core web app” targeting  core 2.1. The wwwroot included can be removed.
- Add a reference to Plugin.Abstraction project.
- Add <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath> line to the <PropertyGroup> in the project file. 
- Add PlugInServices.cs from the templates folder to the project.
- Replace Startup.cs with the one in the templates folder.
- Under the controllers folder, add the Plugin1Controller.cs from the templates folder and rename the class as [pluginname]Controller.
- Under the Views folder, add a new folder named [pluginname] and inside add a link to _Help.cshtml Plugin.Abstraction and add Help.cshtml from the templates folder. 

## Deployment of plugin
- Build your plugin.
- Copy the plugin complied files in the bin folder and any dependencies to a folder named AECC.[pluginname] under libs found in wwwroot of the main service.
- Copy any help files required for the plugin documentation to wwwroot\libs\AECC.[pluginname]\resources\help.   
- Publish the libs folder and restart the main service.

To test the service go, call an action on the plugin controller using http://.../api/[controllername]/[action]
