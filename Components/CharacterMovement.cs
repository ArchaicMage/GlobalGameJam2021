using System;
using Sirenix.OdinInspector;
using Studio.OverOne.Rucksack;
using Studio.OverOne.Rucksack.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.BigSweater.Components
{
    using Com.BigSweater.Components.Abstractions;
    using Com.BigSweater.Data;
    
    [RequireComponent(typeof(Animator), typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
    internal sealed class CharacterMovement : DependantMonoBehaviour, ICharacterMovement
    {
        public static CharacterMovement Instance { get; private set; }

        public PlayerInput PlayerInput => _playerInput;
        
        public CameraRig CameraRig => _cameraRig;
        
        public Vector2 LookInput => _lookInput;

        public Vector2 MoveInput => _moveInput;
        
        public bool IsFalling { get; private set; }
        
        public bool IsGrounded { get; private set; }
        
        public bool IsInteracting { get; private set; }
        
        public float MoveSpeed { get; private set; }

        public Vector3 TargetDirection => _targetDirection;
        
        public bool OverridingMovementType { get; set; }
        
        #region " Inspector Variables "

        [Required]
        [SerializeField] private MovementTypeSO _movementTypeSO;

        [SerializeField] private LayerMask _groundedLayerMask;

        [SerializeField] private bool _assignMainCamera;
            
        [HideIf(nameof(_assignMainCamera))]
        [SerializeField] private CameraRig _cameraRig;
        
        #endregion
        
        #region " Dependency Injection Variables "

        [GetComponent] private readonly Animator _animator;

        [GetComponent] private readonly CapsuleCollider _collider;

        [GetComponent] private readonly PlayerInput _playerInput;
        
        [GetComponent] private readonly Rigidbody _rigidbody;
        
        [GetComponent] private readonly CharacterSoundFx _soundFx;

        #endregion

        #region " Internal Variables "

        private float _fallingTimer;
        
        private Vector2 _lookInput;
        
        private Vector2 _moveInput;
        
        private float _moveVelocity;

        private Vector3 _targetDirection;

        private float _footstepTimer;

        #endregion
        
        #region " Input Methods "

        private void OnInteraction(InputValue value)
        {
            IsInteracting = value.isPressed;
        }
        
        private void OnLook(InputValue value)
        {
            Vector3 lLook = value.Get<Vector2>();
            _lookInput.Set(lLook.x, lLook.y);
        }
        
        private void OnMove(InputValue value)
        {
            Vector2 lMovement = value.Get<Vector2>();
            _moveInput.Set(lMovement.x, lMovement.y);
        }
        
        #endregion

        private void Awake()
        {
            ResolveDependencies();

            if (false == ReferenceEquals(null, Instance))
                return;
                
            Instance = this;
        }
        
        private void Start()
        {
            _cameraRig = _assignMainCamera
                ? Camera.main.GetComponentInParent<CameraRig>()
                : null;

            _playerInput.camera = _cameraRig?.Camera;
            _cameraRig?.SetFollowTarget(transform);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void FixedUpdate()
        {
            if (ReferenceEquals(null, _movementTypeSO))
                return;

            // if (false == (_animator.GetBool(AnimatorHash.IsAiming) || _animator.GetBool(AnimatorHash.IsSprinting)))
            if (false == OverridingMovementType)
            {
                _animator.SetFloat(AnimatorHash.TurnSpeed, _movementTypeSO.TurnSpeed);
                _animator.SetFloat(AnimatorHash.TurnSpeedMultiplier, _movementTypeSO.TurnSpeedMultiplier);
            }

            CalculateMoveSpeed();

            CalculateIsGrounded();
            
            _targetDirection = CalculateTargetDirection();

            UpdatePlayerDirection();
        }

        private void CalculateIsGrounded()
        {
            Vector3 lCharacterBottom = new Vector3(
                transform.position.x,
                transform.position.y + _collider.center.y,
                transform.position.z);

            if (false == Physics.SphereCast(lCharacterBottom, _collider.radius, Vector3.down, out RaycastHit lHit
                , Mathf.Infinity, _groundedLayerMask, QueryTriggerInteraction.Ignore))
            {
                IsGrounded = false;
                _fallingTimer = 0f;
            }
            else
            {
                IsGrounded = lHit.distance <= _collider.height * 0.5f;
                
            }

            bool lIsFalling = false == _animator.GetBool(AnimatorHash.IsJumping) && false == IsGrounded;
            if (lIsFalling)
                _fallingTimer += Time.fixedDeltaTime;
            else
            {
                if (_fallingTimer > 0)
                    _soundFx.PlayLand();
                
                _fallingTimer = 0f;
            }
            
            _animator.SetFloat(AnimatorHash.FallingTime, _fallingTimer);
            _animator.SetBool(AnimatorHash.IsFalling, lIsFalling && _fallingTimer > 1f);
            _animator.SetBool(AnimatorHash.IsGrounded, IsGrounded);
        }
        
        private void CalculateMoveSpeed()
        {
            _animator.SetFloat(AnimatorHash.MoveInputX, MoveInput.x);
            _animator.SetFloat(AnimatorHash.MoveInputY, MoveInput.y);
            
            MoveSpeed = Mathf.Abs(MoveInput.x) + Mathf.Abs(MoveInput.y);
            MoveSpeed = Mathf.SmoothDamp(_animator.GetFloat(AnimatorHash.MoveSpeed), MoveSpeed, ref _moveVelocity, 0.1f);

            if (Mathf.Approximately(MoveSpeed, 0f))
            {
                // Not moving
                MoveSpeed = 0f;
                _footstepTimer = 0f;
            }
            else if (MoveInput.magnitude > 0.1f)
            {
                _footstepTimer = Mathf.Clamp(_footstepTimer - Time.fixedDeltaTime, 0, _movementTypeSO.WalkSpeed);
                // if (IsGrounded && Mathf.Approximately(_footstepTimer, 0f))
                //     _soundFx.PlayFootstep();
            }
            
            if (_footstepTimer == 0f)
            {
                if (MoveSpeed < .5f)
                {
                    // Walking
                    _footstepTimer = _movementTypeSO.WalkSpeed;
                }
                else if (MoveSpeed <= 1)
                    
                {
                    // Jogging
                    _footstepTimer = _movementTypeSO.JogSpeed;
                }
                else if (MoveSpeed < 2f)
                {
                    // Running
                    _footstepTimer = _movementTypeSO.RunSpeed;
                }
            }

            _animator.SetFloat(AnimatorHash.MoveSpeed, MoveSpeed);
        }

        public Vector3 CalculateTargetDirection()
        {
            Transform lReferenceTransform = _cameraRig.Camera.transform;

            Vector3 lForward = lReferenceTransform.TransformDirection(Vector3.forward);
            lForward.y = 0f;

            Vector3 lRight = lReferenceTransform.TransformDirection(Vector3.right);

            Vector3 lDirectionX = MoveInput.x * lRight;
            Vector3 lDirectionY = MoveInput.y * lForward;
            
            if (_animator.GetBool(AnimatorHash.IsAiming))
                return lForward;

            return lDirectionX + lDirectionY;
        }
        
        private void UpdatePlayerDirection()
        {
            if (MoveInput == Vector2.zero && _targetDirection.magnitude <= 0.1f)
                return;
            
            Vector3 lLookDirection = _targetDirection.normalized;
            Quaternion lFreeRotation = Quaternion.LookRotation(lLookDirection, transform.up);
            float lDiffRotation = lFreeRotation.eulerAngles.y - transform.eulerAngles.y;
            float lEulerY = transform.eulerAngles.y;

            if (lDiffRotation < 0f || lDiffRotation > 0)
                lEulerY = lFreeRotation.eulerAngles.y;

            float lTurnSpeed = _animator.GetFloat(AnimatorHash.TurnSpeed);
            float lTurnSpeedMultiplier = _animator.GetFloat(AnimatorHash.TurnSpeedMultiplier);

            if (_animator.GetBool(AnimatorHash.IsAiming))
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, lEulerY, 0f))
                    , lTurnSpeed * lTurnSpeedMultiplier * Time.fixedDeltaTime);
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, lEulerY, 0f))
                    , lTurnSpeed * lTurnSpeedMultiplier * Time.fixedDeltaTime);
        }
    }
}