using UnityEngine;

public class PlayerTrow : MonoBehaviour
{
    public GameObject gFactory;  //수류탄 공장
    public Transform trowPoint;

    public float trowPower = 20f;

    private void Start()
    {
        gFactory.GetComponent<Rigidbody>();
    }
    void Update()
    {
        Trow();
    }

    void Trow()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject g = Instantiate(gFactory);

            Rigidbody rb = g.GetComponent<Rigidbody>();
            
            g.transform.position = trowPoint.position;
            g.transform.forward = trowPoint.forward;

            Vector3 dir = (trowPoint.forward + trowPoint.up * 0.3f).normalized; 

            rb.AddForce(trowPoint.forward * trowPower, ForceMode.VelocityChange);
            
        }
    }

}
