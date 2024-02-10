using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump : MonoBehaviour
{

    //// 점프 변수 ////
    private bool isJumping = false;                              // 점프 중인지 여부를 나타내는 bool
    private bool canFly = false;                                 // 더블 점프 가능 여부
    private float flyTime;                                       // 체공 시간
    
    public float maxFlyTime; // 체공 시간 설정

    public float checkRunSpeed;                                  // 달리면서 높게 점프를 할 수 있는 속도의 기준
    
    [Range(1, 20)] public float hightJump;                       // 점프의 최대 높이 기본값
    [Range(1, 25)] public float runHightJump;                    // 달리면서 점프의 최대 높이를 설정
    
    public float currentHightJump;                               // 현재 점프의 최대 높이

    public float fallSpeed = 2.5f;                               // 하강 중일 때 키 떼면 아래로 떨어지는 속도 조절 변수
    public float lowJumpSpeed = 2f;                              // 상승 중일 때 키 떼면 상승 속도를 감소시키는 변수

    //// 컴포넌트 ////
    Rigidbody2D rb;
    PlayerController pc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();                        // Rigidbody2D 컴포넌트 가져오기
        pc = GetComponent<PlayerController>();                   // PlayerController 컴포넌트 가져오기
    }
    void Update()
    {
        if (!isJumping && Input.GetButtonDown("Jump"))           //// 만약 점프 중이 아니고, 점프 버튼이 눌렸을 경우
        {
            isJumping = true;                                    // 점프 중으로 상태 변경
            rb.velocity = Vector2.up * currentHightJump;         // Rigidbody2D를 이용하여 위쪽으로 점프
            canFly = true;                                       // 더블 점프 가능 상태로 변경
            flyTime = maxFlyTime;
        }
        else if (canFly && Input.GetButtonDown("Jump"))          //// 만약 더블 점프가 가능하고, 점프 버튼이 눌렸을 경우
        {
            canFly = false;                                      // 더블 점프 이후에는 다시 더블 점프를 할 수 없도록 변경
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // 위치 고정 (날기) 꾹 누르고있으면 그대로 멈춰있음
        }
        else if(!canFly && Input.GetButton("Jump"))              //// 만약 점프 버튼을 꾹 누르고 있을 경우
        {
            flyTime -= Time.deltaTime;                           // 플라이 시간이 다 될 때까지 날기
            if (flyTime <= 0)                                    
            {
                rb.velocity = new Vector2(0,-0.000001f);
                rb.constraints = RigidbodyConstraints2D.None;    // 위치 고정 해제, 로테이션은 다시 고정
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else if (Input.GetButtonUp("Jump"))                      //// 만약 점프 버튼을 뗐을 때
        {
            rb.constraints = RigidbodyConstraints2D.None;        // 위치 고정 해제, 로테이션은 다시 고정
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }


        if (rb.velocity.y < 0)                                   //// 만약 현재 수직 속도가 아래로 향하고 있다면
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;   // 중력에 fallSpeed 적용
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))  //// 만약 현재 수직 속도가 위로 향하고 있고, 점프 버튼이 눌리지 않았다면
        { 
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpSpeed - 1) * Time.deltaTime;  // 중력에 lowJumpSpeed 적용
        }

    }

    private void FixedUpdate()                                   // 물리 업데이트 주기에 맞춰 호출되므로 사용
    {
        currentHightJump = hightJump;                            // 기본 최대 높이로 설정

        if (pc.recentSpeed >= checkRunSpeed)                     //// 현재 달리기 속도가 체크 런 스피드 보다 크거나 같으면
        {
            currentHightJump = runHightJump;                     // 달리면서 점프의 최대 높이를 적용
        }
        else                                                     //// 현재 달리기 속도가 체크 런 스피드 보다 작다면
        {
            currentHightJump = hightJump;                        // 점프의 최대 높이 기본값 적용
        }

        if (rb.velocity.y < 0)                                   // 플레이어가 낙하중일 때
        {

            Bounds bounds = GetComponent<Collider2D>().bounds;   // 콜라이더의 경계를 얻음 (직접 설정해도 되긴 함)

            RaycastHit2D rayHit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, 1, LayerMask.GetMask("Platform"));
                                                                 // 아래 방향으로 1만큼 ray 쏴서, Platform 인지 확인
            Vector2 start = new Vector2(bounds.min.x, bounds.center.y); // Boxcast의 시작점 계산
            Vector2 end = new Vector2(bounds.max.x, bounds.center.y);   // Boxcast의 끝점 계산
            
            Debug.DrawRay(start, Vector2.down * 1, Color.red);
            Debug.DrawRay(end, Vector2.down * 1, Color.red);
            if (rayHit.collider != null)                         //// Platform과 충돌했다면
            { 
                isJumping = false;                               // 점프 중이 아님

                if (rb.velocity.y < -25)                         // 떨어지는 속도가 -25보다 작았다면 (빨리 떨어졌다면) *그런데 이건 속도 기준이니까 나중에 높이 기준으로 바꿔야 하려나..?
                {  
                    pc.HasDamaged();                             // PlayerController.cs에 HasDamaged 함수 실행
                }
            }
            else                                                 // Platform과 충돌하지 않았다면
            {
                isJumping=true;                                  // 점프 중
            }
        }
    }


}