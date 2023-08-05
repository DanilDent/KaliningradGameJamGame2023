using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    public float TensionMultiplier = 0f;
    public float HookTensionPercent = 0f;
    public Transform CurrentPipeEnter;
    public Transform CurrentHook;
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
        if (CurrentPipeEnter != null)
        {
            gameObject.GetComponent<PlayerInPipeController>().enabled = false;
            Destroy(gameObject.GetComponent<PlayerInPipeController>());
            gameObject.AddComponent<PlayerMovementController>();
            _rigidbody.AddForce(transform.forward * _config.PipeBoostForce * TensionMultiplier * _config.BoostScaler, ForceMode.Impulse);
            _eventService.HideInteractButton?.Invoke();
            TensionMultiplier = 0f;
        }
        else if (CurrentHook != null)
        {
            gameObject.GetComponent<PlayerOnHookController>().enabled = false;
            Destroy(gameObject.GetComponent<PlayerOnHookController>());
            gameObject.AddComponent<PlayerMovementController>();
            _rigidbody.AddForce(transform.forward * _config.MaxTension * HookTensionPercent, ForceMode.Impulse);
            _eventService.HideInteractButton?.Invoke();
            HookTensionPercent = 0f;
        }
    }

    private void HandleInteractButtonPressed()
    {
        if (CurrentPipeEnter != null)
        {
            Destroy(gameObject.GetComponent<PlayerMovementController>());
            var pipeController = gameObject.AddComponent<PlayerInPipeController>();
            _gfx.localScale = Vector3.one * 0.5f;
            transform.forward = CurrentPipeEnter.forward;
            _prevPosition = transform.position;
            pipeController.Init(CurrentPipeEnter, _gfx);
        }
        else if (CurrentHook != null)
        {
            Destroy(gameObject.GetComponent<PlayerMovementController>());
            gameObject.AddComponent<PlayerOnHookController>();
            transform.forward = CurrentHook.forward;
            _prevPosition = transform.position;
        }
    }

    private void HandleInteractButtonReleased()
    {
        if (CurrentPipeEnter != null)
        {
            _gfx.localScale = Vector3.one;
            Destroy(gameObject.GetComponent<PlayerInPipeController>());
            gameObject.AddComponent<PlayerMovementController>();
            transform.position = _prevPosition;
        }
        else if (CurrentHook != null)
        {
            Destroy(gameObject.GetComponent<PlayerOnHookController>());
            gameObject.AddComponent<PlayerMovementController>();
            transform.forward = Vector3.forward;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("EnterPipeTrigger") && CurrentPipeEnter == null)
        {
            EventService.Instance.DisplayInteractButton?.Invoke();
            CurrentPipeEnter = other.transform;
        }

        if (other.gameObject.tag.Equals("HookTrigger") && CurrentHook == null)
        {
            CurrentHook = other.transform;
            EventService.Instance.DisplayInteractButton?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("EnterPipeTrigger") && CurrentPipeEnter != null && other.gameObject == CurrentPipeEnter)
        {
            EventService.Instance.HideInteractButton?.Invoke();
            CurrentPipeEnter = null;
        }

        if (other.gameObject.tag.Equals("HookTrigger"))
        {
            CurrentHook = null;
            EventService.Instance.HideInteractButton?.Invoke();
        }
    }

}