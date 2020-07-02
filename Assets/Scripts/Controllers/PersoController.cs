using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersoController : MonoBehaviour
{

    //var rotate :
    private float rotate = 0f;
    public float rotationSpeed = 200f;

    //var move : 
    private float movedirection = 0f;
    public float speed = 5f;

    //
    private CharacterController Frank;
    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Frank = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Movement();
        transform.Rotate(new Vector3(0, rotate * rotationSpeed * Time.deltaTime, 0));
        Vector3 forward = movedirection * transform.TransformDirection(Vector3.forward) * speed;
        Frank.Move(forward * Time.deltaTime);
        Frank.SimpleMove(Physics.gravity);

        void Movement()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movedirection = 1f;
                anim.SetBool("walk", true);
                anim.SetBool("backwards", false);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
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
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rotate = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
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
