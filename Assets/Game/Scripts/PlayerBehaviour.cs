﻿using System;
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
    private bool _bButton = false; 

    //public LayerMask LayerMask;
    
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

    public float PlayerPower { get => _playerPower; }

    //weapon
    [Header("Player Weapon")]
    public Transform WeaponPos;
    [SerializeField] private GameObject _bullet;
    private float _firerateTimer;
    private GameObject _firedBullet;
    private BulletStats _bulletStats;
    private float _timer;
    private float _dashTimer;
    private float _fuel, _maxfuel = 10, _temptimer;
    private bool _reload = false;

    //spells
    private bool _snowstorm = false, _earth = false, _fire = false, _darkorb = true;

    private Rigidbody _rb;
    private bool _showGizmo = true;
    
    //ghost 
    [SerializeField] private GameObject _ghost;

    [Header("model testing stuf")]
    public bool HasModel = false;

    private ParticleManager _particleManager;
    private SoundManager _soundManager;
    private Animator _animator;

    private float _animationMovement;

    //ui stuff
    [Header("Player UI")]
    [SerializeField] private UiPlayer _playerUi;
    private bool _hasUi = false;

    //functions
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _dashTimer = _dashReload;
        _timer = _firerateTimer;
        _particleManager = (ParticleManager)FindObjectOfType(typeof(ParticleManager));
        _soundManager = (SoundManager)FindObjectOfType(typeof(SoundManager));
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        if (!this.GetComponent<PlayerControl>()._lockedIn)
            return;
        _playerId = this.GetComponent<PlayerControl>().controllerID;
        _timer += Time.deltaTime;
        FuelCheck();
        _dashTimer += Time.deltaTime;
        WeaponCheck();
        PlayerCheck();
        
        GetInput();
        Dash();    

        CalculateDirection();
        Rotate();
  
        Move();
        ApplyAnimation();
        UiUpdate();
    }

    private void ApplyAnimation()
    {
        _animationMovement = Mathf.Abs(_inputLeftJoystick.x + _inputLeftJoystick.y);
        _animator.SetFloat("Speed", _animationMovement);
        _animator.SetFloat("Health", _playerHealth);
        _animator.SetBool("DamageTaken", false);
    }

    private void Dash()
    {
        if (_aButton && _dashTimer > _dashReload)
        {
            _soundManager.TeleportSound();
            _dashTimer = 0;
            _rb.AddForce(transform.forward * _dashPower, ForceMode.Force);
        }
    }

    private void Move()
    {
        if(_inputLeftJoystick.magnitude > 0)
        transform.position += new Vector3(_inputLeftJoystick.x,0,_inputLeftJoystick.y) * _playerSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _turnSpeed * Time.deltaTime);
    }

    private void CalculateDirection()
    {
        float _previousRotation;
        if (_inputRightJoystick.magnitude < 0.1f && _inputLeftJoystick.magnitude < 0.1f)
        {
            _previousRotation = _angle;
            _rb.rotation = Quaternion.Euler(0, _previousRotation, 0);
            return;
        }
        if (_inputLeftJoystick.magnitude < _inputRightJoystick.magnitude+0.3f)//magic number i know
        {
            _angle = Mathf.Atan2(_inputRightJoystick.x, _inputRightJoystick.y);
            _angle = Mathf.Rad2Deg * _angle;
        }
        else
        {
            _angle = Mathf.Atan2(_inputLeftJoystick.x, _inputLeftJoystick.y);
            _angle = Mathf.Rad2Deg * _angle;
        }
        
    }

    private void GetInput()
    {
        _inputLeftJoystick.x = _inputController.LeftStickHorizontal(_playerId);
        _inputLeftJoystick.y = _inputController.LeftStickVertical(_playerId);

        _inputRightJoystick.x = _inputController.RightStickHorizontal(_playerId);
        _inputRightJoystick.y = _inputController.RightStickVertical(_playerId);

        _aButton = _inputController.AButton(_playerId);       
    }

    public void PlayerTeleport()
    {
        _soundManager.TeleportSound();
        _particleManager.TeleportParticleEffect(this.transform.position);
    }

    // player stats
    public void PlayerChangeStats(float maxhp, float currenthp, float speed, float power)
    {
        _soundManager.PickupSound();
        _particleManager.StatsParticleEffect(this.transform.position);
        _playerMaxHealth += maxhp;
        _playerSpeed += speed;
        _playerPower += power;

        PlayerMinStatsCheck();

        _playerHealth += currenthp;

        if (_playerHealth > _playerMaxHealth)
        {
            _playerHealth = _playerMaxHealth;
        }

        if(_hasUi)
        {
            _playerUi.ChangedStats(_playerHealth, _playerSpeed, _playerPower, _playerMaxHealth);
        }
        
    }
    
    private void PlayerMinStatsCheck()
    {
        if (_playerMaxHealth <= 15)
        {
            _playerMaxHealth = 15;
        }

        if (_playerSpeed <= 1.5f)
        {
            _playerSpeed = 1.5f;
        }

        if (_playerPower <= 0.5)
        {
            _playerPower = 0.5f;
        }
    }

    public void PlayerDies()
    {
        _soundManager.DeathSound();
        _particleManager.DeathParticleEffect(this.transform.position);
        Destroy(this.GetComponent<DontDestroyOnLoad>());
        _ghost = Instantiate(_ghost, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
        
        _ghost.GetComponent<ghostController>().SetGhost(_playerId);
        _ghost.GetComponent<ghostController>().SetUi(_playerUi);

        gameObject.active = false;
    }
    
    private void PlayerCheck()
    {
        if(_fire && _hasUi)
        {
            _playerUi.StaminaMeter(_fuel, _maxfuel);
        }
        else if(!_fire && _hasUi)
        {
            _playerUi.StaminaMeter(_timer, _firerateTimer);
        }

        if(_playerHealth <= 0)
        {
            PlayerDies();          
        }
    }

    public void PlayerTakesDamage(float damage)
    {
        _animator.SetBool("DamageTaken", true);
        _playerHealth -= damage;

        if(_hasUi)
        {
            _playerUi.TakesDamage(damage);
        }
        
    }

    // player weapon
    public void WeaponPickUp(GameObject weapon, bool snowstorm, bool earth, bool fire, bool darkorb)
    {
        _soundManager.PickupSound();
        _particleManager.StatsParticleEffect(this.transform.position);
        _bullet = weapon;
        _snowstorm = snowstorm;
        _earth = earth;
        _fire = fire;
        _darkorb = darkorb;
    }

    private void WeaponCheck()
    {
        if (Input.GetAxis("RightTriggerP" + _playerId) > 0.1f)
        {
            UseWeapon();
        }
        else
        {
            _animator.SetBool("Shooting", false);
        }
        
    }
    
    private void UseWeapon()
    {
        if(_darkorb)
        {
            _firerateTimer = 0.4f;
            CastSpell(1);
        }

        if(_snowstorm)
        {
            _firerateTimer = 0.8f;
            CastSpell(2);
        }
        
        if(_fire)
        {
            _firerateTimer = 0.08f;
            _maxfuel = 10;
            CastFuelSpell();
        }
        
        if(_earth)
        {
            _firerateTimer = 0.8f;
            CastSpell(3);
        }

    }

    //spell that resets timer to be shot again
    private void CastSpell(int soundIndex)
    {
        if (_timer >= _firerateTimer)
        {
            _animator.SetBool("Shooting", true);
            PlaySpellSound(soundIndex);         
            _firedBullet = Instantiate(_bullet, new Vector3(WeaponPos.position.x, WeaponPos.position.y, WeaponPos.position.z), this.transform.rotation);
            _firedBullet.GetComponent<BulletStats>().BulletPower(_playerPower, false);
            _timer = 0;
        }
    }

    private void PlaySpellSound(int soundNumber)
    {
        switch (soundNumber)
        {
            case 1:
                _soundManager.DarkSound();
                break;
            case 2:
                _soundManager.SnowSound();
                break;
            case 3:
                _soundManager.EarthSound();
                break;
            default:
                _soundManager.ShootingSound();
                break;
        }
    }

    //spell you can shoot until fuel is empty, it rechareges while waiting
    private void CastFuelSpell()
    {
        if(_fuel >= 0)
        {
            if(_timer >= _firerateTimer)
            {
                _animator.SetBool("Shooting", true);
                _soundManager.FlamesSound();
                _firedBullet = Instantiate(_bullet, new Vector3(WeaponPos.position.x, WeaponPos.position.y, WeaponPos.position.z), this.transform.rotation);
                _firedBullet.GetComponent<BulletStats>().BulletPower(_playerPower, false);
                _fuel -= 0.6f;
                _timer = 0;
            }
        }
    }

    private void FuelCheck()
    {
        if (_fuel >= _maxfuel)
        {
            _fuel = _maxfuel;
        }

        if (!_reload)
        {
            _fuel += Time.deltaTime * 4;
        }

        if (_temptimer >= 1.5f)
        {
            _fuel = 1;
            _reload = false;
            _temptimer = 0;
            Debug.Log("reloaded");
        }

        if (_fuel < 0)
        {
            _reload = true;
            Debug.Log("reload");
        }
        
        if (_reload)
        {
            _temptimer += Time.deltaTime;
            _fuel = -1;
        }
        
    }
    
    //ui stuff
    private void UiUpdate()
    {
        if(_playerUi != null)
        {
            _playerUi.StartStats(_playerHealth, _playerMaxHealth);
            _playerUi.ChangedStats(_playerHealth, _playerSpeed, _playerPower, _playerMaxHealth);
            _hasUi = true;
        }
        else if(_playerUi == null)
        {
            _hasUi = false;
        }
        
    }

}
