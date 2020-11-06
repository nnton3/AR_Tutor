using UnityEngine;

public class ButtonsStartGameBtnControl : MonoBehaviour
{
    [SerializeField] private GameObject startGameBtn;
    private ButtonsSelector selector;

    private void Awake()
    {
        selector = FindObjectOfType<ButtonsSelector>();
    }

    private void OnEnable()
    {
        startGameBtn.SetActive(true);
    }

    private void OnDisable()
    {
        selector.UnselectAll();
        startGameBtn.SetActive(false);
    }
}
