using System.Collections;
using UnityEngine;

public class PlayerOnHookController : MonoBehaviour
{
    private float _step = 0.2f;
    private PlayerController _playerController;
    private void Start()
    {
        _playerController = PlayerController.Instance;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.forward = PlayerController.Instance.CurrentHook.transform.forward;
        _step = PlayerSettings.Instance.Config.HookBarFillerSpeed;
        StartCoroutine(HookTensionPercentCoroutine());
    }

    private void Update()
    {
        if (PlayerController.Instance.CurrentHook != null)
        {
            transform.position = PlayerController.Instance.CurrentHook.transform.position;
        }

        PlayerUIController.Instance.UpdateBtnColor(Color.green);

        if (Input.GetButtonDown("Jump"))
        {
            EventService.Instance.BoostButtonPressed?.Invoke();
        }
    }

    private void OnDestroy()
    {
        _playerController.HookTensionPercent = 0f;
    }

    private IEnumerator HookTensionPercentCoroutine()
    {
        while (true)
        {
            while (_playerController.HookTensionPercent < 1f)
            {
                _playerController.HookTensionPercent += _step * Time.deltaTime;
                yield return null;
            }
            _playerController.HookTensionPercent = 1f;
            while (_playerController.HookTensionPercent > 0f)
            {
                _playerController.HookTensionPercent -= _step * Time.deltaTime;
                yield return null;
            }
            _playerController.HookTensionPercent = 0f;
        }
    }
}