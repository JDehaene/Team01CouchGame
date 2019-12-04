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

    //ghost spawn stuff
    private GameObject _ghostSpawnerFinalRoom;

    private void Start()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        //WeaponPickUp(_ghostWeapon);
        _timer = _firerateTimer;
        NewStage();
    }

    private void Update()
    {
        if(_ghostIsAlive && _ghostId != 0)
        {
            _timer -= Time.deltaTime;

            WeaponCheck();
            GhostCheck();
            
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
        _input.x = _inputController.LeftStickHorizontal(_ghostId);
        _input.y = _inputController.LeftStickVertical(_ghostId);
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
        if (_timer <= 0l && _weaponEnabled)
        {
            _firedBullet = Instantiate(_bullet, new Vector3(WeaponPos.position.x, WeaponPos.position.y, WeaponPos.position.z), this.transform.rotation);
            _firedBullet.GetComponent<BulletStats>().BulletPower(_ghostPower, true);
            _timer = _firerateTimer;
        }
    }

    //ghost spawn / next room
    private void GhostMovesToFinalRoom()
    {
        _weaponEnabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.transform.position = _ghostSpawnerFinalRoom.transform.position;
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
