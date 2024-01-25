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

    void Update()
    {
        // 만약 점프 중이 아니고, 점프 버튼이 눌렸을 경우
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            // 점프 중으로 상태 변경
            isJumping = true;

            // Rigidbody2D를 이용하여 위쪽으로 점프
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
        }
    }

    // 충돌이 발생했을 때 호출되는 메서드
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 점프 중이 아님으로 상태 변경
        isJumping = false;
    }
}
