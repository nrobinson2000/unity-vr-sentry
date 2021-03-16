using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


// https://stackoverflow.com/a/39498391
public class CameraRequest : DownloadHandlerScript
{
    // Standard constructor, will allocate memory
    public CameraRequest() : base()
    {
        
    }
    
  // Pre allocated constructor
  public CameraRequest(byte[] buffer, RawImage image) : base(buffer)
  {
      screen = image;
      camTexture = new Texture2D(1280, 720);
  }

  // Called when accessing `bytes` property
  protected override byte[] GetData()
  {
      return null;
  }

  private RawImage screen;
  private Texture2D camTexture;
  
  
  private int byteCounter = 0;
  private bool loadingFrame = false;
  private const int IMAGE_SIZE = 1000000;
  private byte[] completeImage = new byte[IMAGE_SIZE];
  private byte[] incomingBytes = new byte[IMAGE_SIZE];

  // Called once per frame when data is incoming
  protected override bool ReceiveData(byte[] cameraData, int dataLength)
  {
      if (cameraData == null || cameraData.Length < 1)
      {
          // Null/empty buffer
          return false;
      }

      //Debug.Log($"camera data length {cameraData.Length}");
      
    // Search for JPEG data here
    for (int i = 1; i < dataLength; i++)
    {
        // try
        // {
            if (loadingFrame)
            {
                incomingBytes[byteCounter] = cameraData[i-1];
                ++byteCounter;
            }
        // }
        // catch (Exception e)
        // {
        //     Debug.Log($"Num bytes: {byteCounter}");
        //     Debug.Log($"i: {i}");
        //     throw;
        // }

        
        // Beginning of header
        if (cameraData[i-1] == 0xFF && cameraData[i] == 0xD8)
        {
            // Write header
            incomingBytes[0] = 0xFF;
            incomingBytes[1] = 0xD8;
            byteCounter = 2;
            
            loadingFrame = true;
            // Debug.Log($"found starting header at: {i}");
        }
        
        // End of header
        if (cameraData[i-1] == 0xFF && cameraData[i] == 0xD9)
        {
            // Write footer
            incomingBytes[byteCounter++] = 0xFF;
            incomingBytes[byteCounter++] = 0xD9;            
            
            // Debug.Log($"Got a frame! Size: {byteCounter}");
    
            // Buffer.BlockCopy(incomingBytes, 0, completeImage, 0, byteCounter);
            updateImage();
            
            // Reset
            loadingFrame = false;
            byteCounter = 0;
        }
    }
    
    // Look for 0xFF 0xD8 ..... 0xFF 0xD9
    

      return true;
  }

  private void updateImage()
  {
      camTexture.LoadImage(incomingBytes);

      // Texture2D t = screen.texture;
      screen.texture = camTexture;
  }

  // Called when all data has been received
  protected override void CompleteContent()
  {
      Debug.Log("CustomWebRequest :: CompleteContent - DOWNLOAD COMPLETE!");
  }

  // Called when a Content-Length header is received from server
  protected override void ReceiveContentLengthHeader(ulong contentLength)
  {
      base.ReceiveContentLengthHeader(contentLength);
  }
}
