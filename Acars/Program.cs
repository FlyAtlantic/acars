using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acars
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{fed35ab1-b790-4e1f-8b37-ae0ec6919464}");

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new App());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Fly Atlantic ACARS already running.");
            }
        }
    }
}
 