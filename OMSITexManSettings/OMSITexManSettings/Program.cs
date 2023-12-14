using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMSITexManSettings
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\OMSITexManSettings"))
            {
                try
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        MessageBox.Show("Only one instance of OMSITextureManager Settings can be running at one time. " +
                            "Make sure you've closed all running instances (including message boxes) and try again.",
                            "OMSITextureManager Settings - Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (AbandonedMutexException e) // Occurs when the above dialog is already shown in another instance
                {
                    MessageBox.Show("Only one instance of OMSITextureManager Settings can be running at one time. " +
                        "Make sure you've closed all running instances (including message boxes) and try again.",
                        "OMSITextureManager Settings - Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    DialogResult dr = MessageBox.Show("You are launching OMSITextureManager Settings as an administrator. " +
                        "The plugin might not be able to send information to the program in this state. Do you wish to continue?",
                        "OMSITextureManager Settings - Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
