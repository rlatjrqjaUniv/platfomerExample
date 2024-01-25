using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Idling, Moving, Jumping, Falling }
    [SerializeField] State state;

    public float defaultMoveSpeed;
    public float acceleration;
    public float deceleration;
    public float turnBreak;
    public float MaxSpeed;
    public float TimeToMaxSpeed; //이거 내가 잘 몰라서 그러는건지 몬지 동작을 안하는 거 같음

    //점프 높낮이 구현을 하다 보니 다른 스크립트에 작성했음 아마 나중에 합쳐...질 수도 있고? 그냥 둬도 되고
    //public float jumpPower;

    private Rigidbody2D rb;
    private SpriteRenderer Sprite;

    [SerializeField] private float recentSpeed;
    private bool isLeftMove = false;
    private bool isRightMove = false;
    //private bool isJumping = false;

    private void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        // flipx 하려고 스프라이트 가져오기
        Sprite = GetComponent<SpriteRenderer>();
    }

    // Time.deltaTime이 없어서 여기저기 추가했음
    private void Update()
    {
        if (isLeftMove)
        {
            transform.position = new Vector3(transform.position.x - (recentSpeed * 5f * Time.deltaTime),
            transform.position.y, transform.position.z);
        }
        else if (isRightMove)
        {
            transform.position = new Vector3(transform.position.x + (recentSpeed * 5f * Time.deltaTime),
            transform.position.y, transform.position.z);
        }
    }

 
    // 이것도 정리하면 할 수 있을듯
    // 슈퍼마리오1 참고해서 하다보니..! 어지러운 코드
    // 속도가 0보다 크면서 오른쪽을 보고있다면 속도를 0까지 감속시키고
    // Sptite.flipx 이후에 왼쪽으로 가속
    public void MoveLeft()
    {        
        if(recentSpeed>0&& !Sprite.flipX)
        {
            TurnBreak(); //DecreaseSpeed 랑 똑같은데 감속 변수가 다름
        }
        if(recentSpeed==0)
        {
            Sprite.flipX = true;
            isRightMove = false;
            isLeftMove = true;
        }
        if (isLeftMove)
        {
            StartCoroutine(IncreaseSpeed());
        }
    }

    public void MoveRight()
    {
        if (recentSpeed > 0 && Sprite.flipX)
        {
            TurnBreak();
        }
        if (recentSpeed == 0)
        {
            Sprite.flipX = false;
            isRightMove = true;
            isLeftMove = false;
        }
        if (isRightMove)
        {
            StartCoroutine(IncreaseSpeed());
        }
    }

   /* public void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpPower * 150);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }
   */
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
}
