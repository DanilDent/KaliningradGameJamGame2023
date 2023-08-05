using UnityEngine;

public class BackPoint : MonoBehaviour
{
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("PipeTrigger"))
        {
            EventService.Instance.HideInteractButton?.Invoke();
            transform.parent.localScale = Vector3.one;
            PlayerController.Instance.CurrentPipeEnter = null;
        }
    }
}