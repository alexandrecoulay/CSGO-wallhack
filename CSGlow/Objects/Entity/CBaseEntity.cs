﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGlow.MemoryManagers;
using CSGlow.Objects.Structs;
using static CSGlow.Offsets;

namespace CSGlow.Objects
{
    class EntityList
    {
        private CBaseEntity[] entities = new CBaseEntity[4096];

        public EntityList()
        {
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new CBaseEntity(i);
            }
        }
        
        public CBaseEntity this[int index]
        {
            get
            {
                try
                {
                    return entities[index];
                }
                catch 
                {
                    return null;
                }
            }
        }
    }

    class CBaseEntity 
    {
        public int index;

        public CBaseEntity(int index)
        {
            this.index = index;
        }

        public int Base
        {
            get
            {
                return Memory.Read<int>(Memory.clientBase + dwEntityList + index * 0x10);
            }
        }

        public GlowObject GlowObject
        {
            get
            {
                return Memory.Read<GlowObject>(GlowObject.Ptr + GlowIndex * 0x38);
            }
            set
            {
                Memory.Write<GlowObject>(GlowObject.Ptr + GlowIndex * 0x38, value);
            }
        }

        protected int GlowIndex
        {
            get
            {
                return Memory.Read<int>(Base + m_iGlowIndex);
            }
        }
    }
}
