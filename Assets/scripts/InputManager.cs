using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float moveHorizontal; // Горизонтальный ввод игрока (A/D, стрелки)
    public float moveVertical;   // Вертикальный ввод игрока (W/S, стрелки)
    // Имена осей ввода
    const string controlHorizontal = "Horizontal";
    const string controlVertical = "Vertical";

    private RaningComponrnt2 RaningC;

    [SerializeField] private Doun Doun;


    [SerializeField] private KeyCode JampKey = KeyCode.Space;

    [SerializeField] private KeyCode fall = KeyCode.X;

    private bool JampI; 
    private bool Down;

    void Start()
    {
        RaningC = GetComponent<RaningComponrnt2>(); // Получаем Rigidbody объекта
    }


    void Update()
    {
        // Считываем ввод от игрока по осям
        moveHorizontal = Input.GetAxisRaw(controlHorizontal);
        moveVertical = Input.GetAxisRaw(controlVertical);

        if (Input.GetKey(JampKey))
        {
            JampI = true;
        }

        if (Input.GetKey(fall))
        {
            Down = true;
        }
    }

    void FixedUpdate()
    {
        if (JampI)
        {
            RaningC.Jamp();
        }
        RaningC.Move(moveHorizontal, moveVertical);
        if (JampI)
        {
            JampI = false;
        }
        if (Down)
        {
            Doun.DownMove();
            Down = false;
        }
    }
}
