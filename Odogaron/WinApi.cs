using System;
using System.Runtime.InteropServices;

namespace Odogaron
{
    public static class WinApi
    {
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);
    }
}
