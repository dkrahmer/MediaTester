# MediaTester

MediaTester can test any media (SD, microSD, thumb drives, memory sticks, etc.) and verify it stores the expected number of bytes. The testing method is similar to h2testw but MediaTester can detect fake or defective media much faster by performing quick reads as data is written.

Fake media sales have reached epidemic proportions. Fake media is being falsely labled with popular brand names including SanDisk, Samsung, Sony, Kingston, and others. Not only are these counterfeit but they often contain less storage than the cards report to the computer. A 128GB SD card may actually contain only 8GB or 4GB of actual space. After filling up all of the real space, the card will respond as if it is storing the data but it is actually throwing it away. The files will look like they exist but they are actually full of null bytes or completely corrupted. If you try to read the data you will find that it is ALL GONE.

MediaTester is released as a free public service to help stop counterfeiters and fraudsters. If you buy any storage media, use MediaTester to verify it or risk losing your data.

The MediaTester library, Windows GUI, and CLI are released with a generous public domain open source license with no restrictions.

Enjoy!

-Doug Krahmer

## Install

MediaTester is a portable and compact executable, and it will work with the latest versions of Windows 10, without installing any additional software. This executable targets .NET 4.7.2, which is included by default since Windows 10 Version 1809. 

If you want MediaTesterCli, you have to install .NET 6 (only the Runtime is needed, [download link](https://dotnet.microsoft.com/en-us/download/dotnet/6.0). ), because actually is not installed by default in Windows 10. It is available for Windows, Mac OS and Linux.

## Build Releases

### MediaTester (GUI)

Open the solution in Visual Studio, install the dependencies through Nuget and compile it.

### MediaTesterCLI

The final releases are generated from command line using `dotnet` tool. Open the Terminal directly in Visual Studio (View > Terminal) and launch the following command:

    dotnet publish <ProjectName> -r <RuntimeIdentifier> -c Release -p:PublishSingleFile=true --no-self-contained -p:PublishReadyToRun=true

where `ProjectName` is `MediaTesterCLI`; `RuntimeIdentifier` is the target platform (CPU and OS) as specified [here](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).

> :information_source: MediaTesterCli is actually a test app, so it is not yet included in the official release. However, it was successfully tested on Raspberry OS (Bullseye, `linux-arm`) and, of course, on Windows 10. You should be able to compile MediaTesterCli for whatever architecture supported by .NET 6.

## Development

This repository contains 3 projects:

- MediaTesterLib: provides the core functionalities of the MediaTester apps, and it build on top .netstandard 2.0 to guarantee the maximum compability among the different .NET families.

- MediaTesterCli: the command-line fronted, for every architecture supported by .NET6.

- MediaTester: the GUI frontend. This project is based on WinForms for .NET Framework 4.7.2, for the best compatibility with the latests versions of Windows 10/11.

The code is developed in VS2022 and automatically styled with CodeMaid.

### Debug and Testing

I suggest to use [HxD](https://mh-nexus.de/en/hxd/), a fast and free hex editor, to alter the test files when you not have a fake media at hand.
