using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _velocity = 5;
    [SerializeField]
    private float _turnSpeed = 10;

    private Vector2 _input;
    private float _angle;
    private Quaternion _targetRotation;

    public GameObject Bullet;
    private float _timer;
    [SerializeField]
    private float _reloadTime = 2;

    [SerializeField]
    private bool _showGizmo = true;

    public LayerMask LayerMask;

    private Transform _holster;

    //player stats
    [SerializeField] private int _playerHealth, _playerMaxHealth;
    [SerializeField] private float _playerSpeed, _playerPower;
    [SerializeField] private GameObject _playerWeapon;
 

    private void Start()
    {
        _holster = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Update()
    {
        GetInput();

        if (Mathf.Abs(_input.x) < 0.2 && Mathf.Abs(_input.y) < 0.2) return;

        CalculateDirection();
        Rotate();
        Move();
    }

    private void Move()
    {
        transform.position += transform.forward * _velocity * _playerSpeed * Time.deltaTime;
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
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("A"))
        {
            if (_timer <= 0)
            {
                Instantiate(Bullet, _holster.position, transform.rotation);
                _timer = _reloadTime;
            }
        }
    }

    private void ApplyCollision()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position + new Vector3(0, 0, 0),
            new Vector3(1, 1, 1), transform.rotation, LayerMask);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Pushable"))
            {
                Debug.Log("FoundPushableOBject");
                Rigidbody rb = collider.GetComponent<Rigidbody>();

                if (Input.GetButtonDown("A"))
                {
                    rb.AddForce(Vector3.forward * 500, ForceMode.Impulse);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_showGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, 1), new Vector3(2, 2, 2));
        }
    }


    // player stats
    public void PlayerChangeStats(int maxhp, int currenthp, float speed, float power)
    {
        _playerMaxHealth += maxhp;
        _playerHealth += currenthp;
        _playerSpeed += speed;
        _playerPower += power;
    }

    public void PlayerGainsHealt(int hp_gain)
    {
        _playerHealth += hp_gain;
    }

    public void PlayerTakesDamage(int damage)
    {
        _playerHealth -= damage;
    }

    public void PlayerGainsMaxHp(int maxhp_gain)
    {
        _playerMaxHealth += maxhp_gain;
    }

    private void PlayerDies()
    {
        //spawn ghost / activate ghost system
        gameObject.active = false;
    }

    public void PlayerGainsSpeed(float speed_gain)
    {
        _playerSpeed += speed_gain;
    }

    public void PlayerLosesSpeed(float speed_loss)
    {
        _playerSpeed -= speed_loss;
    }

    public void PlayerGainsPower(float power_gain)
    {
        _playerPower += power_gain;

    }

    public void PlayerLosesPower(float power_loss)
    {
        _playerPower -= power_loss;
    }


}
