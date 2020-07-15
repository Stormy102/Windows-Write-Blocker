using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;

namespace WindowsRegistryWriteBlocker
{
    class Program
    {
        private static bool IsWriteBlocked
        {
            get
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    int writeProtect = (int)key.GetValue("WriteProtect");
                    if (writeProtect == 1)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Windows Write Blocker";

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
                Console.WriteLine("DISABLED");
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();

            if (args.Length > 1)
            {
                if (args[1].ToLower() == "--enable")
                {
                    SetWriteBlock(true);
                }
                if (args[1].ToLower() == "--disable")
                {
                    SetWriteBlock(false);
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

            Console.WriteLine();

            // Sleep to allow OS handles to update registry keys 
            Thread.Sleep(3000);

            if (enable)
            {
                Console.WriteLine("Write blocking has been enabled. It is recommended to test with a non-evidential item first");
            }
            else
            {
                Console.WriteLine("Write blocking has been disabled");
            }
            Console.WriteLine();
        }

        private static bool CheckRequirements()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return false;
            }

            using WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
