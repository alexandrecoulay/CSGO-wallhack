using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGlow.MemoryManagers;
using static CSGlow.Offsets;

namespace CSGlow.Objects
{
    static class ClientDLL
    {
        public static float[] ViewMatrix
        {
            get
            {
                return Memory.ReadMatrix<float>(Memory.clientBase + dwViewMatrix, 16);
            }
        }

        public static byte ForceUpdateSpectatorGlow 
        {
            get 
            {
                return Memory.Read<byte>(Memory.clientBase + force_update_spectator_glow);
            }
            set 
            {
                Memory.Write<byte>(Memory.clientBase + force_update_spectator_glow, value);
            }
        }
    }
}
