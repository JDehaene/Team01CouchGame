using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{
    [Header("Pit")]
    [SerializeField] private bool _pit = false;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if(_pit)
            {
                col.GetComponent<PlayerBehaviour>().PlayerDies();
            }
        }

        if(col.tag == "Enemy")
        {
            col.GetComponent<EnemyBehaviour>().EnemyTakesDamage(500);
        }

    }



}
