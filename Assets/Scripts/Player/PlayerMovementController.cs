using UnityEngine;

public class PlayerMovementController
    : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _movement;
    private bool _isGrounded;
    private PlayerSO _config;
    private bool _isJump;
    private Transform _lowestPoint;
    private PlayerAnimationsController _animController;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animController = PlayerAnimationsController.Instance;
    }

    private void Start()
    {
        _config = PlayerSettings.Instance.Config;
        _lowestPoint = transform.Find("LowestPoint");
    }

    private void Update()
    {
        HandleInput();
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _isJump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            _isJump = false;
        }
    }

    private void FixedUpdate()
    {
        UpdateGrounded();
        if (_isGrounded)
        {
            HandleGroundedMovement();
            HandleDeceleration();
        }
        else
        {
            HandleAirMovement();
        }

        HandleRotation();

        if (_isJump && _isGrounded)
        {
            _rigidbody.AddForce(transform.up * _config.JumpForce, ForceMode.Impulse);
        }

        Debug.Log($"Is grounded: {_isGrounded}");
    }

    private void HandleGroundedMovement()
    {
        _rigidbody.AddForce(_movement * _config.Acceleration * Time.deltaTime, ForceMode.Acceleration);
        if (_rigidbody.velocity.magnitude > _config.GroundedSpeed)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _rigidbody.velocity.z * (1f / _config.DecelerationRate));
        }
    }

    private void HandleInput()
    {
        _movement = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        _animController.UpdateVelocityX(_movement.magnitude);
    }

    private void HandleDeceleration()
    {
        if (!_isGrounded)
        {
            return;
        }

        float epsilon = 0.01f;

        if (Mathf.Approximately(Vector3.Dot(_movement.normalized, _rigidbody.velocity.normalized), -1f) && _rigidbody.velocity.magnitude > epsilon)
        {
            _rigidbody.velocity = Vector3.zero;
        }

        if (_movement == Vector3.zero && _rigidbody.velocity.magnitude > epsilon)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void HandleRotation()
    {
        if (_movement == Vector3.zero)
        {
            return;
        }

        transform.forward = _movement;
    }

    private void HandleAirMovement()
    {
        _movement = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        _rigidbody.AddForce(_movement * _config.AirSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void UpdateGrounded()
    {
        Debug.DrawRay(_lowestPoint.position + Vector3.up * 0.025f, -Vector3.up * 0.05f, Color.red);
        int groundLayerMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(_lowestPoint.position + Vector3.up * 0.025f, -Vector3.up, out var hitInfo, 0.05f, groundLayerMask))
        {
            _isGrounded = true;
            Debug.Log(hitInfo.collider.gameObject.layer);
        }
        else
        {
            _isGrounded = false;
        }
    }
}
