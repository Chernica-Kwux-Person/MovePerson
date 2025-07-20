using UnityEngine;

public class MovePerson: MonoBehaviour
{

    public Transform target;
    
    

    void LateUpdate()
    {
        Vector3 tg = target.position;
        transform.position = tg;

        Vector3 rot = target.eulerAngles;
        rot.x = 0;
        transform.eulerAngles = rot;
    }
}
