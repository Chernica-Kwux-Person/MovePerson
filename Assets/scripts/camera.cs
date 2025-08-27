using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float distance = 0.0f;
    [SerializeField] private float distanceVert = 2.0f;
    [SerializeField] private float RotationSpeed = 5.0f;

    [SerializeField] private Vector2 pitchLimits = new Vector2(-30, 60);

    [SerializeField] private float pitch = 0.0f;
    [SerializeField] private float yaw = 5.0f;
    const string controlX = "Mouse X";
    const string controlY = "Mouse Y";

    void LateUpdate()
    {
        if (!target);

        
        float mouseX = Input.GetAxis(controlX) * RotationSpeed;
        float mouseY = Input.GetAxis(controlY) * RotationSpeed;


        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        Vector3 direction = new Vector3 (0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 position = target.position + rotation * direction;
        position.y += distanceVert;

        transform.position = position;
        transform.LookAt(target.position + Vector3.up * distanceVert);

    }

    void Start()
    {
        
    }
}
