using UnityEngine;

namespace Com.BigSweater.Data
{
    internal struct AnimatorHash
    {
        public static readonly int FallingTime = Animator.StringToHash("FallingTime");
        
        public static readonly int IsAiming = Animator.StringToHash("IsAiming");

        public static readonly int IsFalling = Animator.StringToHash("IsFalling");
        
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        
        public static readonly int IsJumping = Animator.StringToHash("IsJumping");
        
        public static readonly int IsSprinting = Animator.StringToHash("IsSprinting");
        
        public static readonly int MoveInputX = Animator.StringToHash("MoveInputX");
        
        public static readonly int MoveInputY = Animator.StringToHash("MoveInputY");
        
        public static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
        
        public static readonly int TurnSpeed = Animator.StringToHash("TurnSpeed");
        
        public static readonly int TurnSpeedMultiplier = Animator.StringToHash("TurnSpeedMultiplier");
    }
}