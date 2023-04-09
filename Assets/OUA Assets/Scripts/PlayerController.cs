using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float hiz = 8f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isRun = false; //Animasyon için
    private float hareketYon;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Yürüme kısmı oklar ve w,a,s,d
        float moveY = Input.GetAxis("Horizontal");
        float moveX = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveY, moveX);
        rb.velocity = movement * hiz;
        if (movement != Vector2.zero)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
        animator.SetBool("isRun", isRun); //Animasyonla aynı yapıyorum

        hareketYon = Input.GetAxisRaw("Horizontal"); //Gittiği yöne bakması için 
        if (hareketYon > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (hareketYon < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
