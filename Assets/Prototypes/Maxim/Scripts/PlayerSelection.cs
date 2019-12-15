using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSelection : MonoBehaviour
{
    public PlayerControl[] _players;
    private List<int> _activePlayers = new List<int>();

    public GameObject bannerTimer;
    private bool isCounting = false;
    public Text txtCounter;
    public float maxTime = 30;
    private float counter = 0;
    private int _playersNeededToStart;

    private void Update()
    {
        CheckInput();
        _playersNeededToStart = _activePlayers.Count;
        //CheckCounter();
    }
    public void CheckCounter()
    {
        if (isCounting)
        {
            if (_activePlayers.Count < 2)
            {
                counter = 0;
                isCounting = false;
                bannerTimer.SetActive(false);
                return;
            }

            counter += Time.deltaTime;
            txtCounter.text = "Time Left: " + (int)(maxTime - counter);

            if (counter >= maxTime)
            {
                this.enabled = false;
                txtCounter.text = "Start";
                for (int i = 0; i < _players.Length; ++i)
                {
                    RemoveNonPlayedChars();
                    _players[i].StartGame();
                }
            }
        }
        else if (_activePlayers.Count > 1)
        {
            counter = 0;
            isCounting = true;
            bannerTimer.SetActive(true);
        }
    }

    void CheckInput()
    {
        //to join
        for (int i = 1; i < _players.Length + 1; ++i)
        {
            if (!_activePlayers.Contains(i) && Input.GetButtonDown("AButtonP" + i))
            {
                Debug.Log("Player" + i + "joined");
                _activePlayers.Add(i);
                _players[GetDevil()].Active(i);
            }
        }

        //to back out
        for (int i = 1; i < 5; ++i)
        {
            if (_activePlayers.Contains(i) && Input.GetButtonDown("BButtonP" + i))
            {
                _activePlayers.Remove(i);
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
    void RemoveNonPlayedChars()
    {
        foreach(PlayerControl _player in _players)
        {
            if (!_player._lockedIn)
                Destroy(_player.gameObject);
        }
    }
    
}
