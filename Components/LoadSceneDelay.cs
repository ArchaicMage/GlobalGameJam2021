
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneDelay : MonoBehaviour
{
    [SerializeField] private float _delay = 5f;

    [SerializeField] private int _sceneIndex = 0;
    
    private float _delayTimer;
    
    private void FixedUpdate()
    {
        _delayTimer += Time.fixedDeltaTime;
        if (_delayTimer > _delay)
            SceneManager.LoadScene(_sceneIndex, LoadSceneMode.Single);
    }
}
