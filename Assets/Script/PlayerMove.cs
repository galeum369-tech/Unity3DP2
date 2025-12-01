using UnityEngine;

/// <summary>
/// 1. 플레이어 이동하기
/// 2. 점프하기
/// </summary>
public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;  //이동 속도
    CharacterController cc;   //캐릭터 컨트롤러 컴포넌트

    //중력적용
    public float gravity = -10f;        //중력
    float velocityY;                    //낙하속도
    float jumpPower = 3f;              //점프파워
    int jumpCount = 0;                  //점프카운트
    public int jumpMaxCount = 2;        //점프 맥스카운트

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //캐릭터 컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        //플레이어 이동
        float h = Input.GetAxis("Horizontal");  //좌우이동
        float v = Input.GetAxis("Vertical");    //앞뒤 이동
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        //게임 기획의도에 따라 대각선은 빠르게 이동하는경우도 있다
        //이경우 노말라이즈 코드 제거
        //transform.position = dir * speed * Time.deltaTime;

        //카메라가 보는 뱡향으로 이동하고픔
        dir = Camera.main.transform.TransformDirection(dir);

        //cc이동하라
        //cc.Move(dir * speed * Time.deltaTime);

        //문제점 : 공중부양

        //중력적용
        //velocityY += gravity * Time.deltaTime;
        //dir.y = velocityY;
        //cc.Move(dir * speed * Time.deltaTime);

        //땅에 닿아 있는 상태라면 낙하속도를 0으로 초기화
        //if (cc.isGrounded)  //땅에 닿아있냐?
        //{
        //    velocityY = 0;
        //}

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            velocityY = 0f;
            jumpCount = 0;
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }


        //if (cc.collisionFlags == CollisionFlags.Above)  //캡슐의 머리
        //if (cc.collisionFlags == CollisionFlags.Sides)  //캡슐의 몸통
        //if (cc.collisionFlags == CollisionFlags.Below)  //캡슐의 하단

        //점프 및 2단 점프
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMaxCount)
        {
            jumpCount++;
            velocityY = jumpPower;

        }

        cc.Move(dir * speed * Time.deltaTime);
    }
}
