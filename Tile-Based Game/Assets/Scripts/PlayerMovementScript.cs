using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] // Player's Rigidbody
    Rigidbody rb;

    [SerializeField] // Player's Speed
    float speed;

    // Update is called once per frame
    void Update()
    {
        // player movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = Vector3.forward * speed;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = Vector3.forward * -speed;
        }
        else
        {
            rb.velocity = Vector3.right * rb.velocity.x;
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = Vector3.right * -speed;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = Vector3.right * speed;
        }
        else
        {
            rb.velocity = Vector3.forward * rb.velocity.z;
        }
    }
}
