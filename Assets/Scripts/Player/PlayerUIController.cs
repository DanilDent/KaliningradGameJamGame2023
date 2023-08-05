using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoSingleton<PlayerUIController>
{
    [SerializeField] private Transform _interactButtonTransform;
    [SerializeField] private Image _interactButtonImg;
    private EventService _eventService;

    protected override void Awake()
    {
        base.Awake();
        _eventService = EventService.Instance;
    }

    private void Start()
    {
        _eventService.DisplayInteractButton += HandleDisplayInteractButton;
        _eventService.HideInteractButton += HandleHideInteractButton;
        _eventService.InteractButtonPressed += HandleInteractButtonPressed;
        _eventService.InteractButtonReleased += HandleInteractButtonReleased;
    }

    private void Update()
    {
        if (_interactButtonTransform.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _eventService.InteractButtonPressed?.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                _eventService.InteractButtonReleased?.Invoke();
            }
        }
    }

    private void HandleInteractButtonPressed()
    {
        _interactButtonImg.color = Color.green;
    }

    private void HandleInteractButtonReleased()
    {
        _interactButtonImg.color = Color.white;
        _eventService.HideInteractButton?.Invoke();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _eventService.DisplayInteractButton -= HandleDisplayInteractButton;
        _eventService.HideInteractButton -= HandleHideInteractButton;
        _eventService.InteractButtonPressed -= HandleInteractButtonPressed;
        _eventService.InteractButtonReleased -= HandleInteractButtonReleased;
    }

    private void HandleDisplayInteractButton()
    {
        if (transform.gameObject.TryGetComponent<PlayerInPipeController>(out var temp))
        {
            return;
        }
        _interactButtonImg.color = Color.white;
        _interactButtonTransform.gameObject.SetActive(true);
    }

    private void HandleHideInteractButton()
    {
        _interactButtonTransform.gameObject.SetActive(false);
    }
}