using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawnerControl : MonoBehaviour
{

    [SerializeField] private GhostSpawner _spawner1, _spawner2, _spawner3, _spawner4;
    [SerializeField] private GameObject _generatorPrefab;
    private Generator _generator;

    private int _finalRoomNumber, _currentRoom;
    private bool _ghostHasSpawned = false;

    private bool[] _spawnerActive = new bool[] { false, false, false, false };

    private void Start()
    {
        _generator = _generatorPrefab.GetComponent<Generator>();
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

    //private void OnTriggerEnter(Collider col)
    //{
    //    if(col.tag == "Player")
    //    {
    //        SpawnGhosts();
    //    }
    //}

}
