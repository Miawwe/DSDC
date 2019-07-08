using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WMPLib;

namespace DSDC
{
    class Program
    {
        static public int numOfDeaths = 0;
        static public bool isDead = false;
        static void Main(string[] args)
        {
            IntPtr Base = Process.GetProcessesByName("DarkSoulsRemastered").FirstOrDefault().MainModule.BaseAddress + 0x01C04E00;
            VAMemory vam = new VAMemory("DarkSoulsRemastered");

            WindowsMediaPlayer myplayer = new WindowsMediaPlayer();

            while (true)
            {
                IntPtr Basefirst = IntPtr.Add((IntPtr)vam.ReadInt32(Base), 0x80);
                IntPtr Basesecond = IntPtr.Add((IntPtr)vam.ReadInt32(Basefirst), 0x8);
                IntPtr Basethird = IntPtr.Add((IntPtr)vam.ReadInt32(Basesecond), 0xE8);
                IntPtr Basefourth = IntPtr.Add((IntPtr)vam.ReadInt32(Basethird), 0x48);
                IntPtr Basefifth = IntPtr.Add((IntPtr)vam.ReadInt32(Basefourth), 0xF8);

                if (vam.ReadInt32(Basefifth) == 1 && isDead == false)
                {
                    isDead = true;
                    //Code to run when you die

                    numOfDeaths += 1;

                    myplayer.URL = "bruh.mp3";
                    myplayer.controls.play();

                    //Console.WriteLine("git gud");
                }

                if (vam.ReadInt32(Basefifth) == 0 && isDead == true)
                {
                    isDead = false;
                }
            }
        }
    }
}
