using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Idling, Moving, Jumping, Falling, Break }
    [SerializeField] State state;

    //// ������Ʈ ////
    private Rigidbody2D rb;
    private SpriteRenderer Sprite;
    private Animator animator;
    private LayerMask layerMask;

    //// �̵� ////
    public float acceleration;                      // ���ӵ�
    public float deceleration;                      // ���ӵ�
    public float turnBreak;                         // ������ȯ ���ӵ�
    public float MaxSpeed;                          // �ִ� �ӵ�
    [SerializeField] private float recentSpeed;     // ���� �ӵ�
    private bool isLeftMove = false;                // �������� bool
    private bool isRightMove = false;               // ���������� bool
    //public float defaultMoveSpeed
    //public float TimeToMaxSpeed

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();            // Rigidbody2D ������Ʈ ��������
        Sprite = GetComponent<SpriteRenderer>();     // SpriteRenderer ������Ʈ ��������
        animator = GetComponent<Animator>();         // Animator ������Ʈ ��������
    }

    private void Update()
    {
        if (isLeftMove)
        {
            transform.position = new Vector3(transform.position.x - (recentSpeed * 5f * Time.deltaTime),  // Time.deltaTime ����. ���� ������ - ���� �ӵ� *5 ��ŭ �̵�
            transform.position.y, transform.position.z);
        }
        else if (isRightMove)
        {
            transform.position = new Vector3(transform.position.x + (recentSpeed * 5f * Time.deltaTime),  // Time.deltaTime ����. ���� ������ + ���� �ӵ� *5 ��ŭ �̵�
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
    public void MoveLeft()                           ////// ���� �̵� �Լ�    
    {        
        if(recentSpeed>0&& !Sprite.flipX)            //// �������� �ְ� �������� ���� �ִٸ�
        {
            animator.SetBool("isBreak", true);
            TurnBreak();                             // TurnBreak() �Լ� ����. �ް��� �ӵ� ����
        }
        if(recentSpeed==0)                           //// ���� �ӵ� == 0 �̸�
        {
            animator.SetBool("isBreak", false);
            Sprite.flipX = true;                     // ��������Ʈ�� ���� �ٶ󺸰� ��
            isRightMove = false;                     // ������ �̵� �Ұ���
            isLeftMove = true;                       // ���� �̵� ����
        }
        if (isLeftMove)                              //// ���� �̵��� �����ϴٸ�
        {
            StartCoroutine(IncreaseSpeed());         // IncreaseSpeed() �Լ� ����. õõ�� ����
        }
    }

    public void MoveRight()                          ////// ������ �̵� �Լ�
    {
        if (recentSpeed > 0 && Sprite.flipX)         //// �������� �ְ� ������ �����ִٸ�
        {
            animator.SetBool("isBreak", true);
            TurnBreak();                             // TurnBreak() �Լ� ����. �ް��� �ӵ� ����
        }
        if (recentSpeed == 0)                        //// ���� �ӵ� == 0 �̸�
        {
            animator.SetBool("isBreak", false);
            Sprite.flipX = false;                    // ��������Ʈ�� ���� �ٶ󺸰� ��
            isLeftMove = false;                      // ���� �̵� �Ұ���
            isRightMove = true;                      // ������ �̵� ����
        }
        if (isRightMove)                             //// ������ �̵��� �����ϴٸ�
        {
            StartCoroutine(IncreaseSpeed());         // IncreaseSpeed() �Լ� ����. õõ�� ����
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
}
