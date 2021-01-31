using System;
using DG.Tweening;
using Studio.OverOne.Rucksack;
using Studio.OverOne.Rucksack.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Com.BigSweater.Components
{
    public class CheckInputType : DependantMonoBehaviour
    {
        private const string GAMEPAD_SCHEME = "Gamepad";
        
        private const string KEYBOARD_SCHEME = "Keyboard&Mouse";
        
        [SerializeField] private SpriteRenderer _controllerSpriteRenderer;
        
        [SerializeField] private SpriteRenderer _keyboardSpriteRenderer;

        [SerializeField] private Image _controllerImage;
        
        [SerializeField] private Image _keyboardImage;

        [SerializeField] private bool _disableAnimationOnStart = true;
        
        [SerializeField] private PlayerInput _playerInput;
        
        [GetComponentInChildren] private readonly DOTweenAnimation _doTween;

        private void Awake() => ResolveDependencies();

        private void Start()
        {
            _doTween.gameObject.SetActive(!_disableAnimationOnStart);
        }

        private void Update()
        {
            string lCurrentScheme = _playerInput ? _playerInput.currentControlScheme : KEYBOARD_SCHEME;
            
            if (_controllerSpriteRenderer)
                _controllerSpriteRenderer.enabled = string.Equals(lCurrentScheme, GAMEPAD_SCHEME);
            
            if (_controllerImage)
                _controllerImage.enabled = string.Equals(lCurrentScheme, GAMEPAD_SCHEME);
            
            if (_keyboardSpriteRenderer)
                _keyboardSpriteRenderer.enabled = string.Equals(lCurrentScheme, KEYBOARD_SCHEME);
            
            if (_keyboardImage)
                _keyboardImage.enabled = string.Equals(lCurrentScheme, KEYBOARD_SCHEME);
        }
    }
}