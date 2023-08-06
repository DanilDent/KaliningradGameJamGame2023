using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    public float TensionMultiplier = 0f;
    public float HookTensionPercent = 0f;
    public Transform CurrentPipeEnter;
    public Transform CurrentHook;
    private EventService _eventService;
    private Vector3 _prevPosition;
    private Rigidbody _rigidbody;
    private PlayerSO _config;
    private PlayerAnimationsController _animController;
    private bool _wasBoostedUsed = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _config = PlayerSettings.Instance.Config;
        if (!gameObject.TryGetComponent<PlayerMovementController>(out var temp))
        {
            gameObject.AddComponent<PlayerMovementController>();
        }
        _eventService = EventService.Instance;
        _animController = PlayerAnimationsController.Instance;

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
        _wasBoostedUsed = true;
        if (CurrentPipeEnter != null)
        {
            _animController.UpdateIsFly(true);
            _rigidbody.useGravity = false;
            _rigidbody.AddForce(CurrentPipeEnter.transform.forward * _config.MaxTensionInPipe * TensionMultiplier, ForceMode.Impulse);
            gameObject.GetComponent<PlayerInPipeController>().enabled = false;
            Destroy(gameObject.GetComponent<PlayerInPipeController>());
            if (!gameObject.TryGetComponent<PlayerMovementController>(out var temp))
            {
                gameObject.AddComponent<PlayerMovementController>();
            }
            _eventService.HideInteractButton?.Invoke();
            TensionMultiplier = 0f;
        }
        else if (CurrentHook != null)
        {
            _animController.UpdateIsFly(true);
            gameObject.GetComponent<PlayerOnHookController>().enabled = false;
            Destroy(gameObject.GetComponent<PlayerOnHookController>());
            if (!gameObject.TryGetComponent<PlayerMovementController>(out var temp))
            {
                gameObject.AddComponent<PlayerMovementController>();
            }
            _rigidbody.AddForce(CurrentHook.transform.forward * _config.MaxTensionOnHook * HookTensionPercent, ForceMode.Impulse);
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
            transform.localScale = Vector3.one * 0.25f;
            transform.rotation = CurrentPipeEnter.rotation;
            _prevPosition = transform.position;
            pipeController.Init(CurrentPipeEnter, transform);
            _animController.UpdateIsHooked(true);
        }
        else if (CurrentHook != null)
        {
            Destroy(gameObject.GetComponent<PlayerMovementController>());
            gameObject.AddComponent<PlayerOnHookController>();
            transform.rotation = CurrentHook.rotation;
            _prevPosition = transform.position;
            _animController.UpdateIsHooked(true);
        }
    }

    private void HandleInteractButtonReleased()
    {
        if (CurrentPipeEnter != null)
        {
            Destroy(gameObject.GetComponent<PlayerInPipeController>());
            if (!gameObject.TryGetComponent<PlayerMovementController>(out var temp))
            {
                gameObject.AddComponent<PlayerMovementController>();
            }
            if (!_wasBoostedUsed)
            {
                transform.position = _prevPosition;
            }
            _animController.UpdateIsHooked(false);
        }
        else if (CurrentHook != null)
        {
            Destroy(gameObject.GetComponent<PlayerOnHookController>());
            if (!gameObject.TryGetComponent<PlayerMovementController>(out var temp))
            {
                gameObject.AddComponent<PlayerMovementController>();
            }
            transform.forward = Vector3.forward;
            _animController.UpdateIsHooked(false);
        }

        _wasBoostedUsed = false;
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
            _rigidbody.useGravity = true;
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

        if (other.gameObject.tag.Equals("HookTrigger") && !transform.TryGetComponent<PlayerOnHookController>(out var temp))
        {
            CurrentHook = null;
            EventService.Instance.HideInteractButton?.Invoke();
        }

        if (other.gameObject.tag.Equals("PipeTrigger"))
        {
            EventService.Instance.HideInteractButton?.Invoke();
            transform.localScale = Vector3.one;
            _rigidbody.useGravity = true;
            PlayerController.Instance.CurrentPipeEnter = null;
        }
    }
}