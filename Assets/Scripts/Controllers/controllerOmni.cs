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
    // bool that check if server is online
    public static bool isServerActive = false;
    public GameObject robot;
    public float rotationSpeed = 10;
    public float speed = 10;
    Vector3 currentEulerAngles;
    float x;
    float y;
    float z;
    public static string jsonMovement = "{\"move_forward\": \"0\", \"move_backwards\": \"0\", \"rotate_left\": \"0\", \"rotate_right\": \"0\"}";
    Movement robotMovement = JsonUtility.FromJson<Movement>(jsonMovement);
    

    // Variables for detection
    private int _t = 0;
    private bool cameraRunning = false;

    public SnapShotCamera snapCam;
    // Start is called before the first frame update
    //Initialisation de nos variables pour le raycast
    //rayDistance est la distance à laquelle le rayon détecte un objet
    [SerializeField] private float rayDistance = 3f;
    //vecteur_correction élève le rayon au-dessus du sol et le centre pour qu'il parte du centre du robot
    private Vector3 vecteur_correction = new Vector3(-0.7f, 0.5f, 0);
    [SerializeField] private LayerMask layers;

    private bool clearAccess = true;

    Dictionary<string, bool> obstacleDic = new Dictionary<string, bool>();           

    void Start()
    {
        obstacleDic.Add("forward",false);
        obstacleDic.Add("backwards",false);
        obstacleDic.Add("left",false);
        obstacleDic.Add("right",false);
    
    }

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
        Movement robotMovement = JsonUtility.FromJson<Movement>(jsonMovement);
        Forward(robotMovement.move_forward, robotMovement.move_backwards);
        Rotate(robotMovement.rotate_left, robotMovement.rotate_right);
        //Sensors();
    }


    private void LateUpdate()
    {
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
        if (forward == 1 && clearAccess)
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

    // void Sensors()
    // {
        

    //     RaycastHit hit_forward_right;
    //     RaycastHit hit_forward_left;
    //     RaycastHit hit_left;
    //     RaycastHit hit_right;
    //     RaycastHit hit_left_backwards;
    //     RaycastHit hit_right_backwards;

    //     //Vector3 offset2 = new Vector3(1.4f, 0, 0);
    //     //Vector3 offset4 = new Vector3(0, 0, 0);
    //     //Vector3 offset3 = new Vector3(1.4f, 0, 0);

    //     // Directions des raycast : bleu et rouge > direction 1
    //     // Vert : direction 3 - Jaune : direction 4
    //     var forward_ray_direction = transform.TransformDirection(Vector3.forward) * rayDistance;
    //     var left_ray_direction = transform.TransformDirection(Vector3.left) * rayDistance;
    //     var right_ray_direction = transform.TransformDirection(Vector3.right) * rayDistance;
    //     //Origines des rayons
    //     Vector3 origine_forward = transform.position + transform.up * 0.8f + transform.forward * 1.2f - transform.right * 0.3f;
    //     Vector3 origine_left = transform.position - transform.right * 0.9f + transform.up * 0.8f + transform.forward * 0.95f;
    //     Vector3 origine_right = transform.position + transform.right * 0.3f + transform.up * 0.8f + transform.forward * 0.95f;
    //     Vector3 origine_backwards = transform.position- transform.right * 0.3f + transform.up * 0.8f + transform.forward * 0.4f;
    //     //Affichage des rayons
    //     Debug.DrawRay(origine_forward, forward_ray_direction, Color.red);
    //     Debug.DrawRay(origine_left, left_ray_direction, Color.green);
    //     Debug.DrawRay(origine_right, right_ray_direction, Color.yellow);
    //     Debug.DrawRay(origine_backwards, right_ray_direction, Color.gray);
    //     //Détection d'objets pour chaque rayon
    //     // Physics.Raycast renvoie un booléen True si le rayon touche quelque chose
    //     //rayon rouge
    //     bool forward_hit = Physics.Raycast(origine_forward, forward_ray_direction, out hit_forward_right, rayDistance, layers);
    //     //rayon vert
    //     bool left_hit = Physics.Raycast(origine_left, left_ray_direction, out hit_left, rayDistance, layers);
    //     //rayon jaune
    //     bool right_hit = Physics.Raycast(origine_right, right_ray_direction, out hit_right, rayDistance, layers);
    //     //rayon gris
    //     bool backwards_hit = Physics.Raycast(origine_backwards, right_ray_direction, out hit_right_backwards, rayDistance, layers);


    //     if (forward_hit)
    //     {
    //         Debug.Log(hit_forward_right.distance);
    //         clearAccess = false;
    //         obstacleDic["forward"] = true;
    //     }
    //     else
    //     {
    //         clearAccess = true;
    //         obstacleDic["forward"] = false;            
    //     }
    //     if (left_hit)
    //     {
    //         Debug.Log(hit_left.distance);
    //         obstacleDic["left"] = true;            
    //     }
    //     else
    //     {
    //         obstacleDic["left"] = false;            
    //     }
    //     if (right_hit)
    //     {
    //         Debug.Log(hit_right.distance);
    //         obstacleDic["right"] = true;            
    //     }
    //     else
    //     {
    //         obstacleDic["right"] = false; 
    //     }        
    //     if (backwards_hit)
    //     {
    //         Debug.Log(hit_right_backwards.distance);
    //         obstacleDic["backwards"] = true;            
    //     }
    //     else
    //     {
    //         obstacleDic["backwards"] = false; 
    //     }        
        
    //     //envoyer message au controleur python
        
    // }

}

public class Movement
{
    public int move_forward;
    public int move_backwards;
    public int rotate_left;
    public int rotate_right;

}