using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostController : MonoBehaviour
{
    [SerializeField]
    private float _velocity = 5;
    [SerializeField]
    private float _turnSpeed = 10;
    [SerializeField]
    private GameObject _model;

    //private Vector2 _input;
    private float _angle;
    private Quaternion _targetRotation;
    
    public LayerMask LayerMask;

    //twinstick
    private Vector2 _inputLeftJoystick;
    private Vector2 _inputRightJoystick;

    //ghost inputs
    [SerializeField] private InputController _inputController;
    private ParticleManager _particleManager;
    [SerializeField] private int _ghostId;

    //ghost stats
    [Header("Ghost Stats")]
    [SerializeField] private float _ghostHealth;
    [SerializeField] private float _ghostMaxHealth;
    [SerializeField] private float _ghostSpeed;
    [SerializeField] private float _ghostPower;

    //weapon
    [Header("Ghost Weapon")]
    public Transform WeaponPos;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _firerateTimer;
    private GameObject _firedBullet;
    private float _timer;
    private bool _weaponEnabled = true;
    private float _fuel, _maxfuel = 10, _temptimer;
    private bool _reload = false;
    
    //spells
    private bool _snowstorm = false, _earth = false, _fire = false, _darkorb = true;

    //ghost live status
    private bool _ghostIsAlive = true;
    private Rigidbody _rb;
    //ghost spawn stuff
    private GameObject _ghostSpawnerFinalRoom;

    //ghost ui stuff
    private UiPlayer _playerUi;
    private bool _hasUi = false;

    private SoundManager _soundManager;
    private Animator _animator;

    private float _movementAnimation;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _particleManager = (ParticleManager)FindObjectOfType(typeof(ParticleManager));
        _soundManager = (SoundManager)FindObjectOfType(typeof(SoundManager));
        _timer = _firerateTimer;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));

        if (_ghostIsAlive && _ghostId != 0)
        {
            _timer += Time.deltaTime;
            FuelCheck();

            WeaponCheck();
            GhostCheck();
            
            GetInput();
            CalculateDirection();
            Rotate();
            Move();
            ApplyAnimation();
            UiUpdate();
        }

        if(_ghostSpawnerFinalRoom == null)
        {
            NewStage();
        }

    }

    private void ApplyAnimation()
    {
        _movementAnimation = Mathf.Abs(_inputLeftJoystick.x + _inputLeftJoystick.y);
        _animator.SetFloat("Speed", _movementAnimation);
        _animator.SetFloat("Health", _ghostHealth);
        _animator.SetBool("DamageTaken", false);
    }

    private void Move()
    {
        if (_inputLeftJoystick.magnitude > 0)
            transform.position += new Vector3(_inputLeftJoystick.x, 0, _inputLeftJoystick.y) * _ghostSpeed * Time.deltaTime;
        //transform.position += transform.forward * _velocity * _ghostSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _turnSpeed * Time.deltaTime);
    }

    private void CalculateDirection()
    {
        //_angle = Mathf.Atan2(_input.x, _input.y);
        //_angle = Mathf.Rad2Deg * _angle;
        float _previousRotation;
        if (_inputRightJoystick.magnitude < 0.1f && _inputLeftJoystick.magnitude < 0.1f)
        {
            _previousRotation = _angle;
            _rb.rotation = Quaternion.Euler(0, _previousRotation, 0);
            return;
        }
        if (_inputLeftJoystick.magnitude < _inputRightJoystick.magnitude + 0.3f)//magic number i know
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
        _inputLeftJoystick.x = _inputController.LeftStickHorizontal(_ghostId);
        _inputLeftJoystick.y = _inputController.LeftStickVertical(_ghostId);

        _inputRightJoystick.x = _inputController.RightStickHorizontal(_ghostId);
        _inputRightJoystick.y = _inputController.RightStickVertical(_ghostId);

    }

    //ghost stats
    public void GhostChangeStats(float maxhp, float currenthp, float speed, float power)
    {
        _soundManager.PickupSound();
        _particleManager.StatsParticleEffect(transform.position);
        _ghostMaxHealth += maxhp;
        _ghostSpeed += speed;
        _ghostPower += power;

        GhostMinStatsCheck();

        _ghostHealth += currenthp;

        if (_ghostHealth > _ghostMaxHealth)
        {
            _ghostHealth = _ghostMaxHealth;
        }

        if (_hasUi)
        {
            _playerUi.ChangedStats(_ghostHealth, _ghostSpeed, _ghostPower, _ghostMaxHealth);
        }

    }

    private void GhostMinStatsCheck()
    {
        if (_ghostMaxHealth <= 15)
        {
            _ghostMaxHealth = 15;
        }

        if (_ghostSpeed <= 1.5)
        {
            _ghostSpeed = 1.5f;
        }

        if (_ghostPower <= 0.5)
        {
            _ghostPower = 0.5f;
        }
    }

    private void GhostDies()
    {
        GhostIsDead();
    }

    private void GhostCheck()
    {
        if (_ghostHealth <= 0)
        {
            GhostDies();
        }
    }

    public void GhostIsDead()
    {
        _soundManager.DeathSound();
        _particleManager.DeathParticleEffect(transform.position);
        _ghostIsAlive = false;
        GhostMovesToFinalRoom();
        _ghostSpawnerFinalRoom.GetComponent<GhostSpawner>().GhostNeedsRespawn(_ghostId);
    }

    public void GhostTakesDamage(float damage)
    {
        _animator.SetBool("DamageTaken", true);
        _ghostHealth -= damage;

        if (_hasUi)
        {
            _playerUi.TakesDamage(damage);
        }

    }

    //ghost weapon
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
        if (Input.GetAxis("RightTriggerP" + _ghostId) > 0.1f)
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
        if (_darkorb)
        {
            _firerateTimer = 0.4f;
            CastSpell(1);
        }

        if (_snowstorm)
        {
            _firerateTimer = 0.8f;
            CastSpell(2);
        }

        if (_fire)
        {
            _firerateTimer = 0.08f;
            _maxfuel = 30;
            CastFuelSpell();
        }

        if (_earth)
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
            _firedBullet.GetComponent<BulletStats>().BulletPower(_ghostPower, true);
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
        if (_fuel >= 0)
        {
            if (_timer >= _firerateTimer)
            {
                _animator.SetBool("Shooting", true);
                _soundManager.FlamesSound();
                _firedBullet = Instantiate(_bullet, new Vector3(WeaponPos.position.x, WeaponPos.position.y, WeaponPos.position.z), this.transform.rotation);
                _firedBullet.GetComponent<BulletStats>().BulletPower(_ghostPower, true);
                _timer = 0;
                _fuel -= 0.5f;
            }
        }
    }

    private void FuelCheck()
    {
        if (_fuel >= +_maxfuel)
        {
            _fuel = _maxfuel;
        }

        if (!_reload)
        {
            _fuel += Time.deltaTime * 2;
        }

        if (_temptimer >= 3)
        {
            _fuel = 1;
            _reload = false;
            _temptimer = 0;
        }

        if (_fuel < 0)
        {
            _reload = true;
        }

        if (_reload)
        {
            _temptimer += Time.deltaTime;
            _fuel = -1;
        }

    }
    
    //ghost spawn / next room
    public void GhostTeleport()
    {
        _particleManager.TeleportParticleEffect(this.transform.position);
    }

    private void GhostMovesToFinalRoom()
    {    
        _weaponEnabled = false;
        this.GetComponent<Collider>().enabled = false;
        _model.active = false;
        this.transform.position = _ghostSpawnerFinalRoom.transform.position;
        _particleManager.TeleportParticleEffect(transform.position);
    }

    public void GhostHasBeenMoved()
    {
        this.GetComponent<Collider>().enabled = true;
        _model.active = true;
        _weaponEnabled = true;
        _ghostHealth = _ghostMaxHealth;
        _ghostIsAlive = true;
    }

    public void NewStage()
    {
        _ghostSpawnerFinalRoom = GameObject.Find("GhostSpawnerFinal" + _ghostId);
        _ghostSpawnerFinalRoom.GetComponent<GhostSpawner>().SetGhost(this.GetComponent<ghostController>());
    }

    public void SetGhost(int ghostid)
    {
        _ghostId = ghostid;
    }
    
    public void SetUi(UiPlayer playerui)
    {
        _playerUi = playerui;
    }

    //ui stuff
    private void UiUpdate()
    {
        if (_playerUi != null)
        {
            _playerUi.StartStats(_ghostHealth, _ghostMaxHealth, _timer, _firerateTimer);
            _playerUi.ChangedStats(_ghostHealth, _ghostSpeed, _ghostPower, _ghostMaxHealth);
            _hasUi = true;
        }
        else if (_playerUi == null)
        {
            _hasUi = false;
        }

    }

}
