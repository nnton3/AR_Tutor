using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTransitionController : MonoBehaviour
{
    private Stack<GameObject[]> transitionHistory = new Stack<GameObject[]>();
    private GameObject[] activeElements = new GameObject[] { };
    private GameObject[] oldElements = new GameObject[] { };
    [SerializeField] private Button backBtn;

    public void ActivatePanel(GameObject[] nextObjs)
    {
        transitionHistory.Push(activeElements);

        if (activeElements.Length > 0)
            foreach (var element in activeElements)
                element.SetActive(false);

        foreach (var element in nextObjs)
            element.SetActive(true);

        activeElements = nextObjs;
    }

    public void ReturnToBack()
    {
        if (transitionHistory.Count < 2) return;

        if (activeElements.Length > 0)
            foreach (var element in activeElements)
                element.SetActive(false);

        var previuseElements = transitionHistory.Pop();

        foreach (var element in previuseElements)
            element.SetActive(true);

        activeElements = previuseElements;
    }

    public void ReturnToBack(int steps)
    {
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
    }
}
