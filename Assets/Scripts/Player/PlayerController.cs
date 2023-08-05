using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform GfxTransform;
    private EventService _eventService;
    private Transform _pipe;
    private Vector3 _prevPosition;

    private void Start()
    {
        gameObject.AddComponent<PlayerMovementController>();
        _eventService = EventService.Instance;

        _eventService.InteractButtonPressed += HandleInteractButtonPressed;
        _eventService.InteractButtonReleased += HandleInteractButtonReleased;
    }

    private void OnDestroy()
    {
        _eventService.InteractButtonPressed -= HandleInteractButtonPressed;
        _eventService.InteractButtonReleased -= HandleInteractButtonReleased;
    }

    private void HandleInteractButtonPressed()
    {
        Destroy(gameObject.GetComponent<PlayerMovementController>());
        var pipeController = gameObject.AddComponent<PlayerInPipeController>();
        GfxTransform.localScale = Vector3.one * 0.8f;
        _prevPosition = transform.position;
        pipeController.Init(_pipe);
    }

    private void HandleInteractButtonReleased()
    {
        GfxTransform.localScale = Vector3.one;
        Destroy(gameObject.GetComponent<PlayerInPipeController>());
        gameObject.AddComponent<PlayerMovementController>();
        transform.position = _prevPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("EnterPipeTrigger"))
        {
            EventService.Instance.DisplayInteractButton?.Invoke();
            _pipe = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("EnterPipeTrigger"))
        {
            EventService.Instance.HideInteractButton?.Invoke();
            _pipe = null;
        }
    }
}