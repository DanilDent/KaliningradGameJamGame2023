using UnityEngine;

public class PlayerOnHookController : MonoBehaviour
{
    private void Start()
    {
        transform.forward = PlayerController.Instance.CurrentHook.transform.forward;
    }

    private void Update()
    {
        transform.position = PlayerController.Instance.CurrentHook.transform.position;
    }
}