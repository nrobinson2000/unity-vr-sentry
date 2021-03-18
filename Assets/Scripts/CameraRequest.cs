using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// https://stackoverflow.com/a/39498391
public class CameraRequest : DownloadHandlerScript
{
    public CameraRequest(byte[] buffer, RawImage image) : base(buffer)
    {
        screen = image;
        camTexture = new Texture2D(0, 0);
    }

    // Unity Game objects
    private RawImage screen;
    private Texture2D camTexture;

    // Size of individual frames
    private const int IMAGE_SIZE = 700000;

    // Current frame data
    private byte[] currentFrame = new byte[IMAGE_SIZE];
    private bool frameReady = false;

    // Incoming buffer data (enough for roughly 2 frames)
    private const ulong INCOMING_SIZE = IMAGE_SIZE * 2;
    private byte[] incomingBuff = new byte[INCOMING_SIZE];
    private int buffCount = 0;

    // JPEG protocol constants
    private readonly byte[] jpegHeader = {0xFF, 0xD8};
    private readonly byte[] jpegTrailer = {0xFF, 0xD9};

    // TODO: Expensive, improve runtime
    private int findHeader(byte[] buff, int length)
    {
        if (buff == null || length < 1)
        {
            return -1;
        }

        for (int i = 1; i < length; ++i)
        {
            if (buff[i - 1] == jpegHeader[0] && buff[i] == jpegHeader[1])
            {
                return i - 1;
            }
        }

        return -1;
    }

    // TODO: Expensive, improve runtime
    private int findTrailer(byte[] buff, int length, int start)
    {
        if (buff == null || buff.Length < 1 || start < 0)
        {
            return -1;
        }

        for (int i = start; i < length; ++i)
        {
            if (buff[i - 1] == jpegTrailer[0] && buff[i] == jpegTrailer[1])
            {
                return i + 1;
            }
        }

        return -1;
    }

    private bool streaming = true;
    protected override bool ReceiveData(byte[] cameraData, int dataLength)
    {
        // Data no longer incoming, stop streaming
        if (cameraData == null || dataLength < 1 || !streaming)
        {
            Debug.Log("STREAM ENDED");
            return false;
        }
        
        // Copy incoming data into incomingBuff
        Buffer.BlockCopy(cameraData, 0, incomingBuff, buffCount, dataLength);

        // Update buffCount with number of bytes copied
        buffCount += dataLength;

        // Find header and trailer locations
        int headerPos = findHeader(incomingBuff, buffCount);
        int trailerPos = findTrailer(incomingBuff, buffCount, headerPos);
        int frameLen = trailerPos - headerPos;

        // We found a frame!
        if (frameLen > 0)
        {
            Buffer.BlockCopy(incomingBuff, headerPos, currentFrame, 0, frameLen);
            buffCount = 0;
            frameReady = true;
        }

        // Continue streaming
        return true;
    }

    // Called once per frame in VideoStream
    public void updateImage()
    {
        if (!frameReady) return;
        if (camTexture.LoadImage(currentFrame))
        {
            screen.texture = camTexture;
        }
    }

    public void stopStream()
    {
        streaming = false;
    }
}