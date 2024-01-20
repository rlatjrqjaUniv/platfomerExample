using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    public void MoveRight()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }




    //함수 포인터로 개조 예정
    System.Reflection.MethodInfo methodInfo;
    public void Action(string functionName)
    {
        methodInfo = GetType().GetMethod(functionName);

        // 메서드가 찾아졌는지 확인
        if (methodInfo != null)
        {
            // 메서드 실행
            methodInfo.Invoke(this, null);
        }
        else
        {
            Debug.LogError("Method not found: " + functionName);
        }
    }

}
