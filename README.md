# Windows Registry Write Blocker

## Disclaimer
This software has been tested to work on a Windows 10 Home laptop with a Kingston 32GB USB. As Windows is constantly changing, it is highly important that you test this before use on any potential digital evidence. There are no express guarantees or warranties, both suggested or implied with this software.

## Installation
Download the latest version from the Releases tab. Alternatively you can download the repository and compile it from source with Visual Studio 2019. Please not that the program requires Administrative privileges to run due to the modification of HKLM registry keys.

## How it works

The program works by modifying several Registry HKLM keys.
> HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices
> HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control
These keys allow the user to prevent write access to USB devices.