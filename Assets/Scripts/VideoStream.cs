using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VideoStream : MonoBehaviour
{
    private string camUrl = "http://camera.local/stream/video.mjpeg";

    public RawImage screen;

    private CameraRequest camImage;
    private UnityWebRequest webRequest;
    
    private const ulong BUFFER_SIZE = 90000;
    private byte[] bytes = new byte[BUFFER_SIZE];
    
    // Start is called before the first frame update
    void Start()
    {
        webRequest = new UnityWebRequest(camUrl);
        webRequest.downloadHandler = new CameraRequest(bytes, screen);
        var request = webRequest.SendWebRequest();

        Debug.Log("sent request");



        
        
        // TODO: handle async event
        // BAD!
        // while (!request.isDone)
        // {
        //     
        // }
        
        // request.webRequest.result.

    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
