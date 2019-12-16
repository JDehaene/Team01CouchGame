using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameBehaviour : MonoBehaviour
{
    [SerializeField]private PlayerSelection _players;
    private int _playersJoined = 0;
    private void Start()
    {
        _players = FindObjectOfType<PlayerSelection>();
    }
    void Update()
    {
        if (_playersJoined > 1)
            _players.CheckCounter();

        Debug.Log(_playersJoined);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBehaviour>() != null)
            _playersJoined++;
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerBehaviour>() != null)
            _playersJoined--;

    }
}
