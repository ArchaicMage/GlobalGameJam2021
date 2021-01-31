using System;
using Com.BigSweater.Components;
using Com.BigSweater.Data;
using UnityEngine;

internal sealed class Platform : MonoBehaviour
{
    public LayerMask layers;

    protected CharacterMovement m_CharacterController;

    const float k_SqrMaxCharacterMovement = 1f;

    private Vector3 _previousPosition; 
    
    void OnTriggerStay(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            CharacterMovement character = other.GetComponent<CharacterMovement>();

            if (character != null)
                m_CharacterController = character;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            if (m_CharacterController != null && other.gameObject == m_CharacterController.gameObject)
                m_CharacterController = null;
        }
    }

    public void MoveCharacterController(Vector3 deltaPosition)
    {
        if (m_CharacterController != null && deltaPosition.sqrMagnitude < k_SqrMaxCharacterMovement)
        {
            Vector3 lMoveDirection = m_CharacterController.CalculateTargetDirection();
            float lMoveSpeed = m_CharacterController.GetComponent<Animator>()
                .GetFloat(AnimatorHash.MoveSpeed); 
            Rigidbody lRigidbody = m_CharacterController.GetComponent<Rigidbody>();
            lRigidbody.MovePosition(lRigidbody.position + deltaPosition + lMoveDirection * (lMoveSpeed * Time.fixedDeltaTime));
        }
    }

    private void FixedUpdate()
    {
        if (m_CharacterController)
        {
            Vector3 lDeltaPosition = transform.position - _previousPosition;
        
            MoveCharacterController(lDeltaPosition);
        }
        
        _previousPosition = transform.position;
    }
}
