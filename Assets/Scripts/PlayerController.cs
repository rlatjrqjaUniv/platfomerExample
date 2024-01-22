using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Idling,Moving,Jumping,Falling }
    [SerializeField] State state;
    
    public float moveCooltime; // �̵� Ű�� �������� �Ҽ��� ������ ���� ���Ұ���(Ƚ��), (moveSpedd * moveColltime = �������� ���̶�� ���� ��)
    public float moveSpeed; // move Cooltime 1ȸ�� ��ŭ�� ���� ���Ұ�����(��)
    // moveCooltime �� ���� moveSpeed�� ������ ���� �� �����ȴٸ� ���ӵ��� ���� ����
    // moveCooltime �� ���� moveSpeed�� ������ ���� �����ִٸ� ���ӵ��� �ٰ� ��.
    // ex) moveCooltime 0.1��, moveSpeed 100 �̶�� 0.1�ʵ��� 100�� ���� �� �����Ǿ ���ӵ�X
    // ex) moveCooltime 0.1��, moveSpeed 110 �̶�� 0.1�ʵ��� 110�� ���� �� �����Ǳ� ���� 0.1�ʰ� ������, 110�� ���� �� �߰��Ǳ� ������ ���ӵ�O
    
    public float MaxSpeed; //Rigidbody�� info ���� velocity ���� �ʹ� ���ӵ��� �ʰ� �߰��� ����
    public float jumpPower;

    public Rigidbody2D rb;
    private bool isMoving = false;
    private bool isJuming = false;

    public event EventHandler OnKeyPressed;

    private void Start()
    {
        // Rigidbody2D ������Ʈ ��������
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed);
    }

    public void MoveLeft()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine(Vector2.left));
        }
    }

    public void MoveRight()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine(Vector2.right));
        }
    }

    public void Jump()
    {
        if(!isJuming)
        {
            isJuming = true;
            rb.AddForce(Vector2.up * jumpPower * 100);
        }
    }

    IEnumerator MoveCoroutine(Vector2 direction)
    {
        isMoving = true;
        rb.AddForce(direction * moveSpeed, ForceMode2D.Force);
        yield return new WaitForSeconds(moveCooltime); // ���� ������ ������ �ð�
        isMoving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJuming = false;
    }
}
