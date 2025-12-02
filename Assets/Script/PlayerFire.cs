using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 1. 총알 발사
/// 2. 총알 발사 최적화를 위한 오브젝트 풀링
/// </summary>
public class PlayerFire : MonoBehaviour
{
    public GameObject bulletFactory;        //총알 공장(프리팹)
    public Transform firePoint;             //발사 위치

    //오브젝트 풀링 이용해보자(못함 아직 잘 모르겠음 해서 주석처리로 남겨놓음)

    //public int poolSize = 10;   //오브젝트풀링에 사용할 최대 갯수

    //Queue<GameObject> bulletPool;   //총알 오브젝트 풀링 큐

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //오브젝트 풀링 초기화
        //InitObjectPooling();
    }

    //void InitObjectPooling()
    //{
    //    bulletPool = new Queue<GameObject>();
    //    for (int i = 0; i < poolSize; i++)
    //    {
    //        //총알 오브젝트 생성
    //        GameObject bullet = Instantiate(bulletFactory);
    //        //총알 오브젝트 비활성
    //        bullet.SetActive(false);
    //        //큐에 저장
    //        bulletPool.Enqueue(bullet);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //큐로 오브젝트 풀링 발사
            //if (bulletPool.Count > 0)
            //{
            //    //큐에서 오브젝트 꺼내기
            //    GameObject bullet = bulletPool.Dequeue();
            //    //오브젝트 활성 및 위치, 회전설정
            //    bullet.SetActive(true);
            //    bullet.transform.position = firePoint.position;
            //    bullet.transform.forward = firePoint.forward;
            //}
            //else //오브젝트 풀이 비어있는 경우
            //{
            //    GameObject bullet = Instantiate(bulletFactory);
            //    bullet.SetActive(false);
            //    //오브젝트 풀에 추가
            //    bulletPool.Enqueue(bullet);
            //}

            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = firePoint.position;
            bullet.transform.forward = firePoint.forward;   
        }
    }

    // 오브젝트 풀에 오브젝트 다시 추가하는 함수 (외부에서 호출 가능하도록 public 선언)
    //public void ReloadPool(GameObject obj)
    //{
    //    //총알 비활성
    //    obj.SetActive(false);
    //    //오브젝트 풀에 다시 추가
    //    bulletPool.Enqueue (obj);
    //}
}
