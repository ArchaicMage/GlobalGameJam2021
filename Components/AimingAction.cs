using Studio.OverOne.Rucksack;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.BigSweater.Components
{
    using Com.BigSweater.Components.Abstractions;
    using Com.BigSweater.Data;
    
    [RequireComponent(typeof(Animator), typeof(CharacterMovement))]
    [RequireComponent(typeof(PlayerInput))]
    internal sealed class AimingAction : CharacterAction
    {
        public bool IsAiming { get; private set; }
        
        #region " Inspector Variables "

        [SerializeField] private MovementTypeSO _movementTypeSO;
        
        #endregion
        
        #region " Dependency Injection Variables "

        [GetComponent] private readonly Animator _animator;

        [GetComponent] private readonly CharacterMovement _characterMovement;

        #endregion

        #region " Input Methods "

        private void OnAiming(InputValue value)
        {
            IsAiming = value.isPressed;
            _characterMovement.CameraRig.Aim(IsAiming);
        }
        
        #endregion

        private void FixedUpdate()
        {
            _animator.SetBool(AnimatorHash.IsAiming, IsAiming);
            if (false == IsAiming)
            {
                _characterMovement.OverridingMovementType = false;
                return;
            }
            
            if (ReferenceEquals(null, _movementTypeSO))
                return;

            _characterMovement.OverridingMovementType = true;
            
            _animator.SetFloat(AnimatorHash.TurnSpeed, _movementTypeSO.TurnSpeed);
            _animator.SetFloat(AnimatorHash.TurnSpeedMultiplier, _movementTypeSO.TurnSpeedMultiplier);
        }
    }
}