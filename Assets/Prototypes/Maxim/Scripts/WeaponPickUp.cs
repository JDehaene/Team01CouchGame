using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject Weapon;

    private PlayerBehaviour _playerScript;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            _playerScript = col.GetComponent<PlayerBehaviour>();
            PickUp(col);
        }
    }

    public void PickUp(Collider player)
    {
        Weapon = Instantiate(Weapon, _playerScript.WeaponPos.position, _playerScript.WeaponPos.rotation);
        _playerScript.ReplaceWeapon();
        _playerScript.WeaponPickUp(Weapon);

        Destroy(gameObject);
    }

}
