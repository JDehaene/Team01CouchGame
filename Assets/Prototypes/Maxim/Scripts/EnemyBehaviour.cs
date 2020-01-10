using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy variables")]
    private Collider[] _colliders;
    [SerializeField]
    private float _radius = 5f;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private int _enemyType;
    [Header("Charge variables")]

    [SerializeField]
    private float _timeUntilCharge;
    [SerializeField]
    private float _chargeSpeedModifier;
    [SerializeField]
    private GameObject _player;
    private bool _charging;
    private bool _chargePosDetermined;
    private Vector3 _chargePos;
    [SerializeField] private Vector3 _chargeForce;

    [Header("Shooting variables")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _rateOfFire;
    private float _fireCooldown;
    private GameObject _firedBullet;

    //enemy stats
    [Header("Enemy Stats")]
    [SerializeField] private float _enemyHp;
    [SerializeField] private float _enemyPower;

    private ParticleManager _particleManager;
    private SoundManager _soundManager;
    private Animator _animator;
    private Multiplier _multiplier;

    private void Start()
    {
        //assign random enemy type
        _multiplier = (Multiplier)FindObjectOfType(typeof(Multiplier));
        _particleManager = (ParticleManager)FindObjectOfType(typeof(ParticleManager));
        _soundManager = (SoundManager)FindObjectOfType(typeof(SoundManager));
        _animator = GetComponent<Animator>();

        //scalable stats
        _enemyHp = _enemyHp * _multiplier.StartPlayerCount;
        _enemyPower = _enemyPower * _multiplier.StartPlayerCount;
    }

    void Update()
    {
        EnemyCheck();
        _colliders = Physics.OverlapSphere(this.transform.position, _radius);      
        EnemyPicker();
        ApplyAnimation();

        if (_multiplier.MutliplierChanged)
        {
            ChangeStats(_multiplier.MultiplyNumber);
            _multiplier.MutliplierChanged = false;
        }
    }

    public void ChangeStats(float multiplier)
    {
        _particleManager.StatsParticleEffect(this.transform.position);
        _enemyHp = _enemyHp * multiplier;
        _enemyPower = _enemyPower * multiplier;
    }

    private void ApplyAnimation()
    {
        _animator.SetFloat("Health", _enemyHp);
    }

    private void Rotation(float rotationSpeed)
    {
        Vector3 lookVector = _player.transform.position - transform.position;
        lookVector.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed);
    }

    void EnemyPicker()
    {
        switch(_enemyType)
        {
            case 1:
                EnemyBehaviourOne();
                break;
            case 2:
                EnemyBehaviourTwo();
                break;
            case 3:
                EnemyBehaviourThree();
                break;
            case 4:
                EnemyBehaviourFour();
                break;

        }
    }

    private void EnemyBehaviourOne()
    {
        _colliders = Physics.OverlapSphere(this.transform.position, _radius);

        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _player = collider.GetComponent<Transform>().gameObject;
                Rotation(1);                
                transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);
                _animator.SetFloat("Speed", 1);
            }
            else
            {
                _animator.SetFloat("Speed", 0);
            }
        }
    }

    private void EnemyBehaviourTwo()
    {
        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _player = collider.GetComponent<Transform>().gameObject;
                Rotation(1);

                if(Vector3.Distance(transform.position, collider.transform.position) > _radius/2 + _radius/4)
                {
                    transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);
                    _animator.SetFloat("Speed", 1);
                }
                else
                {
                    _animator.SetFloat("Speed", 0);
                }
                if (!_charging && Vector3.Distance(transform.position, collider.transform.position) < _radius / 2 + _radius/4)
                {
                    
                    _charging = true;
                }

                if(_charging)
                {
                    StartCoroutine("Charge");
                }
            }
        }
    }

    private void EnemyBehaviourThree()
    {
        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _player = collider.GetComponent<Transform>().gameObject;

                //transform.RotateAround(Vector3.up, transform.LookAt(_player.transform));
                Rotation(1);

                if (Vector3.Distance(transform.position, collider.transform.position) > _radius)
                {
                    transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);
                    _animator.SetFloat("Speed", 1);
                }                
                else
                {
                    _animator.SetFloat("Speed", 0);
                }

                if (Vector3.Distance(transform.position, collider.transform.position) < _radius)
                    Shoot();
                else
                {
                    _animator.SetBool("Shooting", false);
                }
            }
        }
    }
    private void EnemyBehaviourFour()
    {
        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _player = collider.GetComponent<Transform>().gameObject;
                Rotation(0.5f);
                if (Vector3.Distance(transform.position, collider.transform.position) < _radius && Vector3.Distance(transform.position, collider.transform.position) > _radius / 8)
                {
                    transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);
                    _animator.SetFloat("Speed", 1);
                }
                else
                {
                    _animator.SetFloat("Speed", 0);
                }                  
            }
        }
    }


    private void Shoot()
    {
        _fireCooldown += Time.deltaTime;
        if(_fireCooldown >= _rateOfFire)
        {
            _animator.SetBool("Shooting", true);
            _firedBullet = Instantiate(_bullet,new Vector3(this.transform.position.x, this.transform.position.y +1, this.transform.position.z) + transform.forward,this.transform.rotation);
            _firedBullet.GetComponent<BulletStats>().BulletPower(_enemyPower, true);
            _fireCooldown = 0;
        }
    }

    private IEnumerator Charge()
    {
        yield return new WaitForSeconds(_timeUntilCharge);

        if(!_chargePosDetermined)
        {
            _chargePos = new Vector3(_player.transform.position.x ,transform.position.y, _player.transform.position.z);
            _chargePosDetermined = true;
        }

        if (Vector3.Distance(transform.position, _chargePos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _chargePos, _speed * _chargeSpeedModifier * Time.deltaTime);
            _animator.SetFloat("Speed", 1);
        }           
        else
        {
            _animator.SetFloat("Speed", 0);
            _chargePosDetermined = false;
            _charging = false;
            _chargePos = Vector3.zero;
            StopCoroutine("Charge");
        }
    }

    //enemy stats
    public void EnemyCheck()
    {
        if(_enemyHp <= 0)
        {
            EnemyDies();
        }
    }

    public void EnemyTakesDamage(float damage)
    {
        _enemyHp -= damage;
    }

    private void EnemyDies()
    {
        _soundManager.DeathSound();
        _particleManager.DeathParticleEffect(transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().PlayerTakesDamage(_enemyPower);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && _charging)
        {
            other.GetComponent<PlayerBehaviour>().PlayerTakesDamage(_enemyPower * 5);
            other.GetComponent<Rigidbody>().AddForce(-other.transform.forward * _chargeForce.z, ForceMode.Impulse);
        }
    }
}
