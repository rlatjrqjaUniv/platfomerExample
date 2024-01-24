using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Idling,Moving,Jumping,Falling }
    [SerializeField] State state;
    
    public float defaultMoveSpeed;
    public float acceleration;
    public float deceleration;
    public float MaxSpeed;
    public float TimeToMaxSpeed;

    public float jumpPower;

    private Rigidbody2D rb;
    [SerializeField] private float recentSpeed;
    private bool isLeftMove = false;
    private bool isRightMove = false;
    private bool isJuming = false;

    private void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(isLeftMove)
        {
            transform.position = new Vector3(transform.position.x - (recentSpeed * 0.005f),
            transform.position.y, transform.position.z);
        }

        else if(isRightMove)
        {
            transform.position = new Vector3(transform.position.x + (recentSpeed * 0.005f),
                transform.position.y, transform.position.z);
        }
    }

    public void MoveLeft()
    {
        if (recentSpeed < defaultMoveSpeed) recentSpeed = defaultMoveSpeed;
        isRightMove = false;
        isLeftMove = true;
        StartCoroutine(IncreaseSpeed());
    }

    public void MoveRight()
    {
        if(recentSpeed < defaultMoveSpeed) recentSpeed = defaultMoveSpeed;
        isLeftMove = false;
        isRightMove = true;
        StartCoroutine(IncreaseSpeed());
    }

    public void Jump()
    {
        if (!isJuming)
        {
            isJuming = true;
            rb.AddForce(Vector2.up * jumpPower * 100);
        }
    }

    public void Idle()
    {
        StartCoroutine(DecreaseSpeed());
    }

    IEnumerator IncreaseSpeed()
    {
        if (recentSpeed < MaxSpeed) recentSpeed += acceleration;
        yield return new WaitForSeconds(0.01f * TimeToMaxSpeed);
    }

    IEnumerator DecreaseSpeed()
    {
        if (recentSpeed > 0) recentSpeed -= deceleration;
        if (recentSpeed < 0) recentSpeed = 0;
        yield return new WaitForSeconds(0.01f * TimeToMaxSpeed);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJuming = false;
    }

}
