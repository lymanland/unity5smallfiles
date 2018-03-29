using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlefMove : MonoBehaviour {
    private readonly int MaxSpeed = 4;
    private Vector3 runSpeed;

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        runSpeed = new Vector3(Random.Range(-MaxSpeed, MaxSpeed), 0, Random.Range(-MaxSpeed, MaxSpeed));

        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void FixedUpdate()
    {
        //rb.velocity = runSpeed * Time.deltaTime;
        rb.velocity = runSpeed;
    }
}
