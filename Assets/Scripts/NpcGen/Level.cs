using System.Collections.Generic;

namespace NpcGen
{
    public class Level
    {
        public List<Coordinate> Coordinates { get; set; }

        public Level()
        {
            Coordinates  = new List<Coordinate>();
        }
    }
} 


