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

        if (!Input.anyKey)
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

        if (isLeftPressed)
        {
            LeftArrow.Invoke();
        }
       
        if (isRightPressed)
        {
            RightArrow.Invoke();
        }

        
    }
}
