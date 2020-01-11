using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private int _spawnerId;
    [SerializeField] private GhostSpawnerControl _spawnerControlScript;

    private GameObject _spawnControll;
    private ghostController _ghostScript;

    private void Start()
    {
        _spawnControll = GameObject.Find("Generator");
        _spawnerControlScript = _spawnControll.GetComponent<GhostSpawnerControl>();
    }

    //spawner to door information
    public void SpawnGhost()
    {
        _ghostScript.GhostHasBeenMoved();
    }
    
    //ghost to spawner information
    public void SetGhost(ghostController ghostscript)
    {
        _ghostScript = ghostscript;
    }

    public void GhostNeedsRespawn(int ghostid)
    {
        _spawnerId = ghostid;
        _spawnerControlScript.EnableRespawn(_spawnerId -1);
    }

}
