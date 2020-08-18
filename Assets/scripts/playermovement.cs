using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    public Rigidbody rb;
    public float movementspeed= 10.0f;
    public float mousesensitivity = 5.0f;

    public float verticalRotation= 0f;
    public float updownrange = 60.0f;

    public float verticalvelocity = 25f;
    public float jumpspeed= 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float oldflyaxis;
    // Update is called once per frame
    void Update()
    {   //rotation
        float rotleftright = Input.GetAxis("Mouse X")*mousesensitivity;
        transform.Rotate(0, rotleftright, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mousesensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -updownrange, updownrange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // movement
        float forwardspeed = Input.GetAxis("Vertical")*movementspeed;
        float sidewardspeed = Input.GetAxis("Horizontal")*movementspeed;

        CharacterController cc = GetComponent<CharacterController>();

        //verticalvelocity -= Physics.gravity.magnitude;

        Vector3 speed = new Vector3(sidewardspeed , 0, forwardspeed);

        speed = transform.rotation * speed;
        
        cc.Move(speed*Time.deltaTime);


        //flying!!!
        float flyaxis = Input.GetAxis("Flying");



        Transform PlayerTransForm = gameObject.transform;

        if (GroundTouch && flyaxis != 0f)
        {
            PlayerTransForm.Translate(new Vector3(0, flyaxis * verticalvelocity * Time.deltaTime, 0));
            oldflyaxis = flyaxis;
        }
        else if (flyaxis != 0f)
        {
            if (oldflyaxis/flyaxis < 0)
            {
                PlayerTransForm.Translate(new Vector3(0, flyaxis * verticalvelocity * Time.deltaTime, 0));
            }
        }
            





    }

    Boolean GroundTouch = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            GroundTouch = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            GroundTouch = false;
        }
    }

}
