using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //enum State { Idling, Moving, Jumping, Falling, Break }
    //[SerializeField] State state;

    //// ĳ���� ���� : ���߿� ������Ʈ �и� �ص� �� ////
    public GameObject[] lifeIMG;
    int life = 3;

    //// ������Ʈ ////
    private Rigidbody2D rb;
    private SpriteRenderer Sprite;
    private Animator animator;
    private LayerMask layerMask;

    //// �̵� ////
    public float acceleration;                      // ���ӵ�
    public float deceleration;                      // ���ӵ�
    public float checkRun;                          // �޸��� �ӵ��� ����
    public float turnBreak;                         // ������ȯ ���ӵ�
    public float MaxSpeed;                          // �ִ� �ӵ�
    public float recentSpeed;                       // ���� �ӵ�
    private bool isLeftMove = false;                // �������� bool
    private bool isRightMove = false;               // ���������� bool
 

  

    void Start()
    {
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
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    public void EnterLeftWall()  ////////////////////// ����� �ߴµ�......... ���ݸ����� ���� �� ���� ������????????
    {//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ������ ������
        Vector2 startPoint1 = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Vector2 startPoint2 = new Vector2(transform.position.x, transform.position.y - 0.5f);

        // ���̸� �׸� ����
        Vector2 endPoint1 = startPoint1 + Vector2.left * 0.5f; // maxDistance�� ���ϴ� �Ÿ��Դϴ�.
        Vector2 endPoint2 = startPoint2 + Vector2.left * 0.5f; // maxDistance�� ���ϴ� �Ÿ��Դϴ�.

        // Debug.DrawLine �Լ��� ����Ͽ� ���̸� �׸��ϴ�.
        Debug.DrawLine(startPoint1, endPoint1, new Color(0, 1, 0));
        Debug.DrawLine(startPoint2, endPoint2, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), 0f, Vector2.left, 0.25f, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)           
        {
            recentSpeed = 0;
            isLeftMove = false;
        }
      
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void EnterRightWall() ////////////////////// ����� �ߴµ�......... ���ݸ����� ���� �� ���� ������????????
    {/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ������ ������
        Vector2 startPoint1 = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Vector2 startPoint2 = new Vector2(transform.position.x, transform.position.y - 0.5f);

        // ���̸� �׸� ����
        Vector2 endPoint1 = startPoint1 + Vector2.right * 0.5f; // maxDistance�� ���ϴ� �Ÿ��Դϴ�.
        Vector2 endPoint2 = startPoint2 + Vector2.right * 0.5f; // maxDistance�� ���ϴ� �Ÿ��Դϴ�.

        // Debug.DrawLine �Լ��� ����Ͽ� ���̸� �׸��ϴ�.
        Debug.DrawLine(startPoint1, endPoint1, new Color(0, 1, 0));
        Debug.DrawLine(startPoint2, endPoint2, new Color(0, 1, 0));

      
        RaycastHit2D rayHit= Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), 0f, Vector2.right, 0.25f, LayerMask.GetMask("Platform"));

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
            Idle();                                 // Idle() �Լ� ����. õõ�� �ӵ� ����
        }
        if (recentSpeed >= checkRun && !Sprite.flipX)       //// checkRun �ӵ����� ������ �������� ���� �ִٸ�
        {
            animator.SetBool("isBreak", true);
            TurnBreak();                            // TurnBreak() �Լ� ����. �ް��� �ӵ� ����
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
            StartCoroutine(IncreaseSpeed());        // IncreaseSpeed() �Լ� ����. õõ�� ����
        }
    }

    public void MoveRight()                         ////// ������ �̵� �Լ�
    {
        EnterRightWall(); 
        if (recentSpeed > 0 && Sprite.flipX)       //// �������� �ְ� �������� ���� �ִٸ�
        {
            Idle();                                 // Idle() �Լ� ����. õõ�� �ӵ� ����
        }
        if (recentSpeed >= checkRun && Sprite.flipX)       //// checkRun �ӵ����� ������ ������ ���� �ִٸ�
        {
            animator.SetBool("isBreak", true);
            TurnBreak();                            // TurnBreak() �Լ� ����. �ް��� �ӵ� ����
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
            StartCoroutine(IncreaseSpeed());        // IncreaseSpeed() �Լ� ����. õõ�� ����
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
        yield return null;
    }

    IEnumerator DecreaseSpeed()
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
