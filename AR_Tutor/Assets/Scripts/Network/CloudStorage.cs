using System.Collections;
using UnityEngine;
using Firebase.Storage;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CloudStorage : MonoBehaviour
{
    #region Varoables
    private string imagesPath = "gs://ar-tutor.appspot.com/CardImages/";
    private string audiosPath = "gs://ar-tutor.appspot.com/CardAudio/";
    private FirebaseStorage storage;
    public Sprite Img1 { get; private set; } = null;
    public Sprite Img2 { get; private set; } = null;
    public Sprite Img3 { get; private set; } = null;
    public AudioClip Clip1 { get; private set; } = null;
    public AudioClip Clip2 { get; private set; } = null;
    #endregion

    public IEnumerator DownloadSprite(string _path, int _index)
    {
        storage = FirebaseStorage.DefaultInstance;

        var reference = storage.GetReference($"CardImages/{_path}");
        var downloadTask = reference.GetBytesAsync(long.MaxValue);

        yield return new WaitUntil(() => downloadTask.IsCompleted);

        var texture = new Texture2D(100, 100);
        texture.LoadImage(downloadTask.Result);

        switch (_index)
        {
            case 1:
                Img1 = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                break;
            case 2:
                Img2 = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                break;
            case 3:
                Img3 = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                break;
            default:
                break;
        }
    }

    public IEnumerator DownloadAudio(string _path, int _index)
    {
        Uri uri = null;

        storage = FirebaseStorage.DefaultInstance;
        var reference = storage.GetReference($"CardAudio/{_path}");
        var getUriTask = reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
                uri = task.Result;
        });

        yield return new WaitUntil(() => getUriTask.IsCompleted);

        if (uri != null)
            yield return StartCoroutine(LoadRoutine(uri, _index));
    }

    private IEnumerator LoadRoutine(Uri _uri, int _index)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(_uri, AudioType.WAV))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                Debug.Log("error");
            else
            {
                Debug.Log("write data");
                if (_index == 1)
                    Clip1 = DownloadHandlerAudioClip.GetContent(request);
                if (_index == 2)
                    Clip2 = DownloadHandlerAudioClip.GetContent(request);
            }
        }
    }
}
