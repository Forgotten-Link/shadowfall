using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponentInChildren<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(moveInput * speed, body.velocity.y);

        Debug.Log("Moving with velocity: " + body.velocity);
    }
}

