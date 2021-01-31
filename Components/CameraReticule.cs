using Studio.OverOne.Rucksack;
using Studio.OverOne.Rucksack.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Com.BigSweater.Components
{
    public class CameraReticule : DependantMonoBehaviour
    {
        public bool IsAiming { get; set; }
        
        #region " Inspector Variables "

        [SerializeField] private Image _normalReticule;

        [SerializeField] private Image _aimReticule;

        #endregion
        
        #region " Dependency Injection Variables "

        [GetComponentInChildren] private readonly Canvas _canvas;
        
        #endregion

        private void Awake() => ResolveDependencies();

        private void Start()
        {
            _canvas.enabled = true;
            _normalReticule.gameObject.SetActive(true);
            _aimReticule.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            _normalReticule.gameObject.SetActive(false == IsAiming);
            _aimReticule.gameObject.SetActive(IsAiming);
        }
    }
}