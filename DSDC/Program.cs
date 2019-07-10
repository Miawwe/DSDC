using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using WMPLib;

namespace DSDC
{
    class Program
    {
        static public bool isDead = false;
        static public int numOfDeaths;
        static public string exePath = Path.GetDirectoryName(Path.GetFullPath("DSDC.exe"));
        static void Main(string[] args)
        {
            IntPtr Base = Process.GetProcessesByName("DarkSoulsRemastered").FirstOrDefault().MainModule.BaseAddress + 0x01C04E00; //Get base address and add pointer for the death screen
            VAMemory vam = new VAMemory("DarkSoulsRemastered"); 

            if (File.Exists($"{exePath}\\deaths.txt")) //Check if file with deaths exist, if not, set the numbers of deaths to 0
            {
                numOfDeaths = int.Parse(System.IO.File.ReadAllText($@"{exePath}\\deaths.txt")); //Store all text from the file and convert it to an integer
            } else
            {
                numOfDeaths = 0;
            }

            while (true)
            {
                IntPtr Basefirst = IntPtr.Add((IntPtr)vam.ReadInt32(Base), 0x80); //Add offset to the base address with a value of 0x80 and add all the other offsets
                IntPtr Basesecond = IntPtr.Add((IntPtr)vam.ReadInt32(Basefirst), 0x8);
                IntPtr Basethird = IntPtr.Add((IntPtr)vam.ReadInt32(Basesecond), 0xE8);
                IntPtr Basefourth = IntPtr.Add((IntPtr)vam.ReadInt32(Basethird), 0x48);
                IntPtr Basefifth = IntPtr.Add((IntPtr)vam.ReadInt32(Basefourth), 0xF8);

                if (vam.ReadInt32(Basefifth) == 1 && isDead == false) //Read integer from Dark Souls memory with the pointer and offsets which we just set, also check if the player haven't died before
                {
                    isDead = true;
                    numOfDeaths += 1;
                    
                    System.IO.File.WriteAllText($@"{exePath}\\deaths.txt", numOfDeaths.ToString()); //Write number of deaths to a text file
                }

                if (vam.ReadInt32(Basefifth) == 0 && isDead == true) //Reset the isDead value and wait for the next death
                {
                    isDead = false;
                }
            }
        }
    }
}
