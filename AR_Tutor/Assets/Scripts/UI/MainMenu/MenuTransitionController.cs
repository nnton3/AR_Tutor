using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuTransitionController : MonoBehaviour
{
    #region Variables
    private Stack<GameObject[]> transitionHistory = new Stack<GameObject[]>();
    private GameObject[] activeElements = new GameObject[] { };
    private GameObject[] oldElements = new GameObject[] { };
    private IDisposable disposable;
    [SerializeField] private Button backBtn;
    public UnityEvent ReturnToMainMenuEvent = new UnityEvent();
    #endregion

    public void ActivatePanel(GameObject[] nextObjs)
    {
        transitionHistory.Push(activeElements);

        if (activeElements.Length > 0)
            foreach (var element in activeElements)
                element.SetActive(false);

        foreach (var element in nextObjs)
            element.SetActive(true);

        activeElements = nextObjs;
        DisposeReturnBtn();
    }

    public void ReturnToBack()
    {
        DisposeReturnBtn();
        if (transitionHistory.Count < 2) return;

        if (activeElements.Length > 0)
            foreach (var element in activeElements)
                element.SetActive(false);

        var previuseElements = transitionHistory.Pop();

        foreach (var element in previuseElements)
            element.SetActive(true);

        activeElements = previuseElements;

        if (transitionHistory.Count == 1) ReturnToMainMenuEvent?.Invoke();
    }

    public void ReturnToBack(int steps)
    {
        DisposeReturnBtn();
        if (transitionHistory.Count - steps < 1) return;

        if (activeElements.Length > 0)
            foreach (var element in activeElements)
                element.SetActive(false);

        GameObject[] previuseElements = new GameObject[] { };
        for (int i = 0; i < steps; i++)
            previuseElements = transitionHistory.Pop();

        foreach (var element in previuseElements)
            if (element != null)
                element.SetActive(true);

        activeElements = previuseElements;

        if (transitionHistory.Count == 1) ReturnToMainMenuEvent?.Invoke();
    }

    public void AddEventToReturnBtn(UnityAction action)
    {
        disposable = backBtn.OnClickAsObservable()
            .Subscribe(_ => action()).AddTo(this);
    }

    public void DisposeReturnBtn()
    {
        if (disposable != null)
            disposable.Dispose();
    }

    private void OnDestroy()
    {
        DisposeReturnBtn();
    }
}
