using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Server_controller
{

    public class Sensors : MonoBehaviour
    {
        public GameObject robot;
        [SerializeField] private LayerMask layers;        
        public static Dictionary<string, float> sensorsDic = new Dictionary<string, float>();

        public UdpSocket serverSensors;


    //     public static Dictionnary<string, string> obstacleDic = new Dictionnary<string, string>();
        void Start()
        {
            serverSensors = new UdpSocket();
            serverSensors.Start("127.0.0.1",50002,"test");
            sensorsDic.Add("forward",10);
            sensorsDic.Add("backwards",10);
            sensorsDic.Add("left",10);
            sensorsDic.Add("right",10);
                        
        }

        void Update()
        {
            updateSensors();    
        }

        void updateSensors()
        {
            RaycastHit hit_forward;
            RaycastHit hit_left;
            RaycastHit hit_right;
            RaycastHit hit_backwards;

            // Directions des raycast :
            
            var forward_ray_direction = transform.TransformDirection(Vector3.forward) * 100;
            var left_ray_direction = transform.TransformDirection(Vector3.left) * 100;
            var right_ray_direction = transform.TransformDirection(Vector3.right) * 100;

            //Origines des rayons
            Vector3 origine_forward = transform.position + transform.up * 0.8f + transform.forward * 1.2f - transform.right * 0.3f;
            Vector3 origine_left = transform.position - transform.right * 0.9f + transform.up * 0.8f + transform.forward * 0.95f;
            Vector3 origine_right = transform.position + transform.right * 0.3f + transform.up * 0.8f + transform.forward * 0.95f;
            Vector3 origine_backwards = transform.position- transform.right * 0.3f + transform.up * 0.8f + transform.forward * 0.4f;
            //Affichage des rayons
            Debug.DrawRay(origine_forward, forward_ray_direction, Color.red);
            Debug.DrawRay(origine_left, left_ray_direction, Color.green);
            Debug.DrawRay(origine_right, right_ray_direction, Color.yellow);
            Debug.DrawRay(origine_backwards, right_ray_direction, Color.gray);
            //Détection d'objets pour chaque rayon
            // Physics.Raycast renvoie un booléen True si le rayon touche quelque chose
            //rayon rouge
            bool forward_hit = Physics.Raycast(origine_forward, forward_ray_direction, out hit_forward, 100, layers);
            //rayon vert
            bool left_hit = Physics.Raycast(origine_left, left_ray_direction, out hit_left, 100, layers);
            //rayon jaune
            bool right_hit = Physics.Raycast(origine_right, right_ray_direction, out hit_right, 100, layers);
            //rayon gris
            bool backwards_hit = Physics.Raycast(origine_backwards, right_ray_direction, out hit_backwards, 100, layers);

            sensorsDic["forward"] = hit_forward.distance;
            sensorsDic["left"] = hit_left.distance;            
            sensorsDic["right"] = hit_right.distance;            
            sensorsDic["backwards"] = hit_backwards.distance;

            //Server_Manager.server.SendTo("127.0.0.1",50002,Message.CreationRobotObstacleMessage(obstacleDic["forward"],obstacleDic["backwards"],obstacleDic["left"],obstacleDic["right"]));
            // serverOb.SendTo("127.0.0.1",50009,"{\"id\": 103, \"message\": \"{\"forwardObstacle\": \""+ obstacleDic["forward"] + "\", \"backwardsObstacle\": \""
            //     +obstacleDic["backwards"]+"\", \"leftObstacle\": \""+obstacleDic["left"]+"\", \"rightObstacle\": \""+obstacleDic["right"]+"\"}\"}");

            var jsonstr = new Message(103,"{\"forwardSensor\": \""+ sensorsDic["forward"] + "\", \"backwardsSensor\": \""
                +sensorsDic["backwards"]+"\", \"leftSensor\": \""+sensorsDic["left"]+"\", \"rightSensor\": \""+sensorsDic["right"]+"\"}").ToJson();
            serverSensors.SendTo("127.0.0.1",50009,jsonstr);
        }                        
    }

}    