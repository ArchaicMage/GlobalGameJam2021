using System;
using Cinemachine;
using Studio.OverOne.Rucksack;
using Studio.OverOne.Rucksack.Components;
using UnityEngine;

namespace Com.BigSweater.Components
{
    internal sealed class CameraRig : MonoBehaviour
    {
        public Camera Camera => _camera;

        public CameraReticule CameraReticule => _cameraReticule;

        #region " Inspector Variables "

        [SerializeField] private CameraReticule _cameraReticule;
        
        [SerializeField] private int _priorityBoostAmount = 10;
        
        [SerializeField] private Camera _camera;
        
        [SerializeField] private CinemachineVirtualCamera _3rdPersonAimCamera;
        
        [SerializeField] private CinemachineVirtualCamera _3rdPersonNormalCamera;

        #endregion

        #region " Internal Variables "

        private bool _boosted;
        
        #endregion

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Aim(bool isAiming)
        {
            _cameraReticule.IsAiming = isAiming;
            if (isAiming && false == _boosted)
            {
                _3rdPersonAimCamera.Priority += _priorityBoostAmount;
                _boosted = true;
            }
            else
            {
                _3rdPersonAimCamera.Priority -= _priorityBoostAmount;
                _boosted = false;
            }
        }
        
        public void SetFollowTarget(Transform targetTransform)
        {
            _3rdPersonAimCamera.Follow = _3rdPersonNormalCamera.Follow = targetTransform;
        }
    }
}