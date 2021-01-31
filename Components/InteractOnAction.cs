using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Com.BigSweater.Components
{
    public class InteractOnAction : InteractOnTrigger
    {
        #region " Inspector Variables "

        [SerializeField] private UnityEvent _onActionTriggered;

        [SerializeField] private AudioSource _soundFx;
        
        #endregion
        
        #region " Internal Variables "

        private bool _canExecuteAction;

        #endregion
        
        protected override void ExecuteOnEnter(Collider other)
        {
            _canExecuteAction = true;
            base.ExecuteOnEnter(other);
        }

        protected override void ExecuteOnExit(Collider other)
        {
            _canExecuteAction = false;
            base.ExecuteOnExit(other);
        }

        private void Update()
        {
            if (false == _canExecuteAction)
                return;

            if (false == CharacterMovement.Instance.IsInteracting)
                return;
            
            if (_soundFx?.isPlaying == true)
                _soundFx?.Stop();
            
            _soundFx.Play();
            
            _onActionTriggered?.Invoke();
        }
    }
}