using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnusTst : MonoBehaviour
{
    private Rigidbody rb;
    public float MaxOmega = 1004.8f;
    public Vector3 rotationAxis = Vector3.zero;
    public float curvePara = 0.00000005f;
    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (i==0)
        {
            rb = GetComponent<Rigidbody>();
            rb.maxAngularVelocity = 2000;
            var omega = rotationAxis.normalized * MaxOmega;
            var R = transform.rotation;
            var RI = Quaternion.Inverse(transform.rotation);
            var Id = rb.inertiaTensor;
            var Ir = rb.inertiaTensorRotation;
            var IrI = Quaternion.Inverse(Ir);
            var torque = R * Ir * Vector3.Scale(Id, IrI * RI * omega);
            rb.AddTorque(torque, ForceMode.Impulse);
            i++;
        }

        var mv_rotation = Vector3.Cross(rb.angularVelocity, rb.velocity);
        rb.AddForce(mv_rotation * curvePara, ForceMode.Impulse);
    }
}
