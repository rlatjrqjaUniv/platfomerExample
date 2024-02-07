using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //enum State { Idling, Moving, Jumping, Falling, Break }
    //[SerializeField] State state;

    //// 캐릭터 정보 : 나중에 컴포넌트 분리 해도 됨 ////
    public GameObject[] lifeIMG;
    int life = 3;
    public bool IsBlinkEffectRunning = false;

    //// 컴포넌트 ////
    private Rigidbody2D rb;
    private SpriteRenderer Sprite;
    private Animator animator;
    private LayerMask layerMask;

    //// 이동 ////
    public float acceleration;                      // 가속도
    public float runacceleration;                   // 달리기 가속도
    public float deceleration;                      // 감속도
    public float turnBreak;                         // 방향전환 감속도
    public float checkRun;                          // 달리는 속도의 기준
    public float MaxSpeed;                          // 최대 속도
    public float recentSpeed;                       // 현재 속도
    private bool isLeftMove = false;                // 왼쪽으로 bool
    private bool isRightMove = false;               // 오른쪽으로 bool

    // 달리기 버튼 //
    private float MaxSpeed2;                        // 저장,교환용 변수
    private float acceleration2;                    // 저장,교환용 변수
    public float RunSpeed;
    // 위에 있는 runacceleration

    void Start()
    {
        MaxSpeed2 = MaxSpeed;
        acceleration2 = acceleration;
        rb = GetComponent<Rigidbody2D>();           // Rigidbody2D 컴포넌트 가져오기
        Sprite = GetComponent<SpriteRenderer>();    // SpriteRenderer 컴포넌트 가져오기
        animator = GetComponent<Animator>();        // Animator 컴포넌트 가져오기
    }

    private void Update()
    {
        if (isLeftMove)
        {
            transform.position = new Vector3(transform.position.x - (recentSpeed * 5f * Time.deltaTime),  // Time.deltaTime 마다. 현재 포지션 - 현재 속도 *5 만큼 이동
            transform.position.y, transform.position.z);
        }
        else if (isRightMove)
        {
            transform.position = new Vector3(transform.position.x + (recentSpeed * 5f * Time.deltaTime),  // Time.deltaTime 마다. 현재 포지션 + 현재 속도 *5 만큼 이동
            transform.position.y, transform.position.z);
        }
        Animations();
    }
    public void Animations()
    {
        if(recentSpeed <= 0)
        {
            animator.SetBool("isMove", false);
            animator.SetBool("isBreak", false);
        }
        else if (recentSpeed < checkRun && recentSpeed > 0)
        {
            animator.SetBool("isRun", false);
            animator.SetBool("isBreak", false);
            animator.SetBool("isMove", true);
        }
        else if (recentSpeed >= checkRun)
        {
            animator.SetBool("isBreak", false);
            animator.SetBool("isMove", false);
            animator.SetBool("isRun", true);
        }

        if (rb.velocity.y > 0)
        {
            animator.SetBool("isFall", false);
            animator.SetBool("isJump", true);
        }
        if (rb.velocity.y == 0)
        {
            animator.SetBool("isFall", false);
            animator.SetBool("isJump", false);
        }
        if (rb.velocity.y < 0)
        {
            animator.SetBool("isFall", true);
        }
    }
    
    public void EnterLeftWall()  
    {
        // 레이의 시작점
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector2 bsize = new Vector2(bounds.size.x, bounds.size.y) - new Vector2(0f, 0.5f);
        RaycastHit2D rayHit = Physics2D.BoxCast(bounds.center, bsize, 0f, Vector2.left, 0f, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null)           
        {
            recentSpeed = 0;
            isLeftMove = false;
        }
      
    }

    public void EnterRightWall() 
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector2 bsize = new Vector2(bounds.size.x, bounds.size.y) - new Vector2(0f, 0.5f);
        RaycastHit2D rayHit = Physics2D.BoxCast(bounds.center, bsize, 0f, Vector2.right, 0f, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null) 
        {
            recentSpeed = 0;
            isRightMove = false;
        }

    }

    public void MoveLeft()                          ////// 왼쪽 이동 함수    
    {
        EnterLeftWall();
        if (recentSpeed > 0 && !Sprite.flipX)       //// 움직임이 있고 오른쪽을 보고 있다면
        {
            StartCoroutine(DecreaseSpeed(deceleration));
        }
        if (recentSpeed >= checkRun && !Sprite.flipX)       //// checkRun 속도보다 빠르고 오른쪽을 보고 있다면
        {
            animator.SetBool("isBreak", true);
            StartCoroutine(DecreaseSpeed(turnBreak));
        }
        if (recentSpeed == 0)                          //// 현재 속도 == 0 이면
        {
            animator.SetBool("isBreak", false);
            Sprite.flipX = true;                    // 스프라이트가 왼쪽 바라보게 함
            isRightMove = false;                    // 오른쪽 이동 불가능
            isLeftMove = true;                      // 왼쪽 이동 가능
        }
        if (isLeftMove)                             //// 왼쪽 이동이 가능하다면
        {
            StartCoroutine(IncreaseSpeed(acceleration));        // IncreaseSpeed() 함수 실행. 천천히 가속
        }
    }

    public void MoveRight()                         ////// 오른쪽 이동 함수
    {
        EnterRightWall(); 
        if (recentSpeed > 0 && Sprite.flipX)       //// 움직임이 있고 오른쪽을 보고 있다면
        {
            StartCoroutine(DecreaseSpeed(deceleration));
        }
        if (recentSpeed >= checkRun && Sprite.flipX)       //// checkRun 속도보다 빠르고 왼쪽을 보고 있다면
        {
            animator.SetBool("isBreak", true);
            StartCoroutine(DecreaseSpeed(turnBreak));
        }
        if (recentSpeed == 0)                       //// 현재 속도 == 0 이면
        {
            animator.SetBool("isBreak", false);
            Sprite.flipX = false;                   // 스프라이트가 왼쪽 바라보게 함
            isLeftMove = false;                     // 왼쪽 이동 불가능
            isRightMove = true;                     // 오른쪽 이동 가능
        }
        if (isRightMove)                            //// 오른쪽 이동이 가능하다면
        {
            StartCoroutine(IncreaseSpeed(acceleration));        // IncreaseSpeed() 함수 실행. 천천히 가속
        }
    }

    public void KeyDownX()
    {
        MaxSpeed = RunSpeed;
        acceleration = runacceleration;
    }
    public void KeyUpX()
    {
        MaxSpeed = MaxSpeed2;
        acceleration = acceleration2;
    }
    public void Idle()
    {
        StartCoroutine(DecreaseSpeed(deceleration));
    }


    IEnumerator IncreaseSpeed(float speed)
    {
        if (recentSpeed < MaxSpeed) recentSpeed += speed * Time.deltaTime;
        yield return null;
    }

    IEnumerator DecreaseSpeed(float speed)
    {
        if (recentSpeed > 0) recentSpeed -= deceleration * Time.deltaTime;
        if (recentSpeed < 0) recentSpeed = 0;
        yield return null;
    }
    IEnumerator TurnBreakSpeed()
    {
        if (recentSpeed > 0) recentSpeed -= turnBreak * Time.deltaTime;
        if (recentSpeed < 0) recentSpeed = 0;
        yield return null;
    }


    //// 나중에 컴포넌트 분리 해도 됨 ////
    public void HasDamaged()
    {
        if(life >=1) 
        {
            lifeIMG[life - 1].SetActive(false);

            if (life == 1)
            {
                Debug.Log("마이가 쓰러졌습니다.");
                GameOver();
            }
        }
        else
        {
            //라이프가 비정상적
            //0 또는 음수
        }

        life--;
        StartCoroutine(BlinkEffect());
    }
    public void GameOver()
    {
        //나중에 다른 오브젝트에 메세지 던지는 방식으로 수정하기
        acceleration = 0;
        Destroy(GetComponent<Jump>());
    }

    //깜빡거리는 효과
    IEnumerator BlinkEffect()
    {
        IsBlinkEffectRunning = true; // 블링크 이펙트가 실행 중임을 표시

        int count = 0;

        while (count < 2)
        {
            float fadeCount = 1.0f;

            while (fadeCount > 0.0f)
            {
                fadeCount -= 0.1f;
                yield return new WaitForSeconds(0.01f);
                Sprite.color = new Color(1, 1, 1, fadeCount);
            }
            while (fadeCount < 1.0f)
            {
                fadeCount += 0.1f;
                yield return new WaitForSeconds(0.01f);
                Sprite.color = new Color(1, 1, 1, fadeCount);
            }

            count++;
        }

        IsBlinkEffectRunning = false; // 블링크 이펙트가 끝났음을 표시
    }
}
