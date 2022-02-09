using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CSGlow.Hacks;

namespace CSGlow
{
    static class Threads
    {
        static Thread wallThread = new Thread(WallHack.WallHackThread);

        public static void InitAll()
        {
            wallThread.IsBackground = true;

            wallThread.Start();
        }
    }
}
