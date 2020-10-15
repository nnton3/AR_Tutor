using System.Collections;
using System.Collections.Generic;
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
    public Sprite LastLoadedSprite { get; private set; } = null;
    public AudioClip LastLoadedClip { get; private set; } = null;
    #endregion

    private void Start()
    {
        //StartCoroutine(DownloadSpriteFromCloud("CardImages/Cutter.png"));
        //StartCoroutine(LoadAudio());
    }

    public IEnumerator DownloadSprite(string _path)
    {
        storage = FirebaseStorage.DefaultInstance;

        var reference = storage.GetReference($"CardImages/{_path}");
        var downloadTask = reference.GetBytesAsync(long.MaxValue);

        yield return new WaitUntil(() => downloadTask.IsCompleted);

        var texture = new Texture2D(100, 100);
        texture.LoadImage(downloadTask.Result);

        LastLoadedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    public IEnumerator DownloadAudio(string _path)
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
            yield return StartCoroutine(LoadRoutine(uri));
    }

    private IEnumerator LoadRoutine(Uri _uri)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(_uri, AudioType.WAV))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                Debug.Log("error");
            else
            {
                Debug.Log("write data");
                LastLoadedClip = DownloadHandlerAudioClip.GetContent(request);
            }
        }
    }
}
