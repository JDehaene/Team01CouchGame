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
    [SerializeField]
    private bool _showGizmo = true;

    private Vector2 _inputLeftJoystick;
    private Vector2 _inputRightJoystick;
    private float _angle;
    private Quaternion _targetRotation;

    public LayerMask LayerMask;
    
    //player inputs
    [SerializeField] private InputController _inputController;
    [SerializeField] private int _playerId;

    //player stats
    [SerializeField] private float _playerHealth, _playerMaxHealth;
    [SerializeField] private float _playerSpeed, _playerPower;
    
    //weapon
    public Transform WeaponPos;
    [SerializeField] private GameObject _bullet;
    private GameObject _firedBullet;
    [SerializeField] private float _firerateTimer;
    private BulletStats _bulletStats;
    private float _timer;
    
    //ghost 
    [SerializeField] private GameObject _ghost;
    
    private void Start()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        //WeaponPickUp(_playerWeapon);
        _timer = _firerateTimer;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        WeaponCheck();
        PlayerCheck();

        GetInput();

        if (Mathf.Abs(_inputLeftJoystick.x) < 0.2 && Mathf.Abs(_inputLeftJoystick.y) < 0.2) return;

        CalculateDirection();
        Rotate();
        Move();
    }

    private void Move()
    {
        transform.position += transform.forward * _velocity * _playerSpeed * Time.deltaTime;

        //transform.position += new Vector3(_inputLeftJoystick.x, 0, _inputLeftJoystick.y) * _velocity * _playerSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _turnSpeed * Time.deltaTime);

        //transform.Rotate(0, _inputRightJoystick.x * Time.deltaTime * _turnSpeed * 50, 0);
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

        //_inputRightJoystick.x = _inputController.RightStickHorizontal2(_playerId);
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
    public void PlayerChangeStats(float maxhp, float currenthp, float speed, float power)
    {
        _playerMaxHealth += maxhp;
        _playerHealth += currenthp;
        _playerSpeed += speed;
        _playerPower += power;
    }
    
    private void PlayerDies()
    {
        //spawn ghost / activate ghost system
        Debug.Log("player " + _playerId + " died");
        _ghost = Instantiate(_ghost, transform.position, transform.rotation);
        _ghost.GetComponent<ghostController>().SetGhostID(_playerId);

        gameObject.active = false;
    }
    
    private void PlayerCheck()
    {
        if(_playerHealth > _playerMaxHealth)
        {
            _playerHealth = _playerMaxHealth;
        }

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
