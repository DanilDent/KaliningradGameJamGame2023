using UnityEngine;

public class PlayerInPipeController : MonoBehaviour
{
    private EventService _eventService;
    private float _forceMultiplier = 0f;

    private void Start()
    {
        _eventService = EventService.Instance;

        _eventService.InteractButtonReleased += HandleInteractButtonReleased;
    }

    private void OnDestroy()
    {
        _eventService.InteractButtonReleased -= HandleInteractButtonReleased;
    }

    private void HandleInteractButtonReleased()
    {

    }
}