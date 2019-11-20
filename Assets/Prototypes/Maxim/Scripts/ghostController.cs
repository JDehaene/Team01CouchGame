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

    public GameObject Bullet;
    private float _timer;
    [SerializeField]
    private float _reloadTime = 2;

    [SerializeField]
    private bool _showGizmo = true;

    public LayerMask LayerMask;
    private Transform _holster;
    //Player inputs
    [SerializeField] private InputController _inputController;
    [SerializeField] private int _playerId;

    //player stats
    [SerializeField] private float _ghostHealth, _ghostMaxHealth;
    [SerializeField] private float _ghostSpeed, _ghostPower;

    //player weapon
    [SerializeField] private GameObject _ghostWeapon;
    public Transform WeaponPos;

    private void Start()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        _holster = transform.GetChild(0).GetComponent<Transform>();
        WeaponPickUp(_ghostWeapon);
    }

    private void Update()
    {
        WeaponCheck();

        GetInput();

        if (Mathf.Abs(_input.x) < 0.2 && Mathf.Abs(_input.y) < 0.2) return;

        CalculateDirection();
        Rotate();
        Move();


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
        _input.x = _inputController.LeftStickHorizontal(_playerId);
        _input.y = _inputController.LeftStickVertical(_playerId);


        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
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
    public void PlayerChangeStats(float maxhp, float currenthp, float speed, float power)
    {
        _ghostMaxHealth += maxhp;
        _ghostHealth += currenthp;
        _ghostSpeed += speed;
        _ghostPower += power;
        UpdateWeapon();
    }

    private void PlayerDies()
    {
        //spawn ghost / activate ghost system
        gameObject.active = false;
    }

    private void PlayerCheck()
    {
        if (_ghostHealth > _ghostMaxHealth)
        {
            _ghostHealth = _ghostMaxHealth;
        }

        if (_ghostHealth <= 0)
        {
            PlayerDies();
        }

    }

    // player weapon
    public void WeaponPickUp(GameObject weapon)
    {
        _ghostWeapon = weapon;
        _ghostWeapon.GetComponent<WeaponBehaviour>().WeaponStats(this.transform, _ghostPower);
    }

    private void UpdateWeapon()
    {
        _ghostWeapon.GetComponent<WeaponBehaviour>().WeaponStats(this.transform, _ghostPower);
        _ghostWeapon.GetComponent<WeaponBehaviour>().SetWeapon();
    }

    private void WeaponCheck()
    {
        if (_ghostWeapon != null)
        {
            _ghostWeapon.transform.position = WeaponPos.position;
            _ghostWeapon.transform.rotation = WeaponPos.rotation;

            if (Input.GetAxis("RightTriggerP" + _playerId) > 0.1f)
            {
                _ghostWeapon.GetComponent<WeaponBehaviour>().UseWeapon();
                Debug.Log("pew");
            }

        }
    }

    public void ReplaceWeapon()
    {
        Destroy(_ghostWeapon);
        _ghostWeapon = null;
    }
}
