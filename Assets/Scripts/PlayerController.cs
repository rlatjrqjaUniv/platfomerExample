using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float moveCooltime; // 이동 키를 눌렀을때 소수점 단위로 힘을 가할건지(횟수), (moveSpedd * moveColltime = 가해지는 힘이라고 보면 됨)
    public float moveSpeed; // move Cooltime 1회에 얼만큼의 힘을 가할것인지(힘)
    // 힘을 크게 하고 Colltime을 길게 주면 *관성*이 강해짐
    // 힘을 작게 하고 Colltime을 짧게 주면 *관성*이 약해짐
    public Rigidbody2D rb;
    private bool isMoving = false;

    private void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveLeft()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine(Vector2.left));
        }
    }

    public void MoveRight()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine(Vector2.right));
        }
    }

    IEnumerator MoveCoroutine(Vector2 direction)
    {
        isMoving = true;
        rb.AddForce(direction * moveSpeed, ForceMode2D.Force);
        yield return new WaitForSeconds(moveCooltime); // 조절 가능한 딜레이 시간
        isMoving = false;
    }
}
