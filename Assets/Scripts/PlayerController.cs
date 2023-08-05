using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _movement;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _jumpForce;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateGrounded();
        HandleMovement();


        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        Debug.Log($"Is grounded: {_isGrounded}");
    }

    private void HandleMovement()
    {
        if (!_isGrounded)
        {
            return;
        }

        _movement = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));
        _rigidbody.AddForce(_movement * _acceleration * Time.deltaTime, ForceMode.Acceleration);
        if (_rigidbody.velocity.magnitude > _playerSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _playerSpeed;
        }
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
