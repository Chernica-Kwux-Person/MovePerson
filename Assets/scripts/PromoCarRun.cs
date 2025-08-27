using UnityEngine;

public class PromoCarRun : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 offset = new Vector3(1,0,0);
        offset *=  moveSpeed;
        offset = offset - rb.linearVelocity;
        offset.y = 0;
        rb.AddForce(offset, ForceMode.VelocityChange);
    }

}