using UnityEngine;

public class PlayerInPipeController : MonoBehaviour
{
    private EventService _eventService;
    private float _forceMultiplier = 0f;
    private Transform _pipe;

    public void Init(Transform pipe)
    {
        _pipe = pipe;
    }

    private void Start()
    {
        _eventService = EventService.Instance;

        _eventService.InteractButtonReleased += HandleInteractButtonReleased;
    }

    private void Update()
    {
        transform.position = _pipe.transform.position;
    }

    private void OnDestroy()
    {
        _eventService.InteractButtonReleased -= HandleInteractButtonReleased;
    }

    private void HandleInteractButtonReleased()
    {

    }
}