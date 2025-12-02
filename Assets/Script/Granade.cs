using UnityEngine;


/// <summary>
/// 1. 포물선 그리며 날아가기(플레이어 쪽에서 해봄)
/// 2. 무언가에 닿으면 터지기(시간 되면 터지기도(나중))
/// </summary>
public class Granade : MonoBehaviour
{
    Vector3 startPosition;
    Rigidbody rb;

    //이펙트
    public GameObject fxFactory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
            ShowEffect();
        }
    }

    void ShowEffect()
    {
        GameObject fx = Instantiate(fxFactory);
        fx.transform.position = transform.position;

        Destroy(fx, 2f);
    }
}
