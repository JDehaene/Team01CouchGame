using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private float _firerateTimer;

    public GameObject Bullet;

    private Transform _weaponPos, _player;
    private float _timer, _playerPower;

    private void Start()
    {
        _timer = _firerateTimer;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        _weaponPos = _player.GetComponent<PlayerBehaviour>().WeaponPos;
        Bullet.GetComponent<BulletBehaviour>().BulletPower(_playerPower);
    }

    public void WeaponStats(Transform player, float playerPower)
    {
        _player = player;
        _playerPower = playerPower;
    }
    
    public void UseWeapon()
    {

    }

}
