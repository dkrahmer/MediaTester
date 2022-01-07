# MediaTester
MediaTester can test any media (SD, microSD, thumb, etc) and verify it stores the expected number of bytes. The testing method is similar to h2testw fake or defective media can be identified much faster by performing quick reads as data is written.

Fake media sales have reached epidemic proportions. Fake media is being falsely labeled with popular brand names including SanDisk, Samsung, Sony, Kingston, and others. Not only are these counterfeits but they often contain less storage than the cards report to the computer. A 128GB SD card may actually contain only 8GB or 4GB of actual space. After filling up all of the real space, the card will respond as if it is storing the data but it is actually throwing it away. The files will look like they exist but they are actually full of null bytes or completely corrupted. If you try to read the data you will find that it is all gone.

MediaTester is released as a free public service to help stop counterfeiters and fraudsters. If you buy any storage media, you should use MediaTester to verify it or risk losing your data.

The MediaTester library, GUI, and CLI are released with a generous open source license with no restrictions.

-Doug Krahmer

## Build Releases

The final releases are generated from command line using `dotnet` tool. Open the Terminal directly in Visual Studio (View > Terminal) and launch the following command:

	dotnet publish <ProjectName> -r <RuntimeIdentifier> -c Release -p:PublishSingleFile=true --no-self-contained -p:PublishReadyToRun=true

where `ProjectName` can be `MediaTesterCLI` or `MediaTester`; `RuntimeIdentifier` is the target platform (CPU and OS) as specified [here](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).

> :information_source: MediaTester is officially distribuited only for `win-x86`, `win-x64`, `linux-x64`, `linux-arm`, `linux-arm64`, and `osx-x64`.
