using System;
using System.Collections.Generic;
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
        private static int PATCH_OFFSET = 0x1D6FBF92;

        static async Task Main(string[] args)
        {
            bool showHelp = false;
            bool undo = false;
            bool watch = false;

            Console.WriteLine("Odogaron v" + Assembly.GetExecutingAssembly().GetName().Version);

            foreach (string arg in args.Select(x => x.Trim('-', ' ')))
            {
                switch (arg)
                {
                    case "help":
                        showHelp = true;
                        break;
                    case "unpatch":
                    case "undo":
                        undo = true;
                        break;
                    case "watch":
                        watch = true;
                        break;
                }
            }

            if (showHelp)
            {
                Console.WriteLine("\nThis tool is used to patch (disable) the SaveData verification code in Monster Hunter World.");
                Console.WriteLine("\nUSAGE: odogaron.exe [arg]");
                Console.WriteLine("ARGS:");
                Console.WriteLine("  help - show this text");
                Console.WriteLine("  patch - apply patch [default]");
                Console.WriteLine("  unpatch - undo patch");
                return;
            }

            if (watch)
            {
                List<int> alreadyProcessed = new List<int>();
                while (true)
                {
                    Run(undo, alreadyProcessed);
                    await Task.Delay(2500);
                }
            }
            else
            {
                Run(undo); // Run once
            }
        }

        static void Run(bool undo, List<int>? alreadyProcessed = null)
        {
            Process[] procs = Process.GetProcessesByName("MonsterHunterWorld");
            if (procs.Length == 0)
            {
                Console.WriteLine("No MonsterHunterWorld process found.");
            }
            else
            {
                foreach (var proc in procs)
                {
                    if (alreadyProcessed != null)
                    {
                        // Only do the patching/unpatching one (only needed when running in 'watch'mode).
                        if (alreadyProcessed.Contains(proc.Id)) continue;
                        else alreadyProcessed.Add(proc.Id);
                    }

                    try
                    {
                        if (undo) UnpatchSaveDataProtection(proc);
                        else PatchSaveDataProtection(proc);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        static int PatchSaveDataProtection(Process proc)
        {
            IntPtr offset = proc.MainModule.BaseAddress + PATCH_OFFSET;
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
            IntPtr offset = proc.MainModule.BaseAddress + PATCH_OFFSET;
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
