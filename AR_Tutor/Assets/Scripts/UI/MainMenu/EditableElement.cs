using UnityEngine;

public class EditableElement : MonoBehaviour, IEditableElement
{
    [SerializeField] protected GameObject deleteBtn, selectImageBtn;
    public bool Visible { get; set; } = true;

    public virtual void ConfigurateElement(MenuMode mode)
    {
        if (mode == MenuMode.CustomizeMenu)
        {
            gameObject.SetActive(true);
            deleteBtn.SetActive(true);
            if (selectImageBtn != null) selectImageBtn.SetActive(true);
        }
        else
        {
            if (!Visible) gameObject.SetActive(false);
            else gameObject.SetActive(true);

            deleteBtn.SetActive(false);
            if (selectImageBtn != null) selectImageBtn.SetActive(false);
        }

        GetComponent<ISwitchedDeleteBtnImg>()?.SwitchImgForDeleteBtn();
    }
}
