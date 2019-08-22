using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tst : MonoBehaviour
{
    public GameObject spherePrefab;
    private Rigidbody rb;
    public float speedP = 14f;
    public Vector3 position = new Vector3(0f, 0.5f, -1.5f);
    public Vector3 throwDirection = new Vector3(0f, 1f, 1.414f);

    // Start is called before the first frame update
    void Start()
    {
        rb = spherePrefab.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ball = Instantiate(spherePrefab, position, Quaternion.identity) as GameObject;
            ball.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            float speed = speedP * 1000f / 3600f;
            float v0 = rb.mass * speed;
            ball.GetComponent<Rigidbody>().AddForce(throwDirection.normalized * v0, ForceMode.Impulse);
        }

    }
}
