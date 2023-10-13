using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassMovement : MonoBehaviour
{
    public float speed;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();


    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 MoveBall = new Vector3 (horizontalMovement, 0, verticalMovement);

        rb.AddForce(MoveBall * speed);

    }
}
