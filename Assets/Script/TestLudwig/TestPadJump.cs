using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPadJump : MonoBehaviour
{

    public Rigidbody RB;
    public float jump;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RB.AddForce(new Vector3(RB.velocity.x, jump));
        }
    }
}
