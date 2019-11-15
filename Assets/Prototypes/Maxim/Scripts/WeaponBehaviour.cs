using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private float _firerateTimer;

    public GameObject Bullet;

    private Transform _weaponPos, _player;
    private float _timer, _playerPower;
    private int _counter;

    private void Start()
    {
        _timer = _firerateTimer;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        _weaponPos = _player.GetComponent<PlayerBehaviour>().WeaponPos;
        if(_counter < 1)
        {
            SetWeapon();
            _counter++;
        }
    }

    public void SetWeapon()
    {
        Bullet.GetComponent<BulletStats>().BulletPower(_playerPower);
    }

    public void WeaponStats(Transform player, float playerPower)
    {
        _player = player;
        _playerPower = playerPower;
    }
    
    public void UseWeapon()
    {
        if (_timer <= 0)
        {
            Instantiate(Bullet, new Vector3(_weaponPos.position.x, _weaponPos.position.y, _weaponPos.position.z), this.transform.rotation);
            _timer = _firerateTimer;
        }
    }

}
