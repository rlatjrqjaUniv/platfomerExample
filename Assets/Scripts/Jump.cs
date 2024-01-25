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

    void Update()
    {
        // ���� ���� ���� �ƴϰ�, ���� ��ư�� ������ ���
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            // ���� ������ ���� ����
            isJumping = true;

            // Rigidbody2D�� �̿��Ͽ� �������� ����
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
        }
    }

    // �浹�� �߻����� �� ȣ��Ǵ� �޼���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ���� �ƴ����� ���� ����
        isJumping = false;
    }
}
