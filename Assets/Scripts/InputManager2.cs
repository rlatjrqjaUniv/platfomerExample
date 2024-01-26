using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager2 : MonoBehaviour
{
    // �ν����� â�� ��Ÿ���� �κ�, ��ũ��Ʈ �� ������Ʈ ����� �ְ� �Լ� �����ϸ� ��
    public UnityEvent Idle;
    //public UnityEvent Jump;
    public UnityEvent LeftArrow;
    public UnityEvent RightArrow;

    void Update()
    {
        // ���� Ű �Է� ������ bool ���
        bool isLeftPressed = Input.GetKey(KeyCode.LeftArrow);
        bool isRightPressed = Input.GetKey(KeyCode.RightArrow);

        // ���� ���� ���¿��� �ƹ�Ű�� ������ ��,�� ���� ������ �ʴ´�
        // �� �� ������ ���� �ʴٸ����� �ٲ�
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            Idle.Invoke();
        }

        // ���ÿ� ������ ���߱� (ĳ���� state ��ȹ�� ����)
        if (Input.GetKey(KeyCode.LeftArrow)&&Input.GetKey(KeyCode.RightArrow))
        {
            Idle.Invoke();
            isLeftPressed = false;
            isRightPressed = false;
        }

        // Input.GetKey(KeyCode.LeftArrow);
        if (isLeftPressed)
        {
            LeftArrow.Invoke();
        }

        // Input.GetKey(KeyCode.RightArrow);
        if (isRightPressed)
        {
            RightArrow.Invoke();
        }

        
    }
}
