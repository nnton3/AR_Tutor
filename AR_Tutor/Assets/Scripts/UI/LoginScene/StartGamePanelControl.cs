using UnityEngine;
using UnityEngine.UI;

public class StartGamePanelControl : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite defaultBoyPhoto, defaultGirlPhoto;
    [SerializeField] private Button startBtn;
    [SerializeField] private Text patientName;
    [SerializeField] private AudioClip helloClip;
    private AudioClip clip;
    private LoginManager loginMenuControl;

    private void Awake()
    {
        loginMenuControl = FindObjectOfType<LoginManager>();
    }

    public void Initialize(PatientData _data, string _identifier)
    {
        Debug.Log(_data.PatientGender);
        if (_data.img == null)
        {
            if (_data.PatientGender == "boy") img.sprite = defaultBoyPhoto;
            else img.sprite = defaultGirlPhoto;
        }
        else img.sprite = _data.img;
        if (patientName != null) patientName.text = _data.PatientName;
        if (_data.nameClip != null) clip = _data.nameClip;

        string identifier = _identifier;
        startBtn.onClick.AddListener(() =>
        {
            Debug.Log(_identifier);
            startBtn.interactable = false;
            startBtn.GetComponent<Animator>().SetTrigger("press");
            loginMenuControl.SelectPatient(identifier);
            Signals.PlayAudioClipEvent.Invoke(helloClip);
        });
    }
}
