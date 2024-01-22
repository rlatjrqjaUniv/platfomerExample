using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Idling,Moving,Jumping,Falling }
    [SerializeField] State state;
    
    public float moveCooltime; // 이동 키를 눌렀을때 소수점 단위로 힘을 가할건지(횟수), (moveSpedd * moveColltime = 가해지는 힘이라고 보면 됨)
    public float moveSpeed; // move Cooltime 1회에 얼만큼의 힘을 가할것인지(힘)
    // moveCooltime 초 동안 moveSpeed로 가해진 힘이 다 소진된다면 가속도가 붙지 않음
    // moveCooltime 초 동안 moveSpeed로 가해진 힘이 남아있다면 가속도가 붙게 됨.
    // ex) moveCooltime 0.1초, moveSpeed 100 이라면 0.1초동안 100의 힘이 다 소진되어서 가속도X
    // ex) moveCooltime 0.1초, moveSpeed 110 이라면 0.1초동안 110의 힘이 다 소진되기 전에 0.1초가 지나고, 110의 힘이 또 추가되기 때문에 가속도O
    
    public float MaxSpeed; //Rigidbody에 info 보면 velocity 있음 너무 가속되지 않게 추가할 예정
    public float jumpPower;

    public Rigidbody2D rb;
    private bool isMoving = false;
    private bool isJuming = false;

    public event EventHandler OnKeyPressed;

    private void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed);
    }

    public void MoveLeft()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine(Vector2.left));
        }
    }

    public void MoveRight()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine(Vector2.right));
        }
    }

    public void Jump()
    {
        if(!isJuming)
        {
            isJuming = true;
            rb.AddForce(Vector2.up * jumpPower * 100);
        }
    }

    IEnumerator MoveCoroutine(Vector2 direction)
    {
        isMoving = true;
        rb.AddForce(direction * moveSpeed, ForceMode2D.Force);
        yield return new WaitForSeconds(moveCooltime); // 조절 가능한 딜레이 시간
        isMoving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJuming = false;
    }
}
