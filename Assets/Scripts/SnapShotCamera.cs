using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SnapShotCamera : MonoBehaviour
{
    public Camera snapCam;

    private int resWidth = 455;
    private int resHeight = 256;
    

    private void Awake()
    {
        snapCam = GetComponent<Camera>();
        if (snapCam.targetTexture == null)
        {
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);
        }
        else
        {
            var targetTexture = snapCam.targetTexture;
            resWidth = targetTexture.width;
            resHeight = targetTexture.height;
        }
        snapCam.gameObject.SetActive(false);
    }
    
    public void TakeSnapShot()
    {
        snapCam.gameObject.SetActive(true);
    }

    public byte[] EncodeImage()
    {
        byte[] bytes = new byte[]{}; 
        
        if (snapCam.gameObject.activeInHierarchy)
        {
            Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            bytes = snapshot.EncodeToJPG();
            snapCam.gameObject.SetActive(false);
        }
        return bytes;
        
        
        
    }

    public void SaveImageToFile()
    {
        if (snapCam.gameObject.activeInHierarchy)
        {
            Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            byte[] bytes = snapshot.EncodeToJPG();
            string fileName = SnapshotName();
            System.IO.File.WriteAllBytes(fileName, bytes);
            Debug.Log("Snapshot taken");
            Debug.Log(Application.dataPath);
            snapCam.gameObject.SetActive(false);
        }
    }
    
    string SnapshotName()
    {
        return string.Format("{0}/Snapshots/snap_{1}x{2}_{3:yyyy-MM-dd_HH-mm-ss}.jpg", Application.dataPath, resWidth, resHeight, System
            .DateTime.Now);
    }
}
