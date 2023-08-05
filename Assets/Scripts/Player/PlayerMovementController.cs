using UnityEngine;

public class PlayerMovementController
    : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _movement;
    private bool _isGrounded;
    private PlayerSO _config;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _config = PlayerSettings.Instance.Config;
    }

    private void Update()
    {
        UpdateGrounded();
        if (_isGrounded)
        {
            HandleGroundedMovement();
        }
        else
        {
            HandleAirMovement();
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _config.JumpForce, ForceMode.Impulse);
        }

        Debug.Log($"Is grounded: {_isGrounded}");
    }

    private void HandleGroundedMovement()
    {
        _movement = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        _rigidbody.AddForce(_movement * _config.Acceleration * Time.deltaTime, ForceMode.Acceleration);
        if (_rigidbody.velocity.magnitude > _config.GroundedSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _config.GroundedSpeed;
        }
    }

    private void HandleAirMovement()
    {
        _movement = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        _rigidbody.AddForce(_movement * _config.AirSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void UpdateGrounded()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.025f, -Vector3.up * 0.05f, Color.red);
        int groundLayerMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(transform.position + Vector3.up * 0.025f, -Vector3.up, out var hitInfo, 0.05f, groundLayerMask))
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
