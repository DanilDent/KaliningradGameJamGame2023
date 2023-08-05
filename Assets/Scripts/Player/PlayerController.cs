using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform GfxTransform;
    private EventService _eventService;

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
        GfxTransform.rotation = Quaternion.Euler(90f, 0f, 0f);
        Destroy(gameObject.GetComponent<PlayerMovementController>());
        gameObject.AddComponent<PlayerInPipeController>();
    }

    private void HandleInteractButtonReleased()
    {
        GfxTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("EnterPipeTrigger"))
        {
            EventService.Instance.DisplayInteractButton?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("EnterPipeTrigger"))
        {
            EventService.Instance.HideInteractButton?.Invoke();
        }
    }
}