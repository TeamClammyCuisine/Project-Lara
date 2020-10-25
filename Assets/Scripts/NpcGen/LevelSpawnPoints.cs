using System.Collections.Generic;

namespace NpcGen
{
    public class LevelSpawnPoints 
    {
        public List<Level> Levels { get; set; }

        public LevelSpawnPoints()
        {
            Levels = new List<Level>
            {
                new Level
                {
                    Coordinates = new List<Coordinate>()
                    {
                        new Coordinate {X = 3, Y = -2},
                        new Coordinate {X = -4, Y = 0.5f},
                    }
                },
                new Level
                {
                    Coordinates = new List<Coordinate>()
                    {
                        new Coordinate {X = 6, Y = -10},
                        new Coordinate {X = 21, Y = 12},
                    }
                },
                new Level
                {
                    Coordinates = new List<Coordinate>()
                    {
                        new Coordinate {X = -13, Y = -6},
                        new Coordinate {X = -16, Y = -13},
                    }
                },
                new Level
                {
                    Coordinates = new List<Coordinate>()
                    {
                        new Coordinate {X = 3, Y = -2},
                        new Coordinate {X = -4, Y = 0.5f},
                    }
                }
            };


        }
    }
}
