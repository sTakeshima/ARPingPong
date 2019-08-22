using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChange : MonoBehaviour
{
    public Vector3 localGravity = new Vector3(0f,4.9f,0f);
    private Rigidbody rb;

    // Start is called before the first frame update 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.useGravity == true)
        {
            setLocalGravity();
        }
    }

    void setLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }
}
