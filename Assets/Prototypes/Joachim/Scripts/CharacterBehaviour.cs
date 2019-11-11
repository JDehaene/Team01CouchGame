using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _dragOnGround;
    [SerializeField] private int _maxRayDist;

    private float _horizontalL;
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _jump;
    private int _jumpAmount = 1;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        HandleInput();
        HandleWallCheck();
        ResetJumps();
    }
    private void FixedUpdate()
    {
        HandleGravity();
        if(_jump)
        {
            ApplyJump();
        }

        MaxRunningSpeed();
        HandleMovement();
        ApplyDrag();
        _controller.Move(_velocity);
    }
    void HandleInput()
    {
        _horizontalL = Input.GetAxis("HorizontalL");
        if (_jumpAmount >= 0 && Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }
    }
    void HandleMovement()
    {
        if(Mathf.Abs(_horizontalL) > 0.1f)
        _velocity.x += _speed * _horizontalL * Time.deltaTime;
    }
    void HandleGravity()
    {
        if(!_controller.isGrounded)
        {
            _velocity += Physics.gravity * _gravityMultiplier * Time.deltaTime;
        }
    }
    void ApplyJump()
    {
        _velocity.y = 0;
        _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
        _jumpAmount--;
        _jump = false;
    }
    void ResetJumps()
    {
        if (_controller.isGrounded)
            _jumpAmount = 1;
    }
    void HandleWallCheck()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,Vector3.right,out hit,_maxRayDist) || Physics.Raycast(transform.position, Vector3.left, out hit, _maxRayDist))
        {
            Debug.Log("Hit!");
            _jumpAmount = 0;
        }
    }
    void MaxRunningSpeed()
    {
        Vector3 xyzVelocity = Vector3.Scale(_velocity, new Vector3(1, 1, 1));


        Vector3 clampedXyzVelocity = Vector3.ClampMagnitude(xyzVelocity, _maxSpeed);

        _velocity = clampedXyzVelocity;
    }
    void ApplyDrag()
    {
            _velocity.x = _velocity.x * (1 - 0.1f * _dragOnGround);
    }
}
