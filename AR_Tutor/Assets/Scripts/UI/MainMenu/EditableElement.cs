using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableElement : MonoBehaviour
{
    [SerializeField] private GameObject deleteBtn, selectImageBtn;
    public bool Visible { get; set; } = true;

    public virtual void ConfigurateElement(MenuMode mode)
    {
        if (mode == MenuMode.CustomizeMenu)
        {
            gameObject.SetActive(true);
            deleteBtn.SetActive(true);
            selectImageBtn.SetActive(true);
        }
        else
        {
            if (!Visible) gameObject.SetActive(false);
            else gameObject.SetActive(true);

            deleteBtn.SetActive(false);
            selectImageBtn.SetActive(false);
        }
    }
}
