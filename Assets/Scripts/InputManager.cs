using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public PlayerController pc;
    string pressedKey;

    [System.Serializable]
    public class KeyMapping
    {
        public string KeyCode;
        public string FunctionName;
    }

    public KeyMapping[] KeyMappings;

    private void Update()
    {
        pressedKey = Input.inputString;
        
        //눌린 키가 있는가
        if (pressedKey != null) 
        {
            //사전 정의된 키들을 가져옴
            foreach (KeyMapping key in KeyMappings)
            {
                if (Input.GetKey(key.KeyCode))
                {
                    Debug.Log(pressedKey); // 누른 키 출력
                    pc.Invoke(key.FunctionName, 0); // 키에 매핑된 함수 실행
                }
            }
        }
    }
}
