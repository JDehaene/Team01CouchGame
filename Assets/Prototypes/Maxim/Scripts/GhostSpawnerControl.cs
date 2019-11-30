using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawnerControl : MonoBehaviour
{

    [SerializeField] private GhostSpawner _spawner1, _spawner2, _spawner3, _spawner4;

    private bool[] _spawnerActive = new bool[] { false, false, false, false };
    
    private void SpawnGhosts()
    {
        if(_spawnerActive[0])
        {
            _spawner1.SpawnGhost();
        }
        if (_spawnerActive[1])
        {
            _spawner2.SpawnGhost();
        }
        if (_spawnerActive[2])
        {
            _spawner3.SpawnGhost();
        }
        if (_spawnerActive[3])
        {
            _spawner4.SpawnGhost();
        }
    }

    public void EnableRespawn(int spawnerid)
    {
        _spawnerActive[spawnerid] = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            SpawnGhosts();
        }
    }

}
