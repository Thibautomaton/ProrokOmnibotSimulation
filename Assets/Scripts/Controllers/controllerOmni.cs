using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Security.Permissions;
using System.Threading;
using Assets.Server_controller;
using UnityEngine;
using UnityEngine.UI;

public class controllerOmni : MonoBehaviour
{
    // isServerActive check if server is online
    public static bool isServerActive = false;
    public GameObject robot;
    public float rotationSpeed = 10;
    public float speed = 10;
    public static string jsonMovement = "{\"move_forward\": \"0\", \"move_backwards\": \"0\", \"rotate_left\": \"0\", \"rotate_right\": \"0\"}";
    Movement robotMovement;
    

    // Variables for detection
    private bool cameraRunning = false;
    public SnapShotCamera snapCam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !cameraRunning)
        {
            Debug.Log("Sending IMAGES has started");
            cameraRunning = true;
        }

        else if (Input.GetKeyDown(KeyCode.Space) && cameraRunning)
        {
            Debug.Log("Sending IMAGES stopped");
            cameraRunning = false;
        }

        if (cameraRunning)
        {
            snapCam.TakeSnapShot();
        }

        //Movement of the robot using python controller

        robotMovement = JsonUtility.FromJson<Movement>(jsonMovement);
        Forward(robotMovement.move_forward, robotMovement.move_backwards);
        Rotate(robotMovement.rotate_left, robotMovement.rotate_right);
    }

    private void LateUpdate()
    {

        // Send images to python controller
        
        if (isServerActive)
        {
            var bytes = snapCam.EncodeImage();
            if (bytes != null && bytes.Length > 0)
            {
                Server_Manager.server.SendImageTo("127.0.0.1", 50000, bytes);
            }
        }
    }

    void Forward(int forward, int backwards)
    {
        if (forward == 1)
        {
            transform.Translate(Vector3.forward * ((Time.deltaTime) * speed));

        }
        else if (backwards == 1)
        {
            transform.Translate(-Vector3.forward * ((Time.deltaTime) * speed));

        }
    }

    void Rotate(int left, int right)
    {
        if (left == 1)
        {
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
            //modifying the Vector3, based on input multiplied by speed and time

        }
        else if (right == 1)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            //modifying the Vector3, based on input multiplied by speed and time  
        }
    }

}

public class Movement
{
    public int move_forward;
    public int move_backwards;
    public int rotate_left;
    public int rotate_right;

}