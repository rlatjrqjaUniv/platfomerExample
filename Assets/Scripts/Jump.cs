using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // ���� ������ ���θ� ��Ÿ���� ����
    private bool isJumping = false;

    // ���� �ӵ��� �����ϱ� ���� ����
    [Range(1, 30)]
    public float jumpVelocity;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // ���� ���� ���� �ƴϰ�, ���� ��ư�� ������ ���
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            // ���� ������ ���� ����
            isJumping = true;

            // Rigidbody2D�� �̿��Ͽ� �������� ����
            rb.velocity = Vector2.up * jumpVelocity;
        }
    }

  
    // �浹�� �߻����� �� ȣ��Ǵ� �޼���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ���� �ƴ����� ���� ����
        isJumping = false;
    }
}




// ����ĳ��Ʈ ������ ����...
/*  private void FixedUpdate()
    {
        if (rb.velocity.y < 0) // �÷��̾ �������� �� == velocity.y�� ����
        {
            Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0)); //ray�� �׸���
            RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Platform")); //ray ���
            if (rayHit.collider != null)
            { // RayCastHit ������ �ݶ��̴��� �˻� Ȯ�� ����
                if (rayHit.distance < 0.5f)
                { // ray�� 0.5 �̻� ���� ��
                    anim.SetBool("isJump", false); // �ִϸ��̼� �ǵ�����
                }
isJumping = false;
            }
        }
    }
 */