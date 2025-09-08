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


    [SerializeField] private KeyCode JampKey = KeyCode.Space;

    private bool JampI; 

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
    }
}
