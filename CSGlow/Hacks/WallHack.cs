using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using CSGlow.Objects;
using CSGlow.Objects.Structs;
using static CSGlow.Objects.GlobalLists;

namespace CSGlow.Hacks
{
    static class WallHack
    {
        public static void WallHackThread()
        {
            while (true)
            {
                if (!Globals.WallHackEnabled)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                if (!EngineDLL.InGame)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                int mp = EngineDLL.MaxPlayer;
                
                for (int i = 0; i < mp; i++)
                {
                    CBaseEntity baseEntity = entityList[i];
                    if (baseEntity == null) continue;
                    CCSPlayer entity = new CCSPlayer(baseEntity);

                    if (entity == null) continue;
                    if (entity.Dormant) continue;
                    if (entity.Health <= 0) continue;


                    if (entity.Team != CBasePlayer.Team)
                    {
                        GlowObject glowObject = entityList[i].GlowObject;

                        glowObject.r = 1;
                        glowObject.g = 0;
                        glowObject.b = 0;

                        glowObject.a = 0.7f;
                        glowObject.m_bFullBloom = Globals.WallHackFullEnabled;
                        glowObject.BloomAmount = Globals.FullBloomAmount;
                        glowObject.m_nGlowStyle = Globals.WallHackGlowOnly ? 1 : 0;
                        glowObject.m_bRenderWhenOccluded = true;
                        glowObject.m_bRenderWhenUnoccluded = false;

                        entityList[i].GlowObject = glowObject;
                    }
                    else
                    {

                        GlowObject glowObject = entityList[i].GlowObject;
                        glowObject.r = 0;
                        glowObject.g = 1;
                        glowObject.b = 0;


                        glowObject.a = 0.7f;
                        glowObject.m_bFullBloom = Globals.WallHackFullEnabled;
                        glowObject.m_nGlowStyle = Globals.WallHackGlowOnly ? 1 : 0;
                        glowObject.m_bRenderWhenOccluded = true;
                        glowObject.m_bRenderWhenUnoccluded = false;

                        entityList[i].GlowObject = glowObject;
                    }
                }

                Thread.Sleep(Globals.UsageDelay);
            }
        }
    }
}
