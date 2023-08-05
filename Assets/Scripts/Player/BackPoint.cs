using UnityEngine;

public class BackPoint : MonoBehaviour
{
    [SerializeField] Transform _gfx;

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("PipeTrigger"))
        {
            EventService.Instance.HideInteractButton?.Invoke();
            _gfx.localScale = Vector3.one;
            PlayerController.Instance.CurrentPipeEnter = null;
        }
    }
}