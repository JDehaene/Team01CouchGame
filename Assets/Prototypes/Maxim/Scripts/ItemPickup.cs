using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private float _maxHpStat, _hpStat;
    [SerializeField] private float _speedStat, _powerStat; 
        
    private PlayerBehaviour _playerScript;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            _playerScript = col.GetComponent<PlayerBehaviour>();
            StatsIncreaseDecrease();
        }
    }

    private void StatsIncreaseDecrease()
    {
        _playerScript.PlayerChangeStats(_maxHpStat, _hpStat, _speedStat, _powerStat);
        Destroy(gameObject);
    }

}
