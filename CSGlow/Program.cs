using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSGlow
{
    static class Program
    {
        public static EntryForm entryForm;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            entryForm = new EntryForm();
            Application.Run(entryForm);
        }
    }
}
