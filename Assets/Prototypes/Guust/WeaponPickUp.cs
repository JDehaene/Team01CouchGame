using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    //test vars
    private Transform _player;
    public GameObject PickedUpWeapon;
    private TestPlayerController _playerScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _player = other.transform;
            _playerScript = _player.GetComponent<TestPlayerController>();
            PickUp(other);
        }
    }

    public void PickUp(Collider player)
    {
        Debug.Log(player.name + " picked up " + this.name);

        PickedUpWeapon = Instantiate(PickedUpWeapon, _playerScript.WeaponPos.position, _playerScript.WeaponPos.rotation);
        _playerScript.GunPickUp(PickedUpWeapon);

        Destroy(gameObject);

    }

}
