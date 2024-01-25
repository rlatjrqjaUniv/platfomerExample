using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSetting : MonoBehaviour
{
    // �ϰ� ���� �� Ű ���� �Ʒ��� �������� �ӵ� ���� ����
    public float fallMultiplier = 2.5f;

    // ��� ���� �� Ű ���� ��� �ӵ��� ���ҽ�Ű�� ����
    public float lowJumpMultiplier = 2f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���� ���� ���� �ӵ��� �Ʒ��� ���ϰ� �ִٸ�
        if (rb.velocity.y < 0)
        {
            // �߷¿� fallMultiplier ����
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // ���� ���� ���� �ӵ��� ���� ���ϰ� �ְ�, ���� ��ư�� ������ �ʾҴٸ�
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // �߷��� �̿��Ͽ� �� ������ ��� ���� ���� ����
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
