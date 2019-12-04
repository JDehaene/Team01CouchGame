using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private int _spawnerId;
    [SerializeField] private GhostSpawnerControl _spawnerControlScript;

    private ghostController _ghostScript;
    

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
