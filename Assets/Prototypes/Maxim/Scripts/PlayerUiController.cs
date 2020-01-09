using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUiController : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerUi;

    private GameObject _test;

    public void Start()
    {

        foreach (var player in _playerUi)
        {
            player.active = false;
        }
    }

    public void SetPlayerActive(int player)
    {
        _playerUi[player -1].active = true;
    }

    public void SetPlayerInactive(int player)
    {
        _playerUi[player -1].active = false;
    }

}
