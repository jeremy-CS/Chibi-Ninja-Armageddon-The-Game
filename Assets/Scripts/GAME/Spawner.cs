using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawns;
    [SerializeField] private GameObject[] _agents;
    private List<int> _lastPositions = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnAgents();
    }

    private void SpawnAgents()
    {
        foreach (GameObject agent in _agents)
        {
            agent.transform.position = _spawns[GetRandomSpawnPoint()].position;
        }
    }

    private int GetRandomSpawnPoint()
    {
        int rand = Random.Range(0, _spawns.Length);
        bool spawnFound = false;

        while (!spawnFound)
        {
            if (_lastPositions.Contains(rand))
            {
                rand = Random.Range(0, _spawns.Length);
            }
            else
            {
                spawnFound = true;
                _lastPositions.Add(rand);
            }
        }

        return rand;
    }
}
