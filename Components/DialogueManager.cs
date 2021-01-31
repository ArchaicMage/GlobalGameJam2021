using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float _fadeTime = .5f;

    [SerializeField] private int _sceneIndex;

    [SerializeField] private AudioSource _confirm;
    
    [SerializeField] private CanvasGroup fadeOutImageCanvasGroup;

    [SerializeField] private List<Dialogue> _dialogues = new List<Dialogue>();

    private int _current;
    
    private void OnInteraction(InputValue value)
    {
        if (value.isPressed)
            MoveForward();
    }
    
    private IEnumerator Start()
    {
        yield return StartCoroutine(FadeCanvasGroupAlpha(1f,0f, fadeOutImageCanvasGroup));

        StartCoroutine(Move(_current));
    }

    [ContextMenu("Move Forward")]
    public void MoveForward()
    {
        if (_current + 1 > _dialogues.Count)
            return;
        
        StartCoroutine(Move(_current + 1));
        _current += 1;
    }

    [ContextMenu("Move Backward")]
    public void MoveBackward()
    {
        if (_current - 1 < 0)
            return;
        
        StartCoroutine(Move(_current - 1));
        
        _current -= 1;
    }
    
    private IEnumerator Move(int next)
    {
        if (_current < _dialogues.Count)
        {
            if (_confirm?.isPlaying == true)
                _confirm?.Stop();
                
            _confirm?.Play();
            
            CanvasGroup lCurrent = _dialogues[_current].CanvasGroup;
            if (lCurrent.alpha > 0)
            {
                yield return StartCoroutine(FadeCanvasGroupAlpha(1f,0f, lCurrent));
            }   
        }

        if (next < _dialogues.Count)
        {
            CanvasGroup lNext = _dialogues[next].CanvasGroup;
            yield return StartCoroutine(FadeCanvasGroupAlpha(0f,1f, lNext));    
        }
        else
        {
            yield return new WaitForSeconds(_fadeTime);
        }
        
        if (next >= _dialogues.Count)
        {
            Invoke("TransitionScene", 1f);
            
            StartCoroutine(FadeCanvasGroupAlpha(0f, 1f, fadeOutImageCanvasGroup));
        }
    }

    private void TransitionScene()
    {
        SceneManager.LoadScene(_sceneIndex, LoadSceneMode.Single);
    }
    
    private IEnumerator FadeCanvasGroupAlpha(float startAlpha, float endAlpha, CanvasGroup canvasGroupToFadeAlpha)
    {

        float elapsedTime = 0f;
        float totalDuration = _fadeTime;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);
            canvasGroupToFadeAlpha.alpha = currentAlpha;
            yield return null;
        }
    }
}
