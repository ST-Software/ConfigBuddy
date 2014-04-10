# ConfigBuddy

Two extensions for .NET framework projects which enable easier management of your projects configurations. 

The idea is to have one "library/app" assembly which includes all configuration values (key/value) in the hierarchy that make sense for your deployment.

- local
	- doe.j
	- roe.j
- developlment
	- integration_tests
	- preview
- production
	- customer1
	- customer2

Inside your project you have ConfigurationTemplates directory where you put all the configuration files that should be processed in the same folder hierarchy as it should be placed in the root folder. These should include suffix ".templates" to be recognized.

- ConfigurationTemplates
	- web.config.template	
	- bin
		- hibernate.xml

The extensions handles following scenario
1. generates project specific configuration before build. This is good for local development where every developer can have its own settings
2. generates configuration for all projects and all configuration sets. This can be used during deployment for picking the correct configuration.

## ConfigBuddy.Configurations

This package disable default build process and generates configurations for all configuration sets/projects to its output instead.

## ConfigBuddy.Project

This package should be installed in the project with custom configuration templates. The package before each build processed these templates and generate "real" configuration files.

## How to use it

Create a new library project "Configurations" in your solution. Install the ConfigBuddy.Configurations to this "Configurations" project

```
Install-Package ConfigBuddy.Configurations
```

You should see the 
- configbuddy.configurations.xml - to be added to your confiugration project which includes basic settings
- config.xml - file including key/value example pairs

You can create folder structure similar to the example above with local, development, production subfolders and copy over the config.xml to specify unique value for this kind of configurations.

Please notes that the folder *local/(user name)* will be used for generating configuration for local development (in before build phase) in projects where ConfiguBuddy.Project is installed.

In your projects where the configuration should be applied to install ConfigBuddy.Project

```
Install-Package ConfigBuddy.Project
```

This will creates configbuddy.project.xml file which includes path to Configuration project.

You need to manually add the project name and relative path to Configurations/configbuddy.configurations.xml file to the projects section. This is not done automatically for you now.

Create a "ConfigurationTemplates" folder and copy there all your configuration files (the structure needs to be preserve that means if you have bin/hibernate.xml it should be placed to ConfigurationTemplates/bin/hibernate.xml and so on) add ".template" sufix to all of them.

Now you can use in these files $(ConfigurationKey) placeholders which will be replaced in prebuild with correct values and processed files will be placed in the project root.