using UnityEngine;

namespace Com.BigSweater.Data
{
    [CreateAssetMenu(fileName = "Movement Type SO", menuName = "3D Game Kit/Movement Type SO")]
    internal class MovementTypeSO : ScriptableObject
    {
        public float TurnSpeed = 10f;

        public float TurnSpeedMultiplier = 1f;

        public float WalkSpeed;
        
        public float JogSpeed;
        
        public float RunSpeed;
    }
}
