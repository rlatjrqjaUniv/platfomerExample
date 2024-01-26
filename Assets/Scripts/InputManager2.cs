using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager2 : MonoBehaviour
{
    // 인스펙터 창에 나타내는 부분, 스크립트 들어간 오브젝트 끌어다 넣고 함수 선택하면 됨
    public UnityEvent Idle;
    //public UnityEvent Jump;
    public UnityEvent LeftArrow;
    public UnityEvent RightArrow;

    void Update()
    {
        // 동시 키 입력 때문에 bool 사용
        bool isLeftPressed = Input.GetKey(KeyCode.LeftArrow);
        bool isRightPressed = Input.GetKey(KeyCode.RightArrow);

        // 가속 붙은 상태에서 아무키를 누르고 왼,오 떼면 멈추질 않는다
        // 왼 오 누르고 있지 않다면으로 바꿈
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            Idle.Invoke();
        }

        // 동시에 누르면 멈추기 (캐릭터 state 기획서 참고)
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
