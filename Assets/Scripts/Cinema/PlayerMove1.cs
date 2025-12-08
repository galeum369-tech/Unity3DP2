using UnityEngine;

public class PlayerMove1 : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    CharacterController cc;             //캐릭터 컨트롤러 컴포넌드

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        cc.Move(dir * speed * Time.deltaTime);
    }
}
