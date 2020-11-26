using UnityEngine;
using UnityEngine.UI;

public class PatientCard : MonoBehaviour
{
    [SerializeField] private Text patientName;
    [SerializeField] private Image img;
    [SerializeField] private Sprite defaultImg;
    private AudioClip nameClip;

    public void Initialize(PatientData _data)
    {
        patientName.text += _data.PatientName;
        if (_data.img != null) img.sprite = _data.img;
        else img.sprite = defaultImg;

        GetComponent<Button>().onClick.AddListener(() => Signals.PlayAudioClipEvent.Invoke(_data.nameClip));
    }
}
