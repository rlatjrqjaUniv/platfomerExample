using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public PlayerController pc;

    [System.Serializable]
    public class KeyMapping
    {
        public string KeyCode;
        public string FunctionName;
    }

    public KeyMapping[] KeyMappings;
    public KeyMapping beforeKey;
    public KeyMapping lastKey;

    private void Update()
    {
        //사전 정의된 키들을 가져옴
        foreach (KeyMapping key in KeyMappings)
        {
            if (!Input.anyKey)
            {
                Debug.Log("Input : NULL"); // 누른 키 출력
                pc.Invoke("Idle", 0);
                break;
            }

            if (Input.GetKeyDown(key.KeyCode))
            {
                lastKey = key;
            }
            else if (Input.GetKey(key.KeyCode))
            {
                Debug.Log("Input : "+ key.KeyCode+", " + key.FunctionName); // 누른 키 출력
                pc.Invoke(lastKey.FunctionName, 0); // 키에 매핑된 함수 실행
            }
            else if(Input.GetKeyUp(key.KeyCode))
            {
                //beforeKey = key;
            }
        }
    }
}