# Contributing to MediaTester

First off, thanks for taking the time to contribute!

The following is a set of guidelines for contributing to MediaTester.

#### Table Of Contents

[How Can I Contribute?](#how-can-i-contribute)
  * [Adding a Language Translation](#adding-a-language-translation)

## How Can I Contribute?

### Adding a Language Translation

If you would like to request or provide a language translation, first search the [issues](https://github.com/dkrahmer/MediaTester/issues) to see if there is an existing request.
If you cannot find an issue related to the language you are interested in, go ahead and create a new issue.

#### Steps to add a New Language Translation to the Windows GUI Application

	1. Create a fork of the repo and create a new feature branch in your fork.
	2. Open the solution in Visual Studio 2019.
	3. Update text on the Main form.
		1. Open the Main.cs form by double-clicking.
		2. Click on the top of the Main form then open the properties.
		3. Change the Language property from "(Default)" to the language you want to add.
		4. Make a few changes then save to verify a new "Main.??.resx" file is created.
		   - Any changes made to the form now apply to that language.
		   - You may adjust sizes if necessary but try to choose translations that fit in the same size space if possible to prevent integration complications later.
        5. Continue 
    4. Update the text values used in code.
		1. Add a new Language resource file.
		   - Right-click the MediaTesterGui project > Add > New Item... > Resource File
		   - Name the it Strings.??.resx - where ?? is the two character ISO code for the language. Use the same code from the file generated for the Main form.
		   - Click: Add
        2. Copy the default strings
		   - Open Strings.resx
		   - Select all rows by pressing ctrl-a
		   - Copy with ctrl-c
		   - Open the new Strings.??.resx
		   - Paste with ctrl-v
		   - Delete the first line containing String1
        3. Edit the string values for the new language then save.
    5. Be sure to test your changes to ensure they work before submitting a pull request!
