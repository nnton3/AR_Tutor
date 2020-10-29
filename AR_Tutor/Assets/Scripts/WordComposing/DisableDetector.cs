using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDetector : MonoBehaviour
{
    private void OnDisable()
    {
        Signals.ResetWordComposingMenu.Invoke();
    }
}
