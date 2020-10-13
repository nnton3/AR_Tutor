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
    private StorageReference storageImagesRef;
    private StorageReference storageAudiosRef;
    #endregion
    [SerializeField] private Image img;
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        //StartCoroutine(DownloadSpriteFromCloud("CardImages/Cutter.png"));
        StartCoroutine(LoadAudio());
    }

    private IEnumerator DownloadSpriteFromCloud(string _path)
    {
        storage = FirebaseStorage.DefaultInstance;

        var reference = storage.GetReference(_path);
        var downloadTask = reference.GetBytesAsync(long.MaxValue);
        
        yield return new WaitUntil(() => downloadTask.IsCompleted);
        Debug.Log("work");
        var texture = new Texture2D(100, 100);
        texture.LoadImage(downloadTask.Result);
        
        if (img != null)
        {
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }

    private IEnumerator LoadAudio()
    {
        Uri uri = null;

        storage = FirebaseStorage.DefaultInstance;
        var reference = storage.GetReference("CardAudio/Boom.wav");
        var getUriTask = reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) =>
        {
            Debug.Log("get URI");
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("start loading");
                uri = task.Result;
            }
        });

        yield return new WaitUntil(() => getUriTask.IsCompleted);

        if (uri != null)
        {
            StartCoroutine(LoadRoutine(uri));
            Debug.Log("get URI task complete");
        }
    }

    private IEnumerator LoadRoutine(Uri _uri)
    {
        Debug.Log("stat load routine");
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(_uri, AudioType.WAV))
        {
            Debug.Log("send request");
            yield return request.SendWebRequest();
            Debug.Log("get result");
            if (request.isNetworkError)
                Debug.Log("error");
            else
            {
                Debug.Log("write data");
                clip = DownloadHandlerAudioClip.GetContent(request);
                FindObjectOfType<AudioSource>().PlayOneShot(clip);
            }
        }
    }
}
