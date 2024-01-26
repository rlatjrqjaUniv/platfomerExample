using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // 점프 중인지 여부를 나타내는 변수
    private bool isJumping = false;

    // 점프 속도를 조절하기 위한 변수
    [Range(1, 30)]
    public float jumpVelocity;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // 만약 점프 중이 아니고, 점프 버튼이 눌렸을 경우
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            // 점프 중으로 상태 변경
            isJumping = true;

            // Rigidbody2D를 이용하여 위쪽으로 점프
            rb.velocity = Vector2.up * jumpVelocity;
        }
    }

  
    // 충돌이 발생했을 때 호출되는 메서드
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 점프 중이 아님으로 상태 변경
        isJumping = false;
    }
}




// 레이캐스트 문제가 많음...
/*  private void FixedUpdate()
    {
        if (rb.velocity.y < 0) // 플레이어가 낙하중일 때 == velocity.y가 음수
        {
            Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0)); //ray를 그리기
            RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Platform")); //ray 쏘기
            if (rayHit.collider != null)
            { // RayCastHit 변수의 콜라이더로 검색 확인 가능
                if (rayHit.distance < 0.5f)
                { // ray가 0.5 이상 들어갔을 때
                    anim.SetBool("isJump", false); // 애니메이션 되돌리기
                }
isJumping = false;
            }
        }
    }
 */