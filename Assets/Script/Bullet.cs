using UnityEngine;



/// <summary>
/// 1.총알 이동
/// 2.일정 거리 이동 후 삭제
/// </summary>
public class Bullet : MonoBehaviour
{
    public float speed = 15f;       //초당 15이동
    public float destroyDistance = 30f; //30이동 후 삭제
    Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //총알 이동 
        transform.Translate(Vector3.forward * speed *  Time.deltaTime);

        //발사 위치로부터 일정거리 이상 떨어지면 삭제(비활성
        //거리 계산 에는 sqrMagnitude사용
        float sqrDistance = (startPosition - transform.position).sqrMagnitude;
        if (sqrDistance > destroyDistance * destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
