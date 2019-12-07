﻿using System;
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

    [Header("Shooting variables")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _rateOfFire;
    private float _fireCooldown;
    private GameObject _firedBullet;

    //enemy stats
    [Header("Enemy Stats")]
    [SerializeField] private float _enemyHp;
    [SerializeField] private float _enemyPower;
    
    private void Start()
    {
        //assign random enemy type
    }

    void Update()
    {
        EnemyCheck();
        _colliders = Physics.OverlapSphere(this.transform.position, _radius);      
        EnemyPicker();
    }
    private void Rotation()
    {
        Vector3 lookVector = _player.transform.position - transform.position;
        lookVector.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
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

        }
    }

    private void EnemyBehaviourOne()
    {
        _colliders = Physics.OverlapSphere(this.transform.position, _radius);

        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Rotation();
                transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);
            }
        }
    }

    private void EnemyBehaviourTwo()
    {
        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _player = collider.GetComponent<GameObject>();
                Rotation();

                if(Vector3.Distance(transform.position, collider.transform.position) > _radius/2 + _radius/4)
                {
                    transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);
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
                Rotation();

                if (Vector3.Distance(transform.position, collider.transform.position) > _radius)
                    transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, collider.transform.position) < _radius)
                    Shoot();
            }
        }
    }

    private void Shoot()
    {
        _fireCooldown += Time.deltaTime;
        if(_fireCooldown >= _rateOfFire)
        {
            _firedBullet = Instantiate(_bullet,this.transform.position + transform.forward,this.transform.rotation);
            _firedBullet.GetComponent<BulletStats>().BulletPower(_enemyPower, true);
            _fireCooldown = 0;
        }
    }

    private IEnumerator Charge()
    {
        yield return new WaitForSeconds(_timeUntilCharge);

        if(!_chargePosDetermined)
        {
            _chargePos = _player.transform.position;
            _chargePosDetermined = true;
        }

        if (Vector3.Distance(transform.position, _chargePos) > 0.1f)        
            transform.position = Vector3.MoveTowards(transform.position, _chargePos, _speed * _chargeSpeedModifier * Time.deltaTime);      
        else
        {
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
        Destroy(gameObject);
    }

}
