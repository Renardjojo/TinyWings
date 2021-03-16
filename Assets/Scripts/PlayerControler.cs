﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerControler : MonoBehaviour
{
    public float thrust = 1f;
    private Rigidbody2D rigid;
    private BoxCollider2D box;
    public float raycastVDistance = 1f;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position - transform.up * (box.size.y / 2 + 0.001f), -transform.up, raycastVDistance);
    }
    
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Debug.DrawLine(transform.position - transform.up * (box.size.y / 2 + 0.001f), transform.position - transform.up * (box.size.y / 2 + 0.001f) + -transform.up * raycastVDistance, Color.green);
            rigid.AddForce(transform.right * thrust, ForceMode2D.Force);
        }
        else
        {
            Debug.DrawLine(transform.position - transform.up * (box.size.y / 2 + 0.001f), transform.position - transform.up * (box.size.y / 2 + 0.001f) + -transform.up * raycastVDistance, Color.red);
        }
    }
}