using System.Drawing;

namespace CSGlow.Objects
{

    static class Globals
    {
        public static bool WallHackEnabled = false;
        public static bool WallHackFullEnabled = false;
        public static bool WallHackGlowOnly = false;
        public static Color WallHackEnemy = Color.Red;
        public static Color WallHackTeammate = Color.Green;
        public static float FullBloomAmount = 1.0f;

        public static bool RenderEnabled = false;
        public static bool RenderEnemyOnly = false;
        public static Color RenderColor = Color.Red;
        public static int RenderBrightness = 1;


        private static int _IdleWait = 10;
        public static int IdleWait
        {
            get
            {
                return _IdleWait;
            }
            set
            {
                _IdleWait = 50 - (value * 10);
            }
        }

        private static int _UsageDelay = 1;
        public static int UsageDelay
        {
            get
            {
                return _UsageDelay;
            }
            set
            {
                _UsageDelay = 5 - value;
            }
        }

    }

    static class GlobalLists
    {
        public static EntityList entityList = new EntityList();
    }
}
