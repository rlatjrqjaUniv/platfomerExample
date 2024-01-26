using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // ���� ������ ���θ� ��Ÿ���� ����
    public bool isJumping = false;

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

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0) // �÷��̾ �������� �� == velocity.y�� ����
        {
            Debug.DrawRay(gameObject.transform.position, Vector2.down, new Color(0, 1, 0)); //ray�� �׸���

            RaycastHit2D rayHit = Physics2D.Raycast(gameObject.transform.position, Vector2.down,1, LayerMask.GetMask("Platform")); //ray ���
            if (rayHit.collider != null)
            { // RayCastHit ������ �ݶ��̴��� �˻� Ȯ�� ����
                //Debug.Log(rayHit.collider.ToString());
                if (rayHit.distance < 0.5f)
                { // ray�� 0.5 �̻� ���� ��
                    //anim.SetBool("isJump", false); // �ִϸ��̼� �ǵ�����
                }
                isJumping = false;
            }
        }
    }


    // �浹�� �߻����� �� ȣ��Ǵ� �޼���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ���� �ƴ����� ���� ����
        //isJumping = false;
    }
}