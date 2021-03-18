using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraMove : MonoBehaviour
{
    private string apiUrl = "http://192.168.1.16:8080/api/move";
    private UnityWebRequest webRequest;

    public void moveLeft()
    {
        webRequest = new UnityWebRequest(apiUrl + "?direction=left");
        webRequest.SendWebRequest();
    }

    public void moveRight()
    {
        webRequest = new UnityWebRequest(apiUrl + "?direction=right");
        webRequest.SendWebRequest();
    }

    public void moveUp()
    {
        webRequest = new UnityWebRequest(apiUrl + "?direction=up");
        webRequest.SendWebRequest();
    }

    public void moveDown()
    {
        webRequest = new UnityWebRequest(apiUrl + "?direction=down");
        webRequest.SendWebRequest();
    }
}