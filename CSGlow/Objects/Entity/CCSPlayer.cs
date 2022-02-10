using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGlow.MemoryManagers;
using CSGlow.Objects.Structs;
using static CSGlow.Offsets;

namespace CSGlow.Objects
{
    class CCSPlayer : CBaseEntity
    {
        public CCSPlayer(int index) : base(index)
        {
        }

        public CCSPlayer(CBaseEntity baseEnt) : base(baseEnt.index)
        { 
        }

        public int Team
        {
            get
            {
                return Memory.Read<int>(Base + m_iTeamNum);
            }
        }

        public int Health
        {
            get
            {
                return Memory.Read<int>(Base + m_iHealth);
            }
        }

        public bool Dormant
        {
            get
            {
                return Memory.Read<int>(Base + m_bDormant) == 1;
            }
        }

        public bool Spotted
        {
            get
            {
                return Memory.Read<int>(Base + m_bSpotted) == 1;
            }

            set
            {
                Memory.Write<int>(Base + m_bSpotted, value ? 1 : 0);
            }
        }

        public bool hasHelmet
        {
            get
            {
                return Memory.Read<int>(Base + m_bHasHelmet) == 1;
            }
        }

        public RenderColor RenderColor
        {
            set
            {
                Memory.Write<RenderColor>(Base + m_clrRender, value);
            }
        }
    }
}
