using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Odogaron
{
    class Program
    {
        private static readonly byte[] UNPATCHED_BYTES = new byte[] { 0x74, 0x69 }; // JE loc
        private static readonly byte[] PATCHED_BYTES = new byte[] { 0x90, 0x90 }; // NOP NOP

        static async Task Main(string[] args)
        {
            bool undo = false;
            Console.WriteLine("Odogaron v" + Assembly.GetExecutingAssembly().GetName().Version);

            if (args.Length > 0)
            {
                string arg = args[0].Trim('-', ' ');
                switch (arg)
                {
                    case "help":
                        Console.WriteLine("\nThis tool is used to patch (disable) the SaveData verification code in Monster Hunter World.");
                        Console.WriteLine("\nUSAGE: odogaron.exe [arg]");
                        Console.WriteLine("ARGS:");
                        Console.WriteLine("  help - show this text");
                        Console.WriteLine("  patch - apply patch [default]");
                        Console.WriteLine("  unpatch - undo patch");
                        return;
                    case "unpatch":
                    case "undo":
                        undo = true;
                        break;
                }
            }

            Process[] procs = Process.GetProcessesByName("MonsterHunterWorld");

            if (undo)
            {
                foreach (var proc in procs)
                    UnpatchSaveDataProtection(proc);
            }
            else
            {
                foreach (var proc in procs)
                    PatchSaveDataProtection(proc);
            }
        }

        static int PatchSaveDataProtection(Process proc)
        {
            IntPtr offset = proc.MainModule.BaseAddress + 0x14AA4902;
            byte[] buff = new byte[2];
            WinApi.ReadProcessMemory(proc.Handle, offset, buff, 2, out var _);

            if (buff.SequenceEqual(UNPATCHED_BYTES))
            {
                WinApi.WriteProcessMemory(proc.Handle, offset, PATCHED_BYTES, 2, out var _);
                Console.WriteLine($"[{proc.Id}] Successfully patched.");
                return 0;
            }
            else if (buff.SequenceEqual(PATCHED_BYTES))
            {
                Console.WriteLine($"[{proc.Id}] Already patched.");
                return 0;
            }
            else
            {
                Console.WriteLine($"[{proc.Id}] Something's wrong, I can feel it.");
                return 1;
            }
        }

        static int UnpatchSaveDataProtection(Process proc)
        {
            IntPtr offset = proc.MainModule.BaseAddress + 0x14AA4902;
            byte[] buff = new byte[2];
            WinApi.ReadProcessMemory(proc.Handle, offset, buff, 2, out var _);

            if (buff.SequenceEqual(PATCHED_BYTES))
            {
                WinApi.WriteProcessMemory(proc.Handle, offset, UNPATCHED_BYTES, 2, out var _);
                Console.WriteLine($"[{proc.Id}] Successfully unpatched.");
                return 0;
            }
            else if (buff.SequenceEqual(UNPATCHED_BYTES))
            {
                Console.WriteLine($"[{proc.Id}] Already unpatched.");
                return 0;
            }
            else
            {
                Console.WriteLine($"[{proc.Id}] Something's wrong, I can feel it.");
                return 1;
            }
        }
    }
}
