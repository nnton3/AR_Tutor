using UnityEngine;
using UnityEngine.UI;

public class StartGamePanelControl : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite defaultBoyPhoto, defaultGirlPhoto;
    [SerializeField] private Button startBtn;
    [SerializeField] private Text patientName;
    [SerializeField] private AudioClip helloClip;
    private LoginManager loginMenuControl;

    private void Awake()
    {
        loginMenuControl = FindObjectOfType<LoginManager>();
    }

    public void Initialize(PatientData _data, string _identifier)
    {
        if (_data.img == null)
        {
            if (_data.PatientGender == "boy") img.sprite = defaultBoyPhoto;
            else img.sprite = defaultGirlPhoto;
        }
        else img.sprite = _data.img;
        if (patientName != null) patientName.text = _data.PatientName;

        string identifier = _identifier;
        startBtn.onClick.AddListener(() =>
        {
            startBtn.interactable = false;
            startBtn.GetComponent<Animator>().SetTrigger("press");
            loginMenuControl.SelectPatient(identifier);
            Signals.PlayAudioClipEvent.Invoke(helloClip);
        });
    }
}
