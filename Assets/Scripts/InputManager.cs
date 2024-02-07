using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent Idle;
    public UnityEvent LeftArrow;
    public UnityEvent RightArrow;
    public UnityEvent KeyDownX;
    public UnityEvent KeyUpX;

    void Update()
    {
        bool isLeftPressed = Input.GetKey(KeyCode.LeftArrow);                         // ���� ȭ��ǥ Ű�� ������ �ִ��� Ȯ���ϴ� bool
        bool isRightPressed = Input.GetKey(KeyCode.RightArrow);                       // ������ ȭ��ǥ Ű�� ������ �ִ��� Ȯ���ϴ� bool

        if (Input.GetKeyDown(KeyCode.X))
        {
            KeyDownX.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            KeyUpX.Invoke();
        }
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))    //// �ޡ������� ȭ��ǥ Ű�� ������ ���� �ʴٸ�
        {
            Idle.Invoke();                                                            // Idle �Լ� ȣ�� (�����ϴ� �Լ�)
        }

        if (Input.GetKey(KeyCode.LeftArrow)&&Input.GetKey(KeyCode.RightArrow))        //// �ޡ������� ȭ��ǥ Ű�� ���ÿ� ������ �ִٸ�
        {
            Idle.Invoke();                                                            // Idle �Լ� ȣ�� (�����ϴ� �Լ�)
            isLeftPressed = false;                                                    // ���� bool = false
            isRightPressed = false;                                                   // ������ bool = false
        }

        if (isLeftPressed)                                                            //// ���� bool == true ���
        {
            LeftArrow.Invoke();                                                       // LeftArrow �Լ� ȣ�� (�����ϴ� �Լ�)
        }

        if (isRightPressed)                                                           //// ������ bool == true ���
        {
            RightArrow.Invoke();                                                      // RightArrow �Լ� ȣ�� (�����ϴ� �Լ�)
        }

     
        
    }
}
