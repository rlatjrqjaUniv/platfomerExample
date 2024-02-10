/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    float moveX,moveY;
    bool isJump;

    Animator animator;
    SpriteRenderer playerDir;
    Rigidbody2D rb;

    void Start()
    {
        playerDir = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        transform.Translate(moveX, 0, 0);


        //좌우 이동시 캐릭터 뒤집기
        if (Input.GetAxis("Horizontal") == 0)
        {
            animator.SetBool("isRun", false);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            playerDir.flipX = false;
            animator.SetBool("isRun", true);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            playerDir.flipX = true;
            animator.SetBool("isRun", true);
        }


        //이중 점프 제한
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isJump)
            {
                rb.AddForce(Vector2.up * 900f);
                isJump = true;
            }
        }


        //천천히 떨어지기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.drag = 7f;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.drag = 0f;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJump = false;
    }
}*/