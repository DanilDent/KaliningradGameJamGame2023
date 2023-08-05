using UnityEngine;

public class PlayerSettings : MonoSingleton<PlayerSettings>
{
    public PlayerSO Config => _config;
    [SerializeField] private PlayerSO _config;
}