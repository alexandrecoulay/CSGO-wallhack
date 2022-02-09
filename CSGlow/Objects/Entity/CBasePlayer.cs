using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGlow.MemoryManagers;
using static CSGlow.Offsets;

namespace CSGlow.Objects
{
    static class CBasePlayer
    {
        public static int LocalPlayerPtr
        {
            get
            {
                return Memory.Read<int>(Memory.clientBase + dwLocalPlayer);
            }
        }

        public static int GetViewModelIndex(int index)
        {
            return Memory.Read<int>(LocalPlayerPtr + m_hViewModel + index * 0x4) & 0xFFF;
        }

        public static int Team
        {
            get
            {
                return Memory.Read<int>(LocalPlayerPtr + m_iTeamNum);
            }
        }

        public static int Flags
        {
            get
            {
                return Memory.Read<int>(LocalPlayerPtr + m_fFlags);
            }
        }
    }
}
