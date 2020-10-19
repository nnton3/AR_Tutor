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
    [HideInInspector] public UnityEvent ReturnToMainMenuEvent = new UnityEvent();
    #endregion

    private void Awake()
    {
        if (backBtn != null) backBtn.onClick.AddListener(ReturnToBack);
    }

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

    public void ActivatePanel(GameObject nextObj)
    {
        transitionHistory.Push(activeElements);

        if (activeElements.Length > 0)
            foreach (var element in activeElements)
                element.SetActive(false);

        nextObj.SetActive(true);

        activeElements = new GameObject[] { nextObj };
        DisposeReturnBtn();
    }

    public void ReturnToBack()
    {
        Debug.Log("work");
        //DisposeReturnBtn();
        if (transitionHistory.Count < 2) ReturnToSignInScene();

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
        if (transitionHistory.Count - steps < 1) ReturnToSignInScene();

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

    private void ReturnToSignInScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void AddEventToReturnBtn(UnityAction action)
    {
        disposable = backBtn.OnClickAsObservable()
            .Subscribe(_ =>
            {
                action();
            }).AddTo(this);
    }

    public void SwitchBackBtnRender(bool _isVisible)
    {
        backBtn.GetComponent<Image>().enabled = _isVisible;
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
