using Studio.OverOne.Rucksack;
using Studio.OverOne.Rucksack.Components;
using UnityEngine;

public class Book : DependantMonoBehaviour
{
    [GetComponent] private Renderer _renderer;
    
    private void Awake() => ResolveDependencies();

    private void Start()
    {
        _renderer.material.SetColor("_BaseColor", new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)));
    }
}
