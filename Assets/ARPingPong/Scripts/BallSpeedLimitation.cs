using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpeedLimitation : MonoBehaviour
{
    public float maxVelocity = 5f;
    private float maxSqrVelocity;
    private Rigidbody _rigidbody;
    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        maxSqrVelocity = maxVelocity * maxVelocity;
    }

    // Update is called once per frame
    void Update()
    {

        if (_rigidbody.velocity.sqrMagnitude > maxSqrVelocity)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * maxVelocity;
        }
    }
}
