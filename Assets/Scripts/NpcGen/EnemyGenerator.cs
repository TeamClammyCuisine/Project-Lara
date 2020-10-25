using System.Collections;
using System.Collections.Generic;
using NpcGen;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public int NumberOfEnemies;
    private List<Coordinate> _coordinates;
    
    public List<GameObject> EnemiesPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        _coordinates = new List<Coordinate>
        {
            new Coordinate{X= 3, Y=-2},
            new Coordinate{X= -4, Y=0.5f},
            
        };
        for (var i = 0; i < NumberOfEnemies; i++)
        {
            var rndPrefab = Random.Range(0,EnemiesPrefabs.Count);
            var rndPos = Random.Range(0,_coordinates.Count);
            var pos = _coordinates[rndPos];
            Instantiate(EnemiesPrefabs[rndPrefab], new Vector3(pos.X, pos.Y,0),Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
