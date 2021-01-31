using Studio.OverOne.Rucksack;
using Studio.OverOne.Rucksack.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.BigSweater.Components.Abstractions
{
    using Com.BigSweater.Components.Abstractions;
    using Com.BigSweater.Data;
    
    [RequireComponent(typeof(PlayerInput))]
    internal abstract class CharacterAction : DependantMonoBehaviour, ICharacterAction
    {
        #region " Dependency Injection Variables "

        [GetComponent] protected readonly PlayerInput PlayerInput;
        
        #endregion

        private void Awake() => ResolveDependencies();
    }
}