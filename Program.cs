using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace vlcPlayer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] arges)
        {
            PlayerWindow pw = null;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (arges.Length>0)
            {
                pw = new PlayerWindow(arges[0]);
            }
            else
            {
                pw = new PlayerWindow(null);
            }
           
            Application.Run(pw);
            
        }
    }
}
