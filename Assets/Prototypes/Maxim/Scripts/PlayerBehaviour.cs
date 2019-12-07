using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //[SerializeField]
    //private float _velocity = 5;
    [SerializeField]
    private float _turnSpeed = 10;

    private Vector2 _inputLeftJoystick;
    private Vector2 _inputRightJoystick;
    private float _angle;
    private Quaternion _targetRotation;

    private bool _aButton = false;

    public LayerMask LayerMask;
    
    //player inputs
    [SerializeField] private InputController _inputController;
    [SerializeField] private int _playerId;

    //player stats
    [Header("Player Stats")]
    [SerializeField] private float _playerHealth;
    [SerializeField] private float _playerMaxHealth;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _playerPower;
    [SerializeField] private float _dashPower = 500;
    [SerializeField] private float _dashReload = 3;

    //weapon
    [Header("Player Weapon")]
    public Transform WeaponPos;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _firerateTimer;
    private GameObject _firedBullet;
    private BulletStats _bulletStats;
    private float _timer;
    private float _dashTimer;

    private Rigidbody _rb;
    
    //ghost 
    [SerializeField] private GameObject _ghost;

    [Header("model testing stuf")]
    public bool HasModel = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _dashTimer = _dashReload;
        //WeaponPickUp(_playerWeapon);
        _timer = _firerateTimer;
    }

    private void Update()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        if (!this.GetComponent<PlayerControl>()._lockedIn)
            return;
        _playerId = this.GetComponent<PlayerControl>().controllerID;
        _timer -= Time.deltaTime;
        _dashTimer += Time.deltaTime;

        WeaponCheck();
        PlayerCheck();

        GetInput();
        Dash();

        if (Mathf.Abs(_inputLeftJoystick.x) < 0.2 && Mathf.Abs(_inputLeftJoystick.y) < 0.2)
        {
            _rb.rotation = Quaternion.Euler(0, _angle, 0);
            return;
        } 

        CalculateDirection();
        Rotate();
        Move();
    }

    private void Dash()
    {
        if (_aButton && _dashTimer > _dashReload)
        {
            _dashTimer = 0;
            _rb.AddForce(transform.forward * _dashPower, ForceMode.Force);
        }
    }

    private void Move()
    {
        transform.position += transform.forward * _playerSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _turnSpeed * Time.deltaTime);
    }

    private void CalculateDirection()
    {
        _angle = Mathf.Atan2(_inputLeftJoystick.x, _inputLeftJoystick.y);
        _angle = Mathf.Rad2Deg * _angle;
    }

    private void GetInput()
    {
        _inputLeftJoystick.x = _inputController.LeftStickHorizontal(_playerId);
        _inputLeftJoystick.y = _inputController.LeftStickVertical(_playerId);

        _aButton = _inputController.AButton(_playerId);
    }

    // player stats
    public void PlayerChangeStats(float maxhp, float currenthp, float speed, float power)
    {
        _playerMaxHealth += maxhp;
        _playerSpeed += speed;
        _playerPower += power;

        PlayerMinStatsCheck();

        _playerHealth += currenthp;

        if (_playerHealth > _playerMaxHealth)
        {
            _playerHealth = _playerMaxHealth;
        }
    }
    
    private void PlayerMinStatsCheck()
    {
        if (_playerMaxHealth <= 15)
        {
            _playerMaxHealth = 15;
        }

        if (_playerSpeed <= 0.5)
        {
            _playerSpeed = 0.5f;
        }

        if (_playerPower <= 0.5)
        {
            _playerPower = 0.5f;
        }
    }

    public void PlayerDies()
    {
        //spawn ghost / activate ghost system
        Debug.Log("player " + _playerId + " died");
        if(HasModel)
        {
            _ghost = Instantiate(_ghost, new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
        }
        else
        {
            _ghost = Instantiate(_ghost, transform.position, transform.rotation);
        }

        
        _ghost.GetComponent<ghostController>().SetGhostID(_playerId);

        gameObject.active = false;
    }
    
    private void PlayerCheck()
    {
        if(_playerHealth <= 0)
        {
            PlayerDies();
        }
    }

    public void PlayerTakesDamage(float damage)
    {
        _playerHealth -= damage;
    }

    // player weapon
    public void WeaponPickUp(GameObject weapon)
    {
        _bullet = weapon;
    }
    
    private void WeaponCheck()
    {
        if (Input.GetAxis("RightTriggerP" + _playerId) > 0.1f)
        {
            UseWeapon();
        }
        
    }
    
    private void UseWeapon()
    {
        if (_timer <= 0)
        {
            _firedBullet = Instantiate(_bullet, new Vector3(WeaponPos.position.x, WeaponPos.position.y, WeaponPos.position.z), this.transform.rotation);
            _firedBullet.GetComponent<BulletStats>().BulletPower(_playerPower, false);
            _timer = _firerateTimer;
        }
    }

}
