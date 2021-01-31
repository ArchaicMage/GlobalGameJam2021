using System;
using System.Collections;
using System.Collections.Generic;
using Studio.OverOne.Rucksack;
using Studio.OverOne.Rucksack.Components;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Dialogue : MonoBehaviour
{
    public CanvasGroup CanvasGroup => _canvasGroup;
    
    [SerializeField] private TMP_Text _textRef;
    
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
    }
}
