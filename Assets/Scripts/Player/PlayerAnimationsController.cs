﻿using UnityEngine;

public class PlayerAnimationsController : MonoSingleton<PlayerAnimationsController>
{
    private int _velocityXHash;
    private int _isJumpHash;
    private int _isHookedHash;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        _velocityXHash = Animator.StringToHash("VelocityX");
        _isJumpHash = Animator.StringToHash("IsJump");
        _isHookedHash = Animator.StringToHash("IsHooked");
    }

    public void UpdateVelocityX(float velocityX)
    {
        _animator.SetFloat(_velocityXHash, velocityX);
    }

    public void UpdateIsJump(bool isJump)
    {
        _animator.SetBool(_isJumpHash, isJump);
    }

    public void UpdateIsHooked(bool isHooked)
    {
        _animator.SetBool(_isHookedHash, isHooked);
    }
}