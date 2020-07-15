# Windows Registry Write Blocker

## Disclaimer
This software has been tested to work on a Windows 10 Home laptop with a [Kingston DataTraveller G4 32GB USB](https://www.amazon.co.uk/Kingston-DTIG4-32GB-Traveler-Flash/dp/B00G9WHN12/), and an [Inatek 2.5" USB Caddy](https://www.amazon.co.uk/Optimized-Inateck-3-0-Portable-2-5-SATA-I-Additional/dp/B00KW4T69A/). As Windows is constantly changing, it is highly important that you test this before use on any potential digital evidence. There are no express guarantees or warranties, both suggested or implied with this software.

## Installation
Download the latest version from the [Releases tab](https://github.com/Stormy102/WindowsRegistryWriteBlocker/releases). Alternatively you can download the repository and compile it from source with Visual Studio 2019. Please not that the program requires Administrative privileges to run due to the modification of HKLM registry keys.

## How it works

The program works by modifying several Registry HKLM keys.

The most significant key modified is HKLM\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies\WriteProtect, as this enables the Windows 
The other keys modified enable the Group Policy found in Computer Configuration > Administrative Templates > System > Removable Storage Access > Removable Disks: Deny write access

> HKLM\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}\Deny_Write to 1
> HKLM\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}\Deny_Write to 1
> HKLM\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\Custom\Deny_Write\Deny_Write to 1
> HKLM\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\Custom\Deny_Write\List\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b} to {53f5630d-b6bf-11d0-94f2-00a0c91efb8b}
> HKLM\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\Custom\Deny_Write\List\{53f56307-b6bf-11d0-94f2-00a0c91efb8b} to {53f56307-b6bf-11d0-94f2-00a0c91efb8b}

> HKLM\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies\WriteProtect to 1

> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\DenyAllGPState to 1
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F56307-B6BF-11D0-94F2-00A0C91EFB8B}\EnumerateDevices to 1
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F56307-B6BF-11D0-94F2-00A0C91EFB8B}\AccessBitMask to 0
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F56307-B6BF-11D0-94F2-00A0C91EFB8B}\UserPolicy to 0
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F56307-B6BF-11D0-94F2-00A0C91EFB8B}\AuditPolicyOnly to 0
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F56307-B6BF-11D0-94F2-00A0C91EFB8B}\SecurityDescriptor to "D:(D;;DCLCRPCRSD;;;IU)(A;;FA;;;SY)(A;;FA;;;LS)(A;;0x1200a9;;;IU)"

> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F5630D-B6BF-11D0-94F2-00A0C91EFB8B}\EnumerateDevices to 1
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F5630D-B6BF-11D0-94F2-00A0C91EFB8B}\AccessBitMask to 0
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F5630D-B6BF-11D0-94F2-00A0C91EFB8B}\UserPolicy to 0
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F5630D-B6BF-11D0-94F2-00A0C91EFB8B}\AuditPolicyOnly to 0
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\{53F5630D-B6BF-11D0-94F2-00A0C91EFB8B}\SecurityDescriptor to "D:(D;;DCLCRPCRSD;;;IU)(A;;FA;;;SY)(A;;FA;;;LS)(A;;0x1200a9;;;IU)"
> HKLM\SYSTEM\CurrentControlSet\Control\Storage\HotplugSecurityDescriptor to the binary value 01000480000000000000000000000000140000000200580004000000010014001601010001010000000000050400000000001400ff011f0001010000000000051200000000001400ff011f0001010000000000051300000000001400a9001200010100000000000504000000
