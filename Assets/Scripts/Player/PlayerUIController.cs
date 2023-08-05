using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoSingleton<PlayerUIController>
{
    [SerializeField] private Transform _interactButtonTransform;
    [SerializeField] private Image _interactButtonImg;
    private EventService _eventService;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Vector3 _offset;
    //
    [SerializeField] private Transform _progressBar;
    [SerializeField] private Image _filler;

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
        if (!transform.gameObject.TryGetComponent<PlayerOnHookController>(out var temp))
        {
            _canvas.transform.position = transform.position + _offset;
        }

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

        UpdateHookBar();
    }

    private void ShowProgressBar()
    {
        _progressBar.gameObject.SetActive(true);
    }

    private void HideProgressBar()
    {
        _progressBar.gameObject.SetActive(false);
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

    private void UpdateHookBar()
    {
        float value = PlayerController.Instance.HookTensionPercent;
        _filler.fillAmount = value;
    }

    private void HandleDisplayInteractButton()
    {
        if (PlayerController.Instance.CurrentHook != null)
        {
            ShowProgressBar();
        }

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
        HideProgressBar();
    }
}