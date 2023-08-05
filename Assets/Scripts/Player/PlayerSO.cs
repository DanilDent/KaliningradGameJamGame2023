using UnityEngine;

[CreateAssetMenu(fileName = "New Player Config", menuName = "Config/PlayerConfig")]
public class PlayerSO : ScriptableObject
{
    public float GroundedSpeed;
    public float Acceleration;
    public float DecelerationRate;
    public float AirSpeed;
    public float JumpForce;
}