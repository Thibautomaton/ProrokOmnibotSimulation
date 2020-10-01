using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Server_controller
{
    public class Server_Manager : MonoBehaviour
    {
        public InputField IP_input;
        public InputField port_input;
        public InputField pwd_input;
        public Text server_ip_label;
        public Text status_label;
        public Text server_port_label;

        private const int PortMin = 1000;
        private const int PortMax = 100000;
        private int portNB;

        public static UdpSocket server;
        
        public void StartServer()
        {
            if (!controllerOmni.isServerActive)
            {
                server = new UdpSocket();
                if (CheckIp(IP_input.text) && CheckPort(port_input.text))
                {
                    server.Start(IP_input.text, portNB, pwd_input.text);
                    if (CheckServerIsRunning())
                    {
                        server_ip_label.text = server_ip_label.text + " " + IP_input.text;
                        server_port_label.text = server_port_label.text + " " + port_input.text;
                        status_label.text = "Online";
                        status_label.color = Color.green;
                        controllerOmni.isServerActive = true;
                    }
                }
            }
        }


        // check if port input is a integer sup to port min and inf to port max
        public bool CheckPort(string port)
        {
            if(int.TryParse(port, out portNB)) {

                if (portNB >= PortMin && portNB <= PortMax)
                {
                    return true;
                }
                else
                {
                    Debug.Log("Server's port must be between " + PortMin + " and " + PortMax);
                    return false;
                }
            }
            else
            {
                Debug.Log("Server's port must be an integer");
                return false;
            }
        }


        // check if ip input is a valid ipv4 adress
        public bool CheckIp(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                Debug.Log("Please fill the ip input");
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                Debug.Log("IP input is not valid IPV4");
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        private bool CheckServerIsRunning()
        {
            return server.IsActive;
        }
    }
}