﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGlow.MemoryManagers;
using static CSGlow.Offsets;

namespace CSGlow.Objects
{
    static class EngineDLL
    {
        public static int ClientStatePtr
        {
            get
            {
                return Memory.Read<int>(Memory.engineBase + dwClientState);
            }
        }

        public static int MaxPlayer
        {
            get
            {
                return Memory.Read<int>(ClientStatePtr + dwClientState_MaxPlayer);
            }
        }

        public static bool InGame
        {
            get
            {
                return Memory.Read<int>(ClientStatePtr + dwClientState_State) == 6;
            }
        }

        public static int ForceReload
        {
            get
            {
                return Memory.Read<int>(ClientStatePtr + clientstate_delta_ticks);
            }
            set
            {
                Memory.Write<int>(ClientStatePtr + clientstate_delta_ticks, value);
            }
        }

        public static bool SendPackets
        {
            get
            {
                return Memory.Read<byte>(Memory.engineBase + dwbSendPackets) == 1;
            }
            set
            {
                Memory.Write<byte>(Memory.engineBase + dwbSendPackets, value ? (byte)0x1 : (byte)0x0);
            }
        }

        public static float ModelAmbientIntensity 
        {
            get 
            {
                return Memory.Read<int>(Memory.engineBase + model_ambient_min);
            }
            set 
            {
                int ptr = Memory.Read<int>(Memory.engineBase + model_ambient_min - 0x2C);
                int convertedBrightness = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
                int xored = convertedBrightness ^ ptr;
                Memory.Write<int>(Memory.engineBase + model_ambient_min, xored);
            }
        }

        public static int GetModelIndexByName(string modelName)
        {
            int modelPrecacheTable = Memory.Read<int>(ClientStatePtr + dwClientState_ModelPrecacheTable);
            int modelPrecacheDict = Memory.Read<int>(modelPrecacheTable + 0x40);
            int modelPrecacheDictItems = Memory.Read<int>(modelPrecacheDict + 0xC);

	        for (int i = 0; i < 1024; i++)
	        {
		        int modelPrecacheDictItem = Memory.Read<int>(modelPrecacheDictItems + 0xC + i * 0x34);
                string modelPrecacheDictItemName = Memory.ReadString(modelPrecacheDictItem, 128, Encoding.ASCII);
		        if (modelPrecacheDictItemName == modelName)
			    {
			        return i;
			    }
	        }

	        return -1;
        }
    }
}
