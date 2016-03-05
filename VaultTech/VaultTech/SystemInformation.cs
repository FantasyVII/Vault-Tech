/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 11/August/2014
 * Date Moddified :- 18/January/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace VaultTech
{
    class SystemInformation
    {
        public string GetCPU
        {
            private set { }
            get { return GetSystemInfo("Win32_Processor"); }
        }

        public string GetGPU
        {
            private set { }
            get { return GetSystemInfo("Win32_VideoController"); }
        }

        public string GetRAM
        {
            private set { }
            get { return GetSystemInfo("Win32_PhysicalMemory"); }
        }

        public string GetSoundDevice
        {
            private set { }
            get { return GetSystemInfo("Win32_SoundDevice"); }
        }

        public string GetNetworkAdapter
        {
            private set { }
            get { return GetSystemInfo("Win32_NetworkAdapter"); }
        }

        public string GetHDD
        {
            private set { }
            get { return GetSystemInfo("Win32_LogicalDisk"); }
        }

        string GetSystemInfo(string Key)
        {
            string Info = "";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + Key);

            foreach (ManagementObject share in searcher.Get())
            {
                foreach (PropertyData PC in share.Properties)
                {
                    Info += "\n";
                    Info += PC.Name + "      " + PC.Value;

                }
                Info += "\n";
            }

            return Info;
        }
    }
}