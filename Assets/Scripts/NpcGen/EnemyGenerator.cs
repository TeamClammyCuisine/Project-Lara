using System.Collections.Generic;
using UnityEngine;

namespace NpcGen
{
    public class EnemyGenerator : MonoBehaviour
    {
        private LevelSpawnPoints _levelsSpawnPoints;
    
        public List<GameObject> EnemiesPrefabs;
        // Start is called before the first frame update

        public EnemyGenerator()
        {
            _levelsSpawnPoints = new LevelSpawnPoints();
        }

        public void Generate(int level, int numberOfEnemies)
        {
            level--;
            var coordinates = _levelsSpawnPoints.Levels[level].Coordinates;
            for (var i = 0; i < numberOfEnemies; i++)
            {
                var rndPrefab = Random.Range(0,EnemiesPrefabs.Count);
                var rndPos = Random.Range(0,coordinates.Count);
                var pos = coordinates[rndPos];
                Instantiate(EnemiesPrefabs[rndPrefab], new Vector3(pos.X, pos.Y,0),Quaternion.identity);
            }
        }
    }
}
