using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This class manages the main camera behaviour * 
*/
public class controllerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject camRobot;

    // Update is called once per frame
    void Update()
    {
        Rotate(); 
    }
    
    void Rotate()
    {
        if (Input.GetKey(KeyCode.P))
        {
            transform.Rotate(-Vector3.up);
        }
        else if (Input.GetKey(KeyCode.M))
        {
            transform.Rotate(Vector3.up);
        }
    }
}
