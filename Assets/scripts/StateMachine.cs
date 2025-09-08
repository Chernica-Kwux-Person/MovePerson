using System;
using UnityEngine;


[Flags]
public enum PlayerStatus
{
    None = 0,
    isGrounded = 1 << 0,
    Fall = 1 << 1,
}

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    PlayerStatus _state = PlayerStatus.None;
    public PlayerStatus state
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            //Debug.Log(value);
        }
    }

    [SerializeField]
    public int number { get; private set; } = 10;

    [SerializeField]
    
    int countCollision = 0;

    void Update()
    {
        
        if (state.HasFlag(PlayerStatus.isGrounded))
        {
            state &= ~PlayerStatus.Fall;
        }
        else
        {
            state = state | PlayerStatus.Fall;
        }
        //Debug.Log("Итоговое состояние" + state);
    }

    void FixedUpdate()
    {
        if (countCollision > 0)
        {
            state = state | PlayerStatus.isGrounded;
        }
        else
        {
            state &= ~PlayerStatus.isGrounded;
        }
        countCollision = 0;
    }



    private void OnCollisionStay(Collision collision)
    {
        // При столкновении сохраняем нормаль поверхности, на которой стоит персонаж
        foreach (ContactPoint contact in collision.contacts)
        {
            GroundChek(contact);
        }
    }

    private void GroundChek(ContactPoint contact)
    {

        if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
        {
            
            countCollision += 1;
        }
        
    }
}
