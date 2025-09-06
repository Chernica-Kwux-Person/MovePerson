using NUnit.Framework.Constraints;
using UnityEngine;

public class RaningComponrnt2 : MonoBehaviour
{
    // Ссылка на трансформ камеры, используется для направления движения
    public Transform camera;

    // Скорость вращения (не используется в текущем коде)
    [SerializeField] private float RotationSpeed = 5.0f;

    // Скорость движения персонажа
    [SerializeField] private float moveSpeed = 10.0f;

    [SerializeField] private float moveSpeedFlay = 5.0f;


    [SerializeField] private float TopJamp = 20.0f;
    [SerializeField] private bool isGrounded = false;

    

    [SerializeField] private Rigidbody car;

    private Vector3 cerVel = Vector3.zero;


    private Rigidbody rb; // Физическое тело объекта


    private Vector3 lastMove = Vector3.zero;
    private bool tim;
    Vector3 VectorRaning;
     private bool space = false;


    private Vector3 _normal; // Нормаль поверхности, на которой стоит персонаж

    private StateMachine StateMachine;

    private InputManager InputManager;


    private Vector3 offset;
    private Vector3 rbVelocity;

    [SerializeField] private float strongGraviti = -0.2f;
    [SerializeField] private float strongJamp = 15;

    float moveY;

    void Start()
    {
        moveY = strongGraviti;
        rb = GetComponent<Rigidbody>(); // Получаем Rigidbody объекта
        StateMachine = GetComponent<StateMachine>();
        InputManager = GetComponent<InputManager>();
        rb.freezeRotation = true;       // Запрещаем физическую ротацию объекта
    }

    void FixedUpdate()
    {
        rbVelocity = rb.linearVelocity;
        rb.AddForce(offset - rbVelocity, ForceMode.VelocityChange);
    }


    private void OnCollisionStay(Collision collision)
    {
        cerVel = carSpead(collision.rigidbody);

    }

    private Vector3 carSpead(Rigidbody collisionRB)
    {
        if (collisionRB != null)
        {
            car = collisionRB;
            return car.linearVelocity;
        }
        else
        {
            return Vector3.zero;
        }
    }



    // Проекция вектора на плоскость, перпендикулярную нормали
    private Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, _normal) * _normal;
    }

    public void Move(float moveHorizontal, float moveVertical, bool JampI)
    {
        // Получаем направление движения на основе камеры и ввода
        Vector3 camGO1forvard = moveHorizontal * camera.right;   // движение вправо/влево относительно камеры
        Vector3 camGO2forvard = moveVertical * camera.forward;   // движение вперёд/назад относительно камеры
        VectorRaning = camGO1forvard + camGO2forvard;    // итоговое направление движения
        //Debug.Log("Пришли");
        if (StateMachine.state.HasFlag(PlayerStatus.isGrounded) & JampI == false)
        {
            // Проецируем направление на плоскость поверхности, чтобы двигаться по склонам
            Vector3 directionAlongSurface = Project(VectorRaning.normalized);
            Vector3 sped = directionAlongSurface * moveSpeed;

            offset = sped + cerVel;
            Debug.Log(cerVel);

            Debug.DrawLine(transform.position, transform.position + offset * 20.0f, Color.red);
            // Смещаем Rigidbody


            //Debug.Log("Зашли");
        }
        else if (StateMachine.state.HasFlag(PlayerStatus.Fall) || JampI)
        {
            Vector3 vectorY = new Vector3 (0, moveY, 0);
            offset = offset + VectorRaning * moveSpeedFlay + vectorY;
            moveY -= strongGraviti;
        }
    }

    public void Jamp()
    {
        if (StateMachine.state.HasFlag(PlayerStatus.isGrounded))
        {
            moveY = strongJamp;
        }
    }

}
