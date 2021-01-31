using UnityEngine;

namespace Com.BigSweater.Components.Abstractions
{
    internal interface ICharacterMovement
    {
        Vector2 LookInput { get; }
        
        Vector2 MoveInput { get; }
        
        float MoveSpeed { get; }
    }
}