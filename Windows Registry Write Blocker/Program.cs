/*
 * Windows Write Blocker - a program that modifies registry keys to prevent writing to removable USB devices
 * 
 * LICENCE: MIT Licence
 * 
 */

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;

namespace WindowsRegistryWriteBlocker
{
    public class Program
    {
        private static bool IsWriteBlocked
        {
            get
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    object writeProtect = key.GetValue("WriteProtect");
                    if (writeProtect == null || (int)writeProtect == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Windows Write Blocker (v.0.2)";

            if (!CheckRequirements())
            {
                Console.WriteLine("This program only works on Windows XP SP2 and higher");
                return;
            }

            Console.WriteLine("Welcome to Windows Registry Write Blocker v0.1-alpha");
            Console.WriteLine("This program block write access to the USB by modifying several");
            Console.WriteLine("Registry HKLM keys to make all of the attached devices read-only");
            Console.WriteLine("For support, visit https://github.com/Stormy102/WindowsRegistryWriteBlocker");
            Console.WriteLine();

            Console.Write("Write blocking is currently ");
            if (IsWriteBlocked)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ENABLED");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("DISABLED/PARTIALLY ENABLED");
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();

            if (args.Length > 0)
            {
                if (args[0].ToLower() == "--enable" || args[0].ToLower() == "-e")
                {
                    SetWriteBlock(true);
                }
                if (args[0].ToLower() == "--disable" || args[0].ToLower() == "-d")
                {
                    SetWriteBlock(false);
                }
                if (args[0].ToLower() == "--help" || args[0].ToLower() == "-h")
                {
                    Console.WriteLine("Command Line Option     | Description");
                    Console.WriteLine("========================|===========================");
                    Console.WriteLine(" -h, --help\t\t| Show the help dialog");
                    Console.WriteLine(" -e, --enable\t\t| Enable the write blocker");
                    Console.WriteLine(" -d, --disable\t\t| Disable the write blocker");
                    return;
                }
            }

            bool quit = false;
            while (quit == false)
            {
                Console.WriteLine("Available options:");
                Console.WriteLine("- Enable the write blocker with [E]");
                Console.WriteLine("- Disable the write blocker with [D]");
                Console.WriteLine("- Press any other key to quit");
                Console.WriteLine();
                Console.Write("> ");

                var input = Console.ReadLine();
                Console.Clear();
                switch (input.ToLower())
                {
                    case "e":
                        SetWriteBlock(true);
                        break;
                    case "d":
                        SetWriteBlock(false);
                        break;
                    default:
                        quit = true;
                        break;
                }
            }
        }

        private static void SetWriteBlock(bool enable)
        {
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue("WriteProtect", enable ? 1 : 0);
            }

            if (enable)
            {
                #region HKLM\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices

                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\'{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}'", RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key.SetValue("Deny_Write", 1);
                }
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\'{53f56307-b6bf-11d0-94f2-00a0c91efb8b}'", RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key.SetValue("Deny_Write", 1);
                }
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\Custom\Deny_Write"))
                {
                    key.SetValue("Deny_Write", 1);
                }
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\Custom\Deny_Write\List"))
                {
                    key.SetValue("{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}");
                    key.SetValue("{53f56307-b6bf-11d0-94f2-00a0c91efb8b}", "{53f56307-b6bf-11d0-94f2-00a0c91efb8b}");
                }

                #endregion

                #region HKLM\SYSTEM\CurrentControlSet\Control\Storage\EnableDenyGP

                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP"))
                {
                    key.SetValue("DenyAllGPState", 1);
                }
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\'{53F56307-B6BF-11D0-94F2-00A0C91EFB8B}'"))
                {
                    key.SetValue("EnumerateDevices", 1);
                    key.SetValue("AccessBitMask", 0);
                    key.SetValue("UserPolicy", 0);
                    key.SetValue("AuditPolicyOnly", 0);
                    key.SetValue("SecurityDescriptor", "D:(D;;DCLCRPCRSD;;;IU)(A;;FA;;;SY)(A;;FA;;;LS)(A;;0x1200a9;;;IU)");
                }
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\'{53F5630D-B6BF-11D0-94F2-00A0C91EFB8B}'"))
                {
                    key.SetValue("EnumerateDevices", 1);
                    key.SetValue("AccessBitMask", 0);
                    key.SetValue("UserPolicy", 0);
                    key.SetValue("AuditPolicyOnly", 0);
                    key.SetValue("SecurityDescriptor", "D:(D;;DCLCRPCRSD;;;IU)(A;;FA;;;SY)(A;;FA;;;LS)(A;;0x1200a9;;;IU)");
                }
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Storage"))
                {
                    key.SetValue("HotplugSecurityDescriptorTest", StringToByteArray("01000480000000000000000000000000140000000200580004000000010014001601010001010000000000050400000000001400ff011f0001010000000000051200000000001400ff011f0001010000000000051300000000001400a9001200010100000000000504000000"), RegistryValueKind.Binary);
                }

                #endregion
            }
            else
            {

                // Delete all of the Group Policy registry keys to revert write-blocking
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", false);
                Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", false);
                Registry.LocalMachine.DeleteSubKey(@"SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\'{53F5630D-B6BF-11D0-94F2-00A0C91EFB8B}'", false);
                Registry.LocalMachine.DeleteSubKey(@"SYSTEM\CurrentControlSet\Control\Storage\EnabledDenyGP\'{53F56307-B6BF-11D0-94F2-00A0C91EFB8B}'", false);
                Registry.LocalMachine.DeleteValue(@"SYSTEM\CurrentControlSet\Control\Storage\HotplugSecurityDescriptor", false);
            }

            // Sleep to allow OS handles to update registry keys 
            Thread.Sleep(3000);

            if (enable)
            {
                Console.WriteLine("Write blocking has been enabled");
                Console.WriteLine("It is highly recommended to test with a non-evidential item first");
            }
            else
            {
                Console.WriteLine("Write blocking has been disabled");
            }
            Console.WriteLine();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private static bool CheckRequirements()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return false;
            }

            if (Environment.OSVersion.Version < new Version(5, 1))
            {
                return false;
            }

            using WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
