using System;
using UnityEngine;


[Flags]
public enum PlayerStatus
{
    None = 0,
    isGrounded = 1 << 0,
    Fall = 1 << 1,
    Sliding = 1 << 2,
    Down = 1 << 3
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
 
}
