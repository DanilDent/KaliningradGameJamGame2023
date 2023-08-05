using UnityEngine;

public class PlayerInPipeController : MonoBehaviour
{
    private EventService _eventService;
    private Transform _pipe;
    private Vector3 _input;
    private Transform _gfx;
    private PlayerSO _config;
    private Transform _head;
    private PlayerController _playerController;

    public void Init(Transform pipe, Transform gfx)
    {
        _pipe = pipe;
        _gfx = gfx;
        _config = PlayerSettings.Instance.Config;
        _playerController = PlayerController.Instance;
    }

    private void Start()
    {
        _eventService = EventService.Instance;
        _eventService.InteractButtonReleased += HandleInteractButtonReleased;

        _head = transform.Find("Gfx/Head");
        _playerController.TensionMultiplier = 1.0f;
    }

    private void Update()
    {
        transform.position = _pipe.transform.position;
        HandleInput();
        HandleDeformation();
    }

    private void HandleInput()
    {
        _input = new Vector3(0f, 0f, Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Jump"))
        {
            _eventService.BoostButtonPressed?.Invoke();
        }
    }

    private void HandleDeformation()
    {
        _gfx.localScale = new Vector3(_gfx.localScale.x, _gfx.localScale.y, _gfx.localScale.z + _config.CrouchInPipeSpeed * _input.z * Time.deltaTime);
        _playerController.TensionMultiplier = Vector3.Distance(_head.transform.position, _playerController.CurrentPipeEnter.position);
        if (_playerController.TensionMultiplier < 1.0f)
        {
            _playerController.TensionMultiplier = 1.0f;
        }
        Debug.Log($"Tension: {_playerController.TensionMultiplier}");
    }

    private void OnDestroy()
    {
        _eventService.InteractButtonReleased -= HandleInteractButtonReleased;
    }

    private void HandleInteractButtonReleased()
    {

    }
}