using UnityEngine;

[CreateAssetMenu(fileName = "New Player Config", menuName = "Config/PlayerConfig")]
public class PlayerSO : ScriptableObject
{
    public float GroundedSpeed;
    public float Acceleration;
    public float DecelerationRate;
    public float AirSpeed;
    public float JumpForce;
    public float CrouchInPipeSpeed;
    public float MaxTensionOnHook;
    public float HookBarFillerSpeed;
    public float WaterLevel = -40f;
    public float MaxTensionInPipe;
}