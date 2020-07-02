using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perso2Controller : MonoBehaviour
{
    //var rotate :
    private float rotate = 0f;
    public float rotationSpeed = 200f;

    //var move : 
    private float movedirection = 0f;
    public float speed = 5f;

    //
    private CharacterController Claude;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Claude = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Movement();
        transform.Rotate(new Vector3(0, rotate * rotationSpeed * Time.deltaTime, 0));
        Vector3 forward = movedirection * transform.TransformDirection(Vector3.forward) * speed;
        Claude.Move(forward * Time.deltaTime);
        Claude.SimpleMove(Physics.gravity);

        void Movement()
        {
            if (Input.GetKey(KeyCode.I))
            {
                movedirection = 1f;
                anim.SetBool("walk", true);
                anim.SetBool("backwards", false);
            }
            else if (Input.GetKey(KeyCode.K))
            {
                movedirection = -1f;
                anim.SetBool("walk", false);
                anim.SetBool("backwards", true);
            }
            else
            {
                movedirection = 0f;
                anim.SetBool("walk", false);
                anim.SetBool("backwards", false);
            }
        }




        void Rotate()
        {
            if (Input.GetKey(KeyCode.J))
            {
                rotate = -1;
            }
            else if (Input.GetKey(KeyCode.L))
            {
                rotate = 1;
            }
            else
            {
                rotate = 0f;
            }
        }
    }
}
