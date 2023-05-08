using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inteferente_ECO
{
    class Settings
    {
        public static int CellSizeX;
        public static int CellSizeY;
        public static string ResourcesPath = @"C:\\Users\\avaio\\Desktop\\InterferenteECOV2\\Inteferente ECO\\Resources\\";
        public static int GCTick = 0;
        public static int DeflectorIncrement = 0;

        public static bool PlacingDeflector = false;
        public static int PlacementLine = 0;
        public static int PlacementColumn = 0;

        public static string Direction = string.Empty;

        public static int RobotLine;
        public static int RobotColumn;

        public static Dictionary<string, int> Collectibles = new Dictionary<string, int>() {
            {"Sticla" , 0},
            {"Plastic" , 0},
            {"Hartie" , 0}
        };

    }
}
