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
        //���� ���ǵ� Ű���� ������
        foreach (KeyMapping key in KeyMappings)
        {
            if (!Input.anyKey)
            {
                Debug.Log("Input : NULL"); // ���� Ű ���
                pc.Invoke("Idle", 0);
                break;
            }

            if (Input.GetKeyDown(key.KeyCode))
            {
                lastKey = key;
            }
            else if (Input.GetKey(key.KeyCode))
            {
                Debug.Log("Input : "+ key.KeyCode+", " + key.FunctionName); // ���� Ű ���
                pc.Invoke(lastKey.FunctionName, 0); // Ű�� ���ε� �Լ� ����
            }
            else if(Input.GetKeyUp(key.KeyCode))
            {
                //beforeKey = key;
            }
        }
    }
}