/*******************************************************************************************************************************/
// Project: PushPin 
// Filename: Program.cs 
// Description: PushPin is designed to provide a visual interface wrapper to pcileech
// PushPin author: JT, jtestman@gmail.com
// pcileech author: Ulf Frisk, pcileech@frizk.net
// Dependencies: PCILeech v4.6 - https://github.com/ufrisk and it's dependencies 
/*******************************************************************************************************************************/

namespace PushPin
{
    using System;
    using System.Windows.Forms;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
