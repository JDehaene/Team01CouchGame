using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostController : MonoBehaviour
{
    [SerializeField]
    private float _velocity = 5;
    [SerializeField]
    private float _turnSpeed = 10;

    private Vector2 _input;
    private float _angle;
    private Quaternion _targetRotation;
    
    public LayerMask LayerMask;

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

    //ghost live status
    private bool _ghostIsAlive = true;
    private Rigidbody _rb;
    //ghost spawn stuff
    private GameObject _ghostSpawnerFinalRoom;

    private SoundManager _soundManager;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        _particleManager = (ParticleManager)FindObjectOfType(typeof(ParticleManager));
        _soundManager = (SoundManager)FindObjectOfType(typeof(SoundManager));
        _timer = _firerateTimer;
    }

    private void Update()
    {
        if(_ghostIsAlive && _ghostId != 0)
        {
            _timer -= Time.deltaTime;

            WeaponCheck();
            GhostCheck();
            
            GetInput();
            if (Mathf.Abs(_input.x) < 0.2 && Mathf.Abs(_input.y) < 0.2)
            {
                _rb.rotation = Quaternion.Euler(0, _angle, 0);
                return;
            }
            CalculateDirection();
            Rotate();
            Move();
        }

        if(_ghostSpawnerFinalRoom == null)
        {
            NewStage();
        }

    }

    private void Move()
    {
        transform.position += transform.forward * _velocity * _ghostSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _turnSpeed * Time.deltaTime);
    }

    private void CalculateDirection()
    {
        _angle = Mathf.Atan2(_input.x, _input.y);
        _angle = Mathf.Rad2Deg * _angle;
    }

    private void GetInput()
    {
        _input.x = _inputController.LeftStickHorizontal(_ghostId);
        _input.y = _inputController.LeftStickVertical(_ghostId);
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
    }

    private void GhostMinStatsCheck()
    {
        if (_ghostMaxHealth <= 15)
        {
            _ghostMaxHealth = 15;
        }

        if (_ghostSpeed <= 0.5)
        {
            _ghostSpeed = 0.5f;
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
        _ghostHealth -= damage;
    }

    //ghost weapon
    public void WeaponPickUp(GameObject weapon)
    {
        _bullet = weapon;
    }
    
    private void WeaponCheck()
    {
        if (Input.GetAxis("RightTriggerP" + _ghostId) > 0.1f)
        {
            UseWeapon();
        }
    }
    
    private void UseWeapon()
    {
        if (_timer <= 0 && _weaponEnabled)
        {
            _soundManager.ShootingSound();
            _firedBullet = Instantiate(_bullet, new Vector3(WeaponPos.position.x, WeaponPos.position.y, WeaponPos.position.z), this.transform.rotation);
            _firedBullet.GetComponent<BulletStats>().BulletPower(_ghostPower, true);
            _timer = _firerateTimer;
        }
    }

    public void GhostTeleport()
    {
        _particleManager.TeleportParticleEffect(this.transform.position);
    }

    //ghost spawn / next room
    private void GhostMovesToFinalRoom()
    {    
        _weaponEnabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.transform.position = _ghostSpawnerFinalRoom.transform.position;
        _particleManager.TeleportParticleEffect(transform.position);
    }

    public void GhostHasBeenMoved()
    {
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<MeshRenderer>().enabled = true;
        _weaponEnabled = true;
        _ghostHealth = _ghostMaxHealth;
        _ghostIsAlive = true;
    }

    public void NewStage()
    {
        _ghostSpawnerFinalRoom = GameObject.Find("GhostSpawnerFinal" + _ghostId);
        _ghostSpawnerFinalRoom.GetComponent<GhostSpawner>().SetGhost(this.GetComponent<ghostController>());
    }

    public void SetGhostID(int ghostid)
    {
        _ghostId = ghostid;
    }

}
