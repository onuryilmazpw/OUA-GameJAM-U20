using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float hiz = 8f;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Yürüme kısmı oklar ve w,a,s,d
        float moveY = Input.GetAxis("Horizontal");
        float moveX = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveY, moveX);
        rb.velocity = movement * hiz;
    }
}
