using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagunusEffect : MonoBehaviour
{
    private Rigidbody rb;
    private float coefficiency = 0.00000005f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var mv_rotation = Vector3.Cross(rb.angularVelocity, rb.velocity);

        /*if(rb.angularVelocity.x >= 0)
        {
            coefficiency = 0.0000001f;
        }else if (rb.angularVelocity.x < 0)
        {
            coefficiency = 0.0000001f;
        }*/
        rb.AddForce(mv_rotation * coefficiency, ForceMode.Impulse);
        
    }
}
