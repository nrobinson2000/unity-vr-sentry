using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VideoStream : MonoBehaviour
{
    // private string camUrl = "http://camera.local/stream/video.mjpeg";

    private string camUrl = "http://192.168.1.16/stream/video.mjpeg";

    public RawImage screen;

    private CameraRequest camImage;
    private UnityWebRequest webRequest;

    private const ulong BUFFER_SIZE = 100000;
    private byte[] bytes = new byte[BUFFER_SIZE];

    void Start()
    {
        PlayStream();
    }

    void Update()
    {
        camImage.updateImage();
    }

    public void PlayStream()
    {
        webRequest = new UnityWebRequest(camUrl);
        camImage = new CameraRequest(bytes, screen);
        webRequest.downloadHandler = camImage;
        var request = webRequest.SendWebRequest();
        Debug.Log("STARTING STREAM!");
    }

    public void PauseStream()
    {
        camImage.stopStream();
        webRequest.Abort();
        Debug.Log("PAUSING STREAM!");
    }
}