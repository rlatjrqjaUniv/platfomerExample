using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Idling, Moving, Jumping, Falling, Break }
    [SerializeField] State state;

    //// 컴포넌트 ////
    private Rigidbody2D rb;
    private SpriteRenderer Sprite;
    private Animator animator;
    private LayerMask layerMask;

    //// 이동 ////
    public float acceleration;                      // 가속도
    public float deceleration;                      // 감속도
    public float turnBreak;                         // 방향전환 감속도
    public float MaxSpeed;                          // 최대 속도
    private float recentSpeed;     // 현재 속도
    private bool isLeftMove = false;                // 왼쪽으로 bool
    private bool isRightMove = false;               // 오른쪽으로 bool
    //public float defaultMoveSpeed
    //public float TimeToMaxSpeed

    //// 캐릭터 정보 : 나중에 컴포넌트 분리 해도 됨 ////
    public GameObject[] lifeIMG;
    int life = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();            // Rigidbody2D 컴포넌트 가져오기
        Sprite = GetComponent<SpriteRenderer>();     // SpriteRenderer 컴포넌트 가져오기
        animator = GetComponent<Animator>();         // Animator 컴포넌트 가져오기
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
            animator.SetBool("isRun", false);
            animator.SetBool("isBreak", false);
        }
        if (recentSpeed > 0)
        {
            animator.SetBool("isRun", true);
        }

        if(rb.velocity.y > 0)
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
    public void MoveLeft()                           ////// 왼쪽 이동 함수    
    {        
        if(recentSpeed>0&& !Sprite.flipX)            //// 움직임이 있고 오른쪽을 보고 있다면
        {
            animator.SetBool("isBreak", true);
            TurnBreak();                             // TurnBreak() 함수 실행. 급격히 속도 감소
        }
        if(recentSpeed==0)                           //// 현재 속도 == 0 이면
        {
            animator.SetBool("isBreak", false);
            Sprite.flipX = true;                     // 스프라이트가 왼쪽 바라보게 함
            isRightMove = false;                     // 오른쪽 이동 불가능
            isLeftMove = true;                       // 왼쪽 이동 가능
        }
        if (isLeftMove)                              //// 왼쪽 이동이 가능하다면
        {
            StartCoroutine(IncreaseSpeed());         // IncreaseSpeed() 함수 실행. 천천히 가속
        }
    }

    public void MoveRight()                          ////// 오른쪽 이동 함수
    {
        if (recentSpeed > 0 && Sprite.flipX)         //// 움직임이 있고 왼쪽을 보고있다면
        {
            animator.SetBool("isBreak", true);
            TurnBreak();                             // TurnBreak() 함수 실행. 급격히 속도 감소
        }
        if (recentSpeed == 0)                        //// 현재 속도 == 0 이면
        {
            animator.SetBool("isBreak", false);
            Sprite.flipX = false;                    // 스프라이트가 왼쪽 바라보게 함
            isLeftMove = false;                      // 왼쪽 이동 불가능
            isRightMove = true;                      // 오른쪽 이동 가능
        }
        if (isRightMove)                             //// 오른쪽 이동이 가능하다면
        {
            StartCoroutine(IncreaseSpeed());         // IncreaseSpeed() 함수 실행. 천천히 가속
        }
    }

    
    public void Idle()
    {
        StartCoroutine(DecreaseSpeed());
    }
    public void TurnBreak()
    {
        StartCoroutine(TurnBreakSpeed());
    }

    IEnumerator IncreaseSpeed()
    {
        if (recentSpeed < MaxSpeed) recentSpeed += acceleration * Time.deltaTime;
        yield return 0;// new WaitForSeconds(0.01f * TimeToMaxSpeed);
    }

    IEnumerator DecreaseSpeed()
    {
        if (recentSpeed > 0) recentSpeed -= deceleration * Time.deltaTime;
        if (recentSpeed < 0) recentSpeed = 0;
        yield return 0;// new WaitForSeconds(0.01f * TimeToMaxSpeed);
    }
    IEnumerator TurnBreakSpeed()
    {
        if (recentSpeed > 0) recentSpeed -= turnBreak * Time.deltaTime;
        if (recentSpeed < 0) recentSpeed = 0;
        yield return 0;
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
        Destroy(GetComponent<JumpSetting>());
    }

    //깜빡거리는 효과
    IEnumerator BlinkEffect()
    {
        int count = 0;

        while(count < 2)
        {
            float fadeCount = 1.0f;

            while (fadeCount > 0.0f)
            {
                fadeCount -= 0.1f;
                yield return new WaitForSeconds(0.01f);
                Sprite.color = new Color(1, 1, 1, fadeCount);
            }
            while (fadeCount<1.0f)
            {
                fadeCount += 0.1f;
                yield return new WaitForSeconds(0.01f);
                Sprite.color = new Color(1, 1, 1, fadeCount);
            }
            
            count++;
        }
    }
}
