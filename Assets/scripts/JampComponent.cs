using UnityEngine;

public class JampComponent : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float TopJamp = 20.0f;
    [SerializeField] private KeyCode JampKey = KeyCode.Space;
    //[SerializeField] private float mass;
    [SerializeField] private float graund = 0.5f;
    [SerializeField] private bool space = false;

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //mass = rb.mass;
    }

    void Update()
    {
        if (Input.GetKeyDown(JampKey))
        {
            space = true;
        }


    }

    private void OnCollisionStay(Collision collision)
    {

        
        Vector3 velocity = rb.linearVelocity;

        //Vector3 momentum = Raning.directionAlongSurface;

        
        foreach (ContactPoint contact in collision.contacts)
        {
            
            if (Vector3.Dot(contact.normal, Vector3.up) > graund)
            {
                
                if (space)
                {
                    //velocity.y = 0;
                    //rb.AddForce(velocity, ForceMode.VelocityChange);
                    Vector3 run = rb.linearVelocity;
                    run.y = 0;
                    rb.AddForce(Vector3.up * TopJamp + run, ForceMode.Impulse);//
                    space = false;
                }
            }
        }
    }
}
