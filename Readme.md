.NET Core Plugins Host Web API Service
=================
AECCWebAPI is a web service which dynamically can load and host multiple plugin libraries which can themselves act as isolated Web API services.
This project is useful if you have multiple Web API services which are developed independently and small enough to not need to be deployed separately.

## Getting started
There are four projects in the source code:
- AECCWebAPI: this is the shell application. 
- McMaster.NETCore.Plugins: this project provides API for loading .NET Core assemblies dynamically as plugins to the main application. See https://github.com/natemcmaster/DotNetCorePlugins. 
- Plugin.Abstraction: this project provides the base classes for the created plugins.
- AECC.Plugin1 is a demo project to show how to create a simple plugin to host inside the main application.

## Creating a new plugin
You can use the provided demo as a template for your new plugins. Alternatively,  the following steps provide a guide to create a new plugin:
- Create a new project named AECC.[pluginname] (e.g. AECC.Plugin1) using “ASP.Net core web app” targeting  core 2.1. The wwwroot included in the created project can be removed.
- Add a reference to Plugin.Abstraction project.
- Add PlugInServices.cs from the templates folder to the project.
- Replace Startup.cs with the one in the templates folder.
- Under the controllers folder, add the Plugin1Controller.cs from the templates folder and rename the class as [pluginname]Controller and edit this as necessary to add your API actions.
- Under the Views folder, add a new folder named [pluginname] and inside add a link to _Help.cshtml from Plugin.Abstraction project and also add Help.cshtml from the templates folder. 

## Deployment of plugin
- Build your plugin.
- Copy the plugin complied files in the bin folder and any dependencies to a folder named AECC.[pluginname] under wwwroot\libs in the main application.
- Copy any help files required for the plugin documentation to wwwroot\libs\AECC.[pluginname]\resources\help. See demo for an example.   
- Publish the libs folder and restart the main service.

To test the service go, call an action on the plugin controller using http://.../api/[controllername]/[action]
