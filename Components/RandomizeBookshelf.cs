using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizeBookshelf : MonoBehaviour
{
    [SerializeField] private float _spawnThreshold = 0.5f;
    
    [SerializeField] private List<GameObject> _gameObjects = new List<GameObject>();

    private void Awake()
    {
        foreach (var lGameObject in _gameObjects)
        {
            float lProbability = Random.Range(0f, 1f);
            lGameObject.SetActive(lProbability < _spawnThreshold);
        }
    }
}
