using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject Weapon;

    private PlayerBehaviour _playerScript;
    private ghostController _ghostScript;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            _playerScript = col.GetComponent<PlayerBehaviour>();
            PlayerPickUp(col);
        }

        if(col.tag == "Ghost")
        {
            _ghostScript = col.GetComponent<ghostController>();
            GhostPickUp(col);
        }

    }

    public void PlayerPickUp(Collider player)
    {
        _playerScript.WeaponPickUp(Weapon);
        Destroy(gameObject);
    }

    public void GhostPickUp(Collider ghost)
    {
        _ghostScript.WeaponPickUp(Weapon);
        Destroy(gameObject);
    }

}
