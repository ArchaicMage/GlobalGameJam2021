using Studio.OverOne.Rucksack;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.BigSweater.Components
{
    using Com.BigSweater.Components.Abstractions;
    using Com.BigSweater.Data;
    
    [RequireComponent(typeof(Animator), typeof(CharacterMovement))]
    [RequireComponent(typeof(PlayerInput))]
    internal sealed class SprintAction : CharacterAction
    {
        public bool IsSprinting { get; private set; }
        
        #region " Inspector Variables "

        [SerializeField] private MovementTypeSO _movementTypeSO;
        
        [SerializeField] private float _sprintMultiplier = 2f;
        
        #endregion
        
        #region " Dependency Injection Variables "

        [GetComponent] private readonly Animator _animator;

        [GetComponent] private readonly CharacterMovement _characterMovement;

        #endregion

        #region " Input Methods "

        private void OnSprint(InputValue value)
        {
            IsSprinting = value.isPressed;
        }
        
        #endregion

        private void FixedUpdate()
        {
            bool lIsSprinting = IsSprinting 
                && _characterMovement.MoveInput != Vector2.zero
                && false == _animator.GetBool(AnimatorHash.IsAiming);
            
            _animator.SetBool(AnimatorHash.IsSprinting, lIsSprinting);
            if (false == lIsSprinting)
            {
                _characterMovement.OverridingMovementType = false;
                return;
            }
            
            float lCurrentSpeed = _animator.GetFloat(AnimatorHash.MoveSpeed);
            float lSprintSpeed = Mathf.Clamp(lCurrentSpeed * _sprintMultiplier, 0f, _sprintMultiplier);
            _animator.SetFloat(AnimatorHash.MoveSpeed,  lSprintSpeed);

            if (ReferenceEquals(null, _movementTypeSO))
                return;
            
            _characterMovement.OverridingMovementType = true;
            
            _animator.SetFloat(AnimatorHash.TurnSpeed, _movementTypeSO.TurnSpeed);
            _animator.SetFloat(AnimatorHash.TurnSpeedMultiplier, _movementTypeSO.TurnSpeedMultiplier);
        }
    }
}