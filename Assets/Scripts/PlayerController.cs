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
    public float TimeToMaxSpeed; //�̰� ���� �� ���� �׷��°��� ���� ������ ���ϴ� �� ����

    //���� ������ ������ �ϴ� ���� �ٸ� ��ũ��Ʈ�� �ۼ����� �Ƹ� ���߿� ����...�� ���� �ְ�? �׳� �ֵ� �ǰ�
    //public float jumpPower;

    private Rigidbody2D rb;
    private SpriteRenderer Sprite;

    [SerializeField] private float recentSpeed;
    private bool isLeftMove = false;
    private bool isRightMove = false;
    //private bool isJumping = false;

    private void Start()
    {
        // Rigidbody2D ������Ʈ ��������
        rb = GetComponent<Rigidbody2D>();
        // flipx �Ϸ��� ��������Ʈ ��������
        Sprite = GetComponent<SpriteRenderer>();
    }

    // Time.deltaTime�� ��� �������� �߰�����
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

 
    // �̰͵� �����ϸ� �� �� ������
    // ���۸�����1 �����ؼ� �ϴٺ���..! �������� �ڵ�
    // �ӵ��� 0���� ũ�鼭 �������� �����ִٸ� �ӵ��� 0���� ���ӽ�Ű��
    // Sptite.flipx ���Ŀ� �������� ����
    public void MoveLeft()
    {        
        if(recentSpeed>0&& !Sprite.flipX)
        {
            TurnBreak(); //DecreaseSpeed �� �Ȱ����� ���� ������ �ٸ�
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
