﻿using System;
using System.Windows.Forms;

namespace DispenserController
{
    static class Program
    {
        public static bool SerialMode { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SerialMode = false;
            foreach (string arg in args)
            {
                if (arg.Length > 6 && arg.Substring(0, 7).ToLower() == "-serial")
                {
                    // Extract serial flag. If present, run in serial mode, else use TCP network
                    SerialMode = arg.Contains("-serial");
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DogDispenserController());
        }
    }
}
