using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump : MonoBehaviour
{

    //// ���� ���� ////
    private bool isJumping = false;                              // ���� ������ ���θ� ��Ÿ���� bool
    private bool canFly = false;                                 // ���� ���� ���� ����
    private float flyTime;                                       // ü�� �ð�
    
    public float maxFlyTime; // ü�� �ð� ����

    public float checkRunSpeed;                                  // �޸��鼭 ���� ������ �� �� �ִ� �ӵ��� ����
    
    [Range(1, 20)] public float hightJump;                       // ������ �ִ� ���� �⺻��
    [Range(1, 25)] public float runHightJump;                    // �޸��鼭 ������ �ִ� ���̸� ����
    
    public float currentHightJump;                               // ���� ������ �ִ� ����

    public float fallSpeed = 2.5f;                               // �ϰ� ���� �� Ű ���� �Ʒ��� �������� �ӵ� ���� ����
    public float lowJumpSpeed = 2f;                              // ��� ���� �� Ű ���� ��� �ӵ��� ���ҽ�Ű�� ����

    //// ������Ʈ ////
    Rigidbody2D rb;
    PlayerController pc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();                        // Rigidbody2D ������Ʈ ��������
        pc = GetComponent<PlayerController>();                   // PlayerController ������Ʈ ��������
    }
    void Update()
    {
        if (!isJumping && Input.GetButtonDown("Jump"))           //// ���� ���� ���� �ƴϰ�, ���� ��ư�� ������ ���
        {
            isJumping = true;                                    // ���� ������ ���� ����
            rb.velocity = Vector2.up * currentHightJump;         // Rigidbody2D�� �̿��Ͽ� �������� ����
            canFly = true;                                       // ���� ���� ���� ���·� ����
            flyTime = maxFlyTime;
        }
        else if (canFly && Input.GetButtonDown("Jump"))          //// ���� ���� ������ �����ϰ�, ���� ��ư�� ������ ���
        {
            canFly = false;                                      // ���� ���� ���Ŀ��� �ٽ� ���� ������ �� �� ������ ����
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // ��ġ ���� (����) �� ������������ �״�� ��������
        }
        else if(!canFly && Input.GetButton("Jump"))              //// ���� ���� ��ư�� �� ������ ���� ���
        {
            flyTime -= Time.deltaTime;                           // �ö��� �ð��� �� �� ������ ����
            if (flyTime <= 0)                                    
            {
                rb.velocity = new Vector2(0,-0.000001f);
                rb.constraints = RigidbodyConstraints2D.None;    // ��ġ ���� ����, �����̼��� �ٽ� ����
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else if (Input.GetButtonUp("Jump"))                      //// ���� ���� ��ư�� ���� ��
        {
            rb.constraints = RigidbodyConstraints2D.None;        // ��ġ ���� ����, �����̼��� �ٽ� ����
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }


        if (rb.velocity.y < 0)                                   //// ���� ���� ���� �ӵ��� �Ʒ��� ���ϰ� �ִٸ�
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;   // �߷¿� fallSpeed ����
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))  //// ���� ���� ���� �ӵ��� ���� ���ϰ� �ְ�, ���� ��ư�� ������ �ʾҴٸ�
        { 
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpSpeed - 1) * Time.deltaTime;  // �߷¿� lowJumpSpeed ����
        }

    }

    private void FixedUpdate()                                   // ���� ������Ʈ �ֱ⿡ ���� ȣ��ǹǷ� ���
    {
        currentHightJump = hightJump;                            // �⺻ �ִ� ���̷� ����

        if (pc.recentSpeed >= checkRunSpeed)                     //// ���� �޸��� �ӵ��� üũ �� ���ǵ� ���� ũ�ų� ������
        {
            currentHightJump = runHightJump;                     // �޸��鼭 ������ �ִ� ���̸� ����
        }
        else                                                     //// ���� �޸��� �ӵ��� üũ �� ���ǵ� ���� �۴ٸ�
        {
            currentHightJump = hightJump;                        // ������ �ִ� ���� �⺻�� ����
        }

        if (rb.velocity.y < 0)                                   // �÷��̾ �������� ��
        {

            Bounds bounds = GetComponent<Collider2D>().bounds;   // �ݶ��̴��� ��踦 ���� (���� �����ص� �Ǳ� ��)
            Vector2 bsize = new Vector2(bounds.size.x, bounds.size.y) - new Vector2(0.1f, 0f);

            RaycastHit2D rayHit = Physics2D.BoxCast(bounds.center, bsize, 0f, Vector2.down, 0.15f, LayerMask.GetMask("Platform"));
                                                                 // �Ʒ� �������� 1��ŭ ray ����, Platform ���� Ȯ��
            Vector2 start = new Vector2(bounds.min.x, bounds.min.y); // Boxcast�� ������ ���
            Vector2 end = new Vector2(bounds.max.x, bounds.min.y);   // Boxcast�� ���� ���
            
            Debug.DrawRay(start, Vector2.down * 0.15f, Color.red);
            Debug.DrawRay(end, Vector2.down * 0.15f, Color.red);
            if (rayHit.collider != null)                         //// Platform�� �浹�ߴٸ�
            { 
                isJumping = false;                               // ���� ���� �ƴ�
                if (rb.velocity.y < -25&&!pc.IsBlinkEffectRunning)                         // �������� �ӵ��� -25���� �۾Ҵٸ� (���� �������ٸ�)
                {
                        pc.HasDamaged();                             // PlayerController.cs�� HasDamaged �Լ� ����
                }
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else                                                 // Platform�� �浹���� �ʾҴٸ�
            {
                isJumping=true;                                  // ���� ��
            }
        }
    }


}