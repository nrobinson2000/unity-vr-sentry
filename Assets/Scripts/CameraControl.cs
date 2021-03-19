using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraControl : MonoBehaviour
{
    private string apiUrl = "http://192.168.1.16:8080/api";
    private UnityWebRequest webRequest;

    public void moveLeft()
    {
        webRequest = new UnityWebRequest(apiUrl + "/move?direction=left");
        webRequest.SendWebRequest();
    }

    public void moveRight()
    {
        webRequest = new UnityWebRequest(apiUrl + "/move?direction=right");
        webRequest.SendWebRequest();
    }

    public void moveUp()
    {
        webRequest = new UnityWebRequest(apiUrl + "/move?direction=up");
        webRequest.SendWebRequest();
    }

    public void moveDown()
    {
        webRequest = new UnityWebRequest(apiUrl + "/move?direction=down");
        webRequest.SendWebRequest();
    }


    public void lightToggle()
    {
        webRequest = new UnityWebRequest(apiUrl + "/toggle-light");
        webRequest.SendWebRequest();  
    }
    
    
    
    
    
    
    
    
    
}