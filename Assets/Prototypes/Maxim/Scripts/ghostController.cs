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
    private bool _showGizmo = true;

    private Vector2 _input;
    private float _angle;
    private Quaternion _targetRotation;
    
    public LayerMask LayerMask;

    //ghost inputs
    [SerializeField] private InputController _inputController;
    [SerializeField] private int _playerId;

    //ghost stats
    [SerializeField] private float _ghostHealth, _ghostMaxHealth;
    [SerializeField] private float _ghostSpeed, _ghostPower;

    //ghost weapon
    [SerializeField] private GameObject _ghostWeapon;
    public Transform WeaponPos;
    private bool _weaponEnabled = true;

    //ghost live status
    private bool _ghostIsAlive = true;

    private void Start()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        WeaponPickUp(_ghostWeapon);
    }

    private void Update()
    {
        if(_ghostIsAlive)
        {
            WeaponCheck();
            GetInput();
            if (Mathf.Abs(_input.x) < 0.2 && Mathf.Abs(_input.y) < 0.2) return;
            CalculateDirection();
            Rotate();
            Move();
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
        _input.x = _inputController.LeftStickHorizontal(_playerId);
        _input.y = _inputController.LeftStickVertical(_playerId);
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


    //ghost stats
    public void GhostChangeStats(float maxhp, float currenthp, float speed, float power)
    {
        _ghostMaxHealth += maxhp;
        _ghostHealth += currenthp;
        _ghostSpeed += speed;
        _ghostPower += power;
        UpdateWeapon();
    }

    private void GhostDies()
    {
        GhostIsDead();
    }

    private void GhostCheck()
    {
        if (_ghostHealth > _ghostMaxHealth)
        {
            _ghostHealth = _ghostMaxHealth;
        }

        if (_ghostHealth <= 0)
        {
            GhostDies();
        }

    }

    public void GhostIsDead()
    {
        _ghostIsAlive = false;
        _weaponEnabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    //ghost weapon
    public void WeaponPickUp(GameObject weapon)
    {
        _ghostWeapon = weapon;
        _ghostWeapon.GetComponent<WeaponBehaviour>().WeaponStats(this.transform, _ghostPower, true);
    }

    private void UpdateWeapon()
    {
        _ghostWeapon.GetComponent<WeaponBehaviour>().WeaponStats(this.transform, _ghostPower, true);
        _ghostWeapon.GetComponent<WeaponBehaviour>().SetWeapon();
    }

    private void WeaponCheck()
    {
        if (_ghostWeapon != null)
        {
            _ghostWeapon.transform.position = WeaponPos.position;
            _ghostWeapon.transform.rotation = WeaponPos.rotation;

            if (Input.GetAxis("RightTriggerP" + _playerId) > 0.1f && _weaponEnabled)
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

    //ghost spawn / next room
    public void GhostMovesToNextRoom(Transform location)
    {
        _weaponEnabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.transform.position = location.position;
    }

    private void GhostHasBeenMoved()
    {
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<MeshRenderer>().enabled = true;
        _weaponEnabled = true;
    }

}
