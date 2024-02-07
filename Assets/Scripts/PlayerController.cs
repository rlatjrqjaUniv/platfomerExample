using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //enum State { Idling, Moving, Jumping, Falling, Break }
    //[SerializeField] State state;

    //// ĳ���� ���� : ���߿� ������Ʈ �и� �ص� �� ////
    public GameObject[] lifeIMG;
    int life = 3;
    public bool IsBlinkEffectRunning = false;

    //// ������Ʈ ////
    private Rigidbody2D rb;
    private SpriteRenderer Sprite;
    private Animator animator;
    private LayerMask layerMask;

    //// �̵� ////
    public float acceleration;                      // ���ӵ�
    public float runacceleration;                   // �޸��� ���ӵ�
    public float deceleration;                      // ���ӵ�
    public float turnBreak;                         // ������ȯ ���ӵ�
    public float checkRun;                          // �޸��� �ӵ��� ����
    public float MaxSpeed;                          // �ִ� �ӵ�
    public float recentSpeed;                       // ���� �ӵ�
    private bool isLeftMove = false;                // �������� bool
    private bool isRightMove = false;               // ���������� bool

    // �޸��� ��ư //
    private float MaxSpeed2;                        // ����,��ȯ�� ����
    private float acceleration2;                    // ����,��ȯ�� ����
    public float RunSpeed;
    // ���� �ִ� runacceleration

    void Start()
    {
        MaxSpeed2 = MaxSpeed;
        acceleration2 = acceleration;
        rb = GetComponent<Rigidbody2D>();           // Rigidbody2D ������Ʈ ��������
        Sprite = GetComponent<SpriteRenderer>();    // SpriteRenderer ������Ʈ ��������
        animator = GetComponent<Animator>();        // Animator ������Ʈ ��������
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
        // ������ ������
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

    public void MoveLeft()                          ////// ���� �̵� �Լ�    
    {
        EnterLeftWall();
        if (recentSpeed > 0 && !Sprite.flipX)       //// �������� �ְ� �������� ���� �ִٸ�
        {
            StartCoroutine(DecreaseSpeed(deceleration));
        }
        if (recentSpeed >= checkRun && !Sprite.flipX)       //// checkRun �ӵ����� ������ �������� ���� �ִٸ�
        {
            animator.SetBool("isBreak", true);
            StartCoroutine(DecreaseSpeed(turnBreak));
        }
        if (recentSpeed == 0)                          //// ���� �ӵ� == 0 �̸�
        {
            animator.SetBool("isBreak", false);
            Sprite.flipX = true;                    // ��������Ʈ�� ���� �ٶ󺸰� ��
            isRightMove = false;                    // ������ �̵� �Ұ���
            isLeftMove = true;                      // ���� �̵� ����
        }
        if (isLeftMove)                             //// ���� �̵��� �����ϴٸ�
        {
            StartCoroutine(IncreaseSpeed(acceleration));        // IncreaseSpeed() �Լ� ����. õõ�� ����
        }
    }

    public void MoveRight()                         ////// ������ �̵� �Լ�
    {
        EnterRightWall(); 
        if (recentSpeed > 0 && Sprite.flipX)       //// �������� �ְ� �������� ���� �ִٸ�
        {
            StartCoroutine(DecreaseSpeed(deceleration));
        }
        if (recentSpeed >= checkRun && Sprite.flipX)       //// checkRun �ӵ����� ������ ������ ���� �ִٸ�
        {
            animator.SetBool("isBreak", true);
            StartCoroutine(DecreaseSpeed(turnBreak));
        }
        if (recentSpeed == 0)                       //// ���� �ӵ� == 0 �̸�
        {
            animator.SetBool("isBreak", false);
            Sprite.flipX = false;                   // ��������Ʈ�� ���� �ٶ󺸰� ��
            isLeftMove = false;                     // ���� �̵� �Ұ���
            isRightMove = true;                     // ������ �̵� ����
        }
        if (isRightMove)                            //// ������ �̵��� �����ϴٸ�
        {
            StartCoroutine(IncreaseSpeed(acceleration));        // IncreaseSpeed() �Լ� ����. õõ�� ����
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


    //// ���߿� ������Ʈ �и� �ص� �� ////
    public void HasDamaged()
    {
        if(life >=1) 
        {
            lifeIMG[life - 1].SetActive(false);

            if (life == 1)
            {
                Debug.Log("���̰� ���������ϴ�.");
                GameOver();
            }
        }
        else
        {
            //�������� ��������
            //0 �Ǵ� ����
        }

        life--;
        StartCoroutine(BlinkEffect());
    }
    public void GameOver()
    {
        //���߿� �ٸ� ������Ʈ�� �޼��� ������ ������� �����ϱ�
        acceleration = 0;
        Destroy(GetComponent<Jump>());
    }

    //�����Ÿ��� ȿ��
    IEnumerator BlinkEffect()
    {
        IsBlinkEffectRunning = true; // ��ũ ����Ʈ�� ���� ������ ǥ��

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

        IsBlinkEffectRunning = false; // ��ũ ����Ʈ�� �������� ǥ��
    }
}
