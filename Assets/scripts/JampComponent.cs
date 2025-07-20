using UnityEngine;

public class JampComponent : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float TopJamp = 20.0f;
    [SerializeField] private KeyCode JampKey = KeyCode.Space;
    //[SerializeField] private float mass;
    //public RaningComponrnt Raning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //mass = rb.mass;
    }


    void Update()
    {

        
        //Vector3 momentum = Raning.directionAlongSurface;

        if (Input.GetKeyDown(JampKey))
        {
            rb.AddForce(Vector3.up * TopJamp, ForceMode.Impulse );//
        }
    }
}
