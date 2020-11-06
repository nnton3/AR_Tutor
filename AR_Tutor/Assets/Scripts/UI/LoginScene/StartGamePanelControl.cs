using UnityEngine;
using UnityEngine.UI;

public class StartGamePanelControl : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Button startBtn;
    [SerializeField] private Text patientName;
    private LoginManager loginMenuControl;

    private void Awake()
    {
        loginMenuControl = FindObjectOfType<LoginManager>();
    }

    public void Initialize(string _identifier, string _name, Sprite _sprite)
    {
        if (img != null) img.sprite = _sprite;
        if (patientName != null) patientName.text = _name;

        string identifier = _identifier;
        startBtn.onClick.AddListener(() =>
        {
            Debug.Log(_identifier);
            loginMenuControl.SelectPatient(identifier);
        });
    }
}
