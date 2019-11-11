using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private int Ammo;
    [SerializeField] private float _fireRateTimer;

    public GameObject Bullet;
    private Transform _weaponpos;

    private Transform _player;
    private float _timer;

    private void Start()
    {
        _timer = _fireRateTimer;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        _weaponpos = _player.GetComponent<TestPlayerController>().WeaponPos;

        if (Ammo <= 0)
        {
            WeaponEmpty();
        }
    }

    public void ShootWeapon()
    {
        if (_timer <= 0)
        {
            Instantiate(Bullet, new Vector3(_weaponpos.position.x , _weaponpos.position.y, _weaponpos.position.z), this.transform.rotation);
            Ammo -= 1;
            _timer = _fireRateTimer;
        }
    }

    private void WeaponEmpty()
    {
        _player.GetComponent<TestPlayerController>().WeaponIsEmpty();
        Destroy(gameObject);
    }

    public void WeaponOfPlayer (Transform player)
    {
        _player = player;
    }

}
