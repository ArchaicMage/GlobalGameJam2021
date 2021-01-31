using System;
using System.ComponentModel;
using Studio.OverOne.Rucksack;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.BigSweater.Components
{
    using Com.BigSweater.Components.Abstractions;
    using Com.BigSweater.Data;
    
    [RequireComponent(typeof(Animator), typeof(CharacterMovement))]
    [RequireComponent(typeof(PlayerInput))]
    internal sealed class JumpAction : CharacterAction
    {
        public bool IsJumping { get; private set; }
        
        #region " Inspector Variables "

        [SerializeField] private MovementTypeSO _movementTypeSO;

        [SerializeField] private float _jumpForce = 5f;

        [SerializeField] private float _jumpMoveSpeed = 5f;
        
        #endregion
        
        #region " Dependency Injection Variables "

        [GetComponent] private readonly Animator _animator;

        [GetComponent] private readonly CharacterMovement _characterMovement;

        [GetComponent] private readonly Rigidbody _rigidbody;

        [GetComponent] private readonly CharacterSoundFx _soundFx;

        #endregion

        #region " Internal Variables "

        private Vector3 _jumpVelocity;
        
        #endregion

        #region " Input Methods "

        private void OnJumping(InputValue value)
        {
            if (false == _characterMovement.IsGrounded)
                return;

            _soundFx.PlayJump();
            _jumpVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            
            _rigidbody.velocity = new Vector3(
                _jumpVelocity.x,
                _jumpForce,
                _jumpVelocity.z);
            
            _animator.SetTrigger(AnimatorHash.IsJumping);
            IsJumping = true;
        }
        
        #endregion

        /// <summary>
        /// Called by the Animator when the Jump animation is completed.
        /// </summary>
        private void JumpAnimationCompleted()
        {
            IsJumping = false;
        }

        private void OnAnimatorMove()
        {
            _animator.ApplyBuiltinRootMotion();

            // if (false == _characterMovement.IsGrounded)
            // {
            //     Vector3 lMoveDirection = _characterMovement.CalculateTargetDirection();
            //     Vector3 lMoveVelocity = lMoveDirection * (_jumpMoveSpeed * Time.fixedDeltaTime);
            //
            //     _rigidbody.velocity = new Vector3(
            //         lMoveVelocity.x + _jumpVelocity.x,
            //         _rigidbody.velocity.y,
            //         lMoveVelocity.z + _jumpVelocity.z);
            //
            //     if (_animator.GetFloat(AnimatorHash.FallingTime) > 1f)
            //         _jumpVelocity = Vector3.zero;
            // }
        }

        private void FixedUpdate()
        {
            bool lIsJumping = IsJumping && false == _characterMovement.IsGrounded; 
            
            _animator.SetBool(AnimatorHash.IsSprinting, lIsJumping);
            if (_characterMovement.IsGrounded)
            {
                _characterMovement.OverridingMovementType = false;
                IsJumping = false;
                return;
            }
            
            Vector3 lMoveDirection = _characterMovement.CalculateTargetDirection();
            _rigidbody.MovePosition(_rigidbody.position + lMoveDirection * (_jumpMoveSpeed * Time.fixedDeltaTime));

            if (ReferenceEquals(null, _movementTypeSO))
                return;

            _characterMovement.OverridingMovementType = true;
            
            _animator.SetFloat(AnimatorHash.TurnSpeed, _movementTypeSO.TurnSpeed);
            _animator.SetFloat(AnimatorHash.TurnSpeedMultiplier, _movementTypeSO.TurnSpeedMultiplier);
        }
    }
}