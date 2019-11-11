using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    //basic vars
    [SerializeField] private CharacterController _CharController;
    [SerializeField] private float _speed;
    [SerializeField] private Camera _cam;
    [SerializeField] private int _playerNumber;
    [SerializeField] public Transform WeaponPos;

    public Vector3 _moveInput = Vector3.zero;
    private Vector3 _camInput;
    private Quaternion _rotation;
    private Transform _absoluteTransform;
    private Vector3 _velocity;
    private GameObject _player;
    
    //player health etc
    public int PlayerHealth;
    public GameObject Weapon;
    
    public Quaternion TargetRotation
    {
        get; set;
    }

    private void Start()
    {
        _absoluteTransform = _CharController.transform;
        _player = this.gameObject;
        Weapon = null;
    }

    private void Update()
    {
        //gets the input from the camera forward
        SetInputFromCam();

        //movement
        PlayerInput();
        Rotation();

        //live check
        PlayerHealthCheck();
        GunCHeck();

        //do movement
        DoMovement();
    }
    

    private void SetInputFromCam()
    {
        Vector3 camF = _cam.transform.forward;
        Vector3 camR = _cam.transform.right;
        Vector3 camU = _cam.transform.up;

        _camInput = camF + camR + camU;
    }

    private void PlayerInput()
    {
        if (_CharController.isGrounded)
        {
            _moveInput = new Vector3(Input.GetAxis("Horizontal" + _playerNumber), 0, Input.GetAxis("Vertical" + _playerNumber));
            Quaternion camRotation = Quaternion.LookRotation(_camInput);
            Vector3 relativeMovement = camRotation * _moveInput;
            _velocity += _moveInput;
            _velocity *= _speed * Time.deltaTime;
        }
    }
    
    private void Rotation()
    {

        //Vector3 targetDirection = new Vector3(_moveInput.x, 0, _moveInput.z);
        Vector3 targetDirection = new Vector3(- Input.GetAxis("VerticalLook" + _playerNumber), 0,- Input.GetAxis("HorizontalLook" + _playerNumber));
        if (targetDirection != Vector3.zero)
        {
            TargetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            _CharController.transform.rotation = TargetRotation;
        }

    }

    private void DoMovement()
    {
        _velocity += Physics.gravity * Time.deltaTime;
        _CharController.Move(_velocity);
    }

    private void PlayerHealthCheck()
    {
        if(PlayerHealth <= 0)
        {
            PlayerDead();
        }
    }

    public void PlayerDead()
    {
        Debug.Log("Player" + _playerNumber + " is dead");
        _player.active = false;
    }

    public void GunPickUp(GameObject weapon)
    {
        Weapon = weapon;
        Weapon.GetComponent<WeaponBehaviour>().WeaponOfPlayer(this.transform);
    }

    private void GunCHeck()
    {
        if(Weapon != null)
        {

            Weapon.transform.position = WeaponPos.position;
            Weapon.transform.rotation = WeaponPos.rotation;
            
            if (Input.GetAxis("Shoot" + _playerNumber) > 0.1)
            {
                Weapon.GetComponent<WeaponBehaviour>().ShootWeapon();
                Debug.Log("Player" + _playerNumber + " Shot");
            }
        }
        
        
    }
    
    public void WeaponIsEmpty()
    {
        Weapon = null;
    }

    public void TakeDamage(int damage)
    {
        PlayerHealth -= damage;
    }

}
