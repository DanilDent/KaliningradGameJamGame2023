using UnityEngine;

public class PlayerAnimationsController : MonoSingleton<PlayerAnimationsController>
{
    private int _velocityXHash;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        _velocityXHash = Animator.StringToHash("VelocityX");
    }

    public void UpdateVelocityX(float velocityX)
    {
        _animator.SetFloat(_velocityXHash, velocityX);
    }
}