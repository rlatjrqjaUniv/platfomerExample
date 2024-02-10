using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent Idle;
    public UnityEvent LeftArrow;
    public UnityEvent RightArrow;

    void Update()
    {
        bool isLeftPressed = Input.GetKey(KeyCode.LeftArrow);                         // 왼쪽 화살표 키를 누르고 있는지 확인하는 bool
        bool isRightPressed = Input.GetKey(KeyCode.RightArrow);                       // 오른쪽 화살표 키를 누르고 있는지 확인하는 bool

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))    //// 왼·오른쪽 화살표 키를 누르고 있지 않다면
        {
            Idle.Invoke();                                                            // Idle 함수 호출 (감속하는 함수)
        }

        if (Input.GetKey(KeyCode.LeftArrow)&&Input.GetKey(KeyCode.RightArrow))        //// 왼·오른쪽 화살표 키를 동시에 누르고 있다면
        {
            Idle.Invoke();                                                            // Idle 함수 호출 (감속하는 함수)
            isLeftPressed = false;                                                    // 왼쪽 bool = false
            isRightPressed = false;                                                   // 오츤쪽 bool = false
        }

        if (isLeftPressed)                                                            //// 왼쪽 bool == true 라면
        {
            LeftArrow.Invoke();                                                       // LeftArrow 함수 호출 (가속하는 함수)
        }

        if (isRightPressed)                                                           //// 오른쪽 bool == true 라면
        {
            RightArrow.Invoke();                                                      // RightArrow 함수 호출 (가속하는 함수)
        }

        
    }
}
