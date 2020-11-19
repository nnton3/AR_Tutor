using UnityEngine;
using UnityEngine.UI;

public class ButtonsCard : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Color ButtonColor;
    [SerializeField] private Animator targetEffect;
    [SerializeField] private AudioClip clip;
    private Button selectBtn;
    private Color defaultClr = Color.white;
    private Color selectedClr = Color.grey;
    private ButtonsSelector selector;
    public bool Selected { get; private set; }

    private void Awake()
    {
        selector = FindObjectOfType<ButtonsSelector>();
        selectBtn = GetComponent<Button>();
        selectBtn.onClick.AddListener(OnClickEventHandler);
    }

    private void OnClickEventHandler()
    {
        if (MainMenuUIControl.Mode == MenuMode.CustomizeMenu) return;

        if (Selected) Unselect();
        else Select();
    }

    public Color GetColor() { return ButtonColor; }
    public Animator GetEffect() { return targetEffect; }
    public AudioClip GetClip() { return clip; }

    private void Select()
    {
        if (!selector.CanSelect()) return;
        Selected = true;
        img.color = selectedClr;
        selector.SelectEvent.Invoke(this);
    }

    public void Unselect()
    {
        Selected = false;
        img.color = defaultClr;

        if (selector != null)
            selector.UnselectEvent.Invoke(this);
    }
}
