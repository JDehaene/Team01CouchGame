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

    

    //player weapon
    [SerializeField] private GameObject _playerWeapon;
    public Transform WeaponPos;

    //ghost stuff
    [SerializeField] private GameObject _ghost;
    
    private void Start()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        WeaponPickUp(_playerWeapon);
    }

    private void Update()
    {
        WeaponCheck();

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
        UpdateWeapon();
    }
    
    private void PlayerDies()
    {
        //spawn ghost / activate ghost system
        _ghost = Instantiate(_ghost, transform.position, transform.rotation);

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

    // player weapon
    public void WeaponPickUp(GameObject weapon)
    {
        _playerWeapon = weapon;
        _playerWeapon.GetComponent<WeaponBehaviour>().WeaponStats(this.transform, _playerPower, false);
    }

    private void UpdateWeapon()
    {
        _playerWeapon.GetComponent<WeaponBehaviour>().WeaponStats(this.transform, _playerPower, false);
        _playerWeapon.GetComponent<WeaponBehaviour>().SetWeapon();
    }

    private void WeaponCheck()
    {
        if (_playerWeapon != null)
        {
            _playerWeapon.transform.position = WeaponPos.position;
            _playerWeapon.transform.rotation = WeaponPos.rotation;

            if (Input.GetAxis("RightTriggerP"+_playerId) > 0.1f)
            {
                _playerWeapon.GetComponent<WeaponBehaviour>().UseWeapon();
            }

        }
    }
    
    public void ReplaceWeapon()
    {
        Destroy(_playerWeapon);
        _playerWeapon = null;
    }
    
    //ghost stuff

}
