using UnityEngine;


/// <summary>
/// 이펙트 발생(충돌 시 삭제)
/// </summary>
public class BulletEffect : MonoBehaviour
{
    public GameObject fxFactory;        //이펙트

    private void OnCollisionEnter(Collision collision)
    {
        ShowEffect();

        Destroy(this.gameObject);
    }

    void ShowEffect()
    {
        GameObject fx = Instantiate(fxFactory);
        fx.transform.position = transform.position;

        Destroy(fx, 0.2f);
    }
}
