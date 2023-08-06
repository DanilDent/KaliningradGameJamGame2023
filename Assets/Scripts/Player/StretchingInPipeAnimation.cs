using UnityEngine;

public class StretchingInPipeAnimation : MonoSingleton<StretchingInPipeAnimation>
{
    [SerializeField] private Transform _firstBone;
    private PlayerSO _config;

    private void Start()
    {
        _config = PlayerSettings.Instance.Config;
    }

    public void Stretch()
    {
        _firstBone.transform.position += _firstBone.transform.forward * _config.CrouchInPipeSpeed * Time.deltaTime;
    }
}