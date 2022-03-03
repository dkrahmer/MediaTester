# MediaTester

MediaTester can test any media (SD, microSD, thumb, etc) and verify it stores the expected number of bytes. The testing method is similar to h2testw fake or defective media can be identified much faster by performing quick reads as data is written.

Fake media sales have reached epidemic proportions. Fake media is being falsely labeled with popular brand names including SanDisk, Samsung, Sony, Kingston, and others. Not only are these counterfeits but they often contain less storage than the cards report to the computer. A 128GB SD card may actually contain only 8GB or 4GB of actual space. After filling up all of the real space, the card will respond as if it is storing the data but it is actually throwing it away. The files will look like they exist but they are actually full of null bytes or completely corrupted. If you try to read the data you will find that it is all gone.

MediaTester is released as a free public service to help stop counterfeiters and fraudsters. If you buy any storage media, you should use MediaTester to verify it or risk losing your data.

The MediaTester library, GUI, and CLI are released with a generous open source license with no restrictions.

-Doug Krahmer

## Install

MediaTester is a portable and compact executable, but it requires .NET 6 Desktop Runtime, which actually is not installed by default in Windows. 

If you just want MediaTesterCli, you have to install .NET Runtime which is available for Windows, Mac OS and Linux.

You can download the latest release of .NET 6 (Desktop) Runtime directly from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

## Build Releases

The final releases are generated from command line using `dotnet` tool. Open the Terminal directly in Visual Studio (View > Terminal) and launch the following command:

    dotnet publish <ProjectName> -r <RuntimeIdentifier> -c Release -p:PublishSingleFile=true --no-self-contained -p:PublishReadyToRun=true

where `ProjectName` can be `MediaTesterCLI` or `MediaTester`; `RuntimeIdentifier` is the target platform (CPU and OS) as specified [here](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).

> :information_source: MediaTester is distributed only for `win-x86` and `win-x64`, while MediaTesterCli is actually a test app, so it is not yet included in the official release. However, it was successfully tested on Raspberry OS (Bullseye, `linux-arm`) and on Windows 10. You should be able to compile MediaTesterCli for whatever architecture supported by .NET 6.

## Development

This repository contains 3 projects:

- MediaTesterLib: provides the core functionalities of the MediaTester apps.

- MediaTesterCli: the command-line fronted, for every architecture supported by .NET6.

- MediaTester: the GUI frontend. This project is based on WinForms, so it is Windows-only. 

The code is developed in VS2022 and automatically styled with CodeMaid.

### Debug and Testing

I suggest to use [HxD](https://mh-nexus.de/en/hxd/), a fast and free hex editor, to alter the test files when you not have a fake media at hand.
