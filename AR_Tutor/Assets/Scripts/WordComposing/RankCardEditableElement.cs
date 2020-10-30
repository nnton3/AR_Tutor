using UnityEngine;
using System.Collections;

public class RankCardEditableElement : EditableElement, IEditableElement
{
    public override void ConfigurateElement(MenuMode mode)
    {
        if (mode == MenuMode.CustomizeMenu)
        {
            gameObject.SetActive(true);
            deleteBtn.SetActive(true);
        }
        else
        {
            if (GetComponent<WordComposingCard>().SecondRankCard)
            {
                if (Visible)
                    gameObject.SetActive(true);
                else
                    gameObject.SetActive(false);
            }
            deleteBtn.SetActive(false);
        }
    }
}
