using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private float _maxHpStat, _hpStat;
    [SerializeField] private float _speedStat, _powerStat; 
        
    private PlayerBehaviour _playerScript;
    private ghostController _ghostScript;
    private bool _ifGhost = false;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            _playerScript = col.GetComponent<PlayerBehaviour>();
            _ifGhost = false;
            StatsIncreaseDecrease();
        }

        if (col.tag == "Ghost")
        {
            _ghostScript = col.GetComponent<ghostController>();
            _ifGhost = true;
            StatsIncreaseDecrease();
        }

    }

    private void StatsIncreaseDecrease()
    {
        if(!_ifGhost)
        {
            _playerScript.PlayerChangeStats(_maxHpStat, _hpStat, _speedStat, _powerStat);
        }
        else
        {
            _ghostScript.GhostChangeStats(_maxHpStat, _hpStat, _speedStat, _powerStat);
        }

        //Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }

}
