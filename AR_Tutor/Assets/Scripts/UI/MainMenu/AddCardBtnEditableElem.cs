using UnityEngine;

public class AddCardBtnEditableElem : EditableElement
{
    private void Awake()
    {
        Visible = false;
    }

    public override void ConfigurateElement(MenuMode mode)
    {
        if (mode == MenuMode.CustomizeMenu)
            gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }

    private void OnEnable()
    {        
        transform.SetAsLastSibling();
    }
}
