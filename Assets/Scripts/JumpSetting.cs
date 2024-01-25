using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSetting : MonoBehaviour
{
    // 하강 중일 때 키 떼면 아래로 떨어지는 속도 조절 변수
    public float fallMultiplier = 2.5f;

    // 상승 중일 때 키 떼면 상승 속도를 감소시키는 변수
    public float lowJumpMultiplier = 2f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 만약 현재 수직 속도가 아래로 향하고 있다면
        if (rb.velocity.y < 0)
        {
            // 중력에 fallMultiplier 적용
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // 만약 현재 수직 속도가 위로 향하고 있고, 점프 버튼이 눌리지 않았다면
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // 중력을 이용하여 더 느리게 상승 중인 점프 적용
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
