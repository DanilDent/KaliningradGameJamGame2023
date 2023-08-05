using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    public float TensionMultiplier = 0f;
    public Transform CurrentPipeEnter;
    [SerializeField] private Transform _gfx;
    private EventService _eventService;
    private Vector3 _prevPosition;
    private Rigidbody _rigidbody;
    private PlayerSO _config;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _config = PlayerSettings.Instance.Config;
        gameObject.AddComponent<PlayerMovementController>();
        _eventService = EventService.Instance;

        _eventService.InteractButtonPressed += HandleInteractButtonPressed;
        _eventService.InteractButtonReleased += HandleInteractButtonReleased;
        _eventService.BoostButtonPressed += HandleBoostButtonPressed;
    }

    protected override void OnDestroy()
    {
        _eventService.InteractButtonPressed -= HandleInteractButtonPressed;
        _eventService.InteractButtonReleased -= HandleInteractButtonReleased;
        _eventService.BoostButtonPressed -= HandleBoostButtonPressed;
    }

    private void HandleBoostButtonPressed()
    {
        gameObject.GetComponent<PlayerInPipeController>().enabled = false;
        Destroy(gameObject.GetComponent<PlayerInPipeController>());
        gameObject.AddComponent<PlayerMovementController>();
        _rigidbody.AddForce(transform.forward * _config.PipeBoostForce * TensionMultiplier * _config.BoostScaler, ForceMode.Impulse);
        _eventService.HideInteractButton?.Invoke();
        TensionMultiplier = 0f;
    }

    private void HandleInteractButtonPressed()
    {
        Destroy(gameObject.GetComponent<PlayerMovementController>());
        var pipeController = gameObject.AddComponent<PlayerInPipeController>();
        _gfx.localScale = Vector3.one * 0.5f;
        transform.forward = CurrentPipeEnter.forward;
        _prevPosition = transform.position;
        pipeController.Init(CurrentPipeEnter, _gfx);
    }

    private void HandleInteractButtonReleased()
    {
        _gfx.localScale = Vector3.one;
        Destroy(gameObject.GetComponent<PlayerInPipeController>());
        gameObject.AddComponent<PlayerMovementController>();
        transform.position = _prevPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("EnterPipeTrigger") && CurrentPipeEnter == null)
        {
            EventService.Instance.DisplayInteractButton?.Invoke();
            CurrentPipeEnter = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("EnterPipeTrigger") && CurrentPipeEnter != null && other.gameObject == CurrentPipeEnter)
        {
            EventService.Instance.HideInteractButton?.Invoke();
            CurrentPipeEnter = null;
        }
    }

}