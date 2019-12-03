using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public PlayerControl[] _players;
    private List<int> activePlayers = new List<int>();

    private void Update()
    {
        CheckInput();
    }
    void CheckInput()
    {
        //to join
        for (int i = 1; i < _players.Length + 1; ++i)
        {
            if (!activePlayers.Contains(i) && Input.GetButtonDown("AButtonP" + i))
            {
                activePlayers.Add(i);
                _players[GetDevil()].Active(i);
            }
        }

        //to back out
        for (int i = 1; i < 5; ++i)
        {
            if (activePlayers.Contains(i) && Input.GetButtonDown("BButtonP" + i))
            {
                activePlayers.Remove(i);
                DeactiveDevil(i);
            }
        }
    }
    int GetDevil()
    {
        for (int i = 0; i < _players.Length; ++i)
        {
            if (!_players[i].enabled)
            {
                return i;
            }
        }
        return 0;
    }

    void DeactiveDevil(int ctrl)
    {
        for (int i = 0; i < _players.Length; ++i)
        {
            if (_players[i].enabled && _players[i].controllerID.Equals(ctrl))
            {
                _players[i].DeActive();
                return;
            }
        }
    }
}
