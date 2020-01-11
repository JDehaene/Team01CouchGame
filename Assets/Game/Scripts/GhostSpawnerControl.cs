using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawnerControl : MonoBehaviour
{
    private GameObject _spawner1Prefab, _spawner2Prefab, _spawner3Prefab, _spawner4Prefab;
    private GhostSpawner _spawner1, _spawner2, _spawner3, _spawner4;
    private GameObject _generatorPrefab;
    private Generator _generator;

    private int _finalRoomNumber, _currentRoom;
    private bool _ghostHasSpawned = false;

    private bool[] _spawnerActive = new bool[] { false, false, false, false };

    private void Start()
    {
        _generatorPrefab = GameObject.Find("Generator");
        _generator = _generatorPrefab.GetComponent<Generator>();
        //SetSpawners();
    }

    private void Update()
    {
        if(_generatorPrefab != null)
        {
            if (_generator.CameraIndex == _generator.NumberOfRooms && !_ghostHasSpawned)
            {
                SpawnGhosts();
                _ghostHasSpawned = true;
            }

        }

    }

    private void SetSpawners()
    {
        _spawner1Prefab = GameObject.Find("GhostSpawnerFinal1");
        _spawner2Prefab = GameObject.Find("GhostSpawnerFinal2");
        _spawner3Prefab = GameObject.Find("GhostSpawnerFinal3");
        _spawner4Prefab = GameObject.Find("GhostSpawnerFinal4");

        _spawner1 = _spawner1Prefab.GetComponent<GhostSpawner>();
        _spawner2 = _spawner2Prefab.GetComponent<GhostSpawner>();
        _spawner3 = _spawner3Prefab.GetComponent<GhostSpawner>();
        _spawner4 = _spawner4Prefab.GetComponent<GhostSpawner>();
        
    }

    private void SpawnGhosts()
    {
        if(_spawnerActive[0])
        {
            _spawner1.SpawnGhost();
            Debug.Log("Ghost 1 spawnded");
        }
        if (_spawnerActive[1])
        {
            _spawner2.SpawnGhost();
            Debug.Log("Ghost 2 spawnded");
        }
        if (_spawnerActive[2])
        {
            _spawner3.SpawnGhost();
            Debug.Log("Ghost 3 spawnded");
        }
        if (_spawnerActive[3])
        {
            _spawner4.SpawnGhost();
        }
    }

    public void EnableRespawn(int spawnerid)
    {
        _spawnerActive[spawnerid] = true;
        Debug.Log("spawner " + spawnerid + " is active");
        SetSpawners();
    }
    
}
