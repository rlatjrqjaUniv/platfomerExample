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
        
        //���� Ű�� �ִ°�
        if (pressedKey != null) 
        {
            //���� ���ǵ� Ű���� ������
            foreach (KeyMapping key in KeyMappings)
            {
                if (Input.GetKey(key.KeyCode))
                {
                    Debug.Log(pressedKey); // ���� Ű ���
                    pc.Invoke(key.FunctionName, 0); // Ű�� ���ε� �Լ� ����
                }
            }
        }
    }
}
