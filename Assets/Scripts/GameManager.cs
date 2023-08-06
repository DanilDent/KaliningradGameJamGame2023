using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private Vector3 _startPosition;
    private PlayerController _playerController;
    private PlayerSO _config;

    private void Start()
    {
        _playerController = PlayerController.Instance;
        _startPosition = _playerController.transform.position;
        _config = PlayerSettings.Instance.Config;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void LateUpdate()
    {
        if (_playerController.transform.position.y < _config.WaterLevel)
        {
            _playerController.transform.position = _startPosition;
            _playerController.transform.forward = Vector3.forward;
        }
    }
}