using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MousePressEvents : MonoBehaviour
{
    public UnityEvent OnPress;
    public bool CanPress = true;

    private void OnMouseDown()
    {
        if (CanPress)
        {
            if (OnPress != null)
                OnPress.Invoke();
        }
    }
}
