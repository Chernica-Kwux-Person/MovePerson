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
    private Vector3 _normalSliding; // Нормаль поверхности, на которой стоит персонаж

    private StateMachine StateMachine;

    private InputManager InputManager;


    private Vector3 offset;
    private Vector3 rbVelocity;

    [SerializeField] private float strongGraviti = -0.2f;
    [SerializeField] private float strongJamp = 15;

    float moveY;

    [SerializeField] private bool jampStart = false;

    [SerializeField]
    int countCollision = 0;
    int countCollisionSliding = 0;

    void Start()
    {
        moveY = strongGraviti;
        rb = GetComponent<Rigidbody>(); // Получаем Rigidbody объекта
        StateMachine = GetComponent<StateMachine>();
        InputManager = GetComponent<InputManager>();
        rb.freezeRotation = true;       // Запрещаем физическую ротацию объекта
    }


    void Update()
    {

        if (StateMachine.state.HasFlag(PlayerStatus.isGrounded))
        {
            StateMachine.state &= ~PlayerStatus.Fall;
        }
        else
        {
            StateMachine.state = StateMachine.state | PlayerStatus.Fall;
        }
        //Debug.Log("Итоговое состояние" + state);
    }

    void FixedUpdate()
    {
        rbVelocity = rb.linearVelocity;
        rb.AddForce(offset - rbVelocity, ForceMode.VelocityChange);
        Debug.DrawLine(transform.position, transform.position + offset * 20.0f, Color.red);

        if (countCollision > 0)
        {
            StateMachine.state = StateMachine.state | PlayerStatus.isGrounded;
        }
        else
        {
            StateMachine.state &= ~PlayerStatus.isGrounded;
        }
        countCollision = 0;

        if (countCollisionSliding > 0)
        {
            StateMachine.state = StateMachine.state | PlayerStatus.Sliding;
        }
        else
        {
            StateMachine.state &= ~PlayerStatus.Sliding;
        }
        countCollision = 0;
    }


    private void OnCollisionStay(Collision collision)
    {
        countCollisionSliding = 0;
        cerVel = carSpead(collision.rigidbody);
        _normal = Vector3.zero;
        foreach (ContactPoint contact in collision.contacts)
        {
            GroundChek(contact);
        }
        _normal /= countCollision;
        _normal.Normalize();
        _normalSliding /= countCollisionSliding;
        _normalSliding.Normalize();
    }

    private void GroundChek(ContactPoint contact)
    {

        if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
        {

            countCollision += 1;
            _normal += contact.normal;
        }
        else
        {
            countCollisionSliding += 1;
            _normalSliding += contact.normal;
            
        }
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
    private Vector3 Project(Vector3 forward, Vector3 norm)
    {
        Debug.Log(norm);
        return forward - Vector3.Dot(forward, norm) * norm;

    }

    public void Move(float moveHorizontal, float moveVertical)
    {
        // Получаем направление движения на основе камеры и ввода
        Vector3 camGO1forvard = moveHorizontal * camera.right;   // движение вправо/влево относительно камеры
        Vector3 camGO2forvard = moveVertical * camera.forward;   // движение вперёд/назад относительно камеры
        VectorRaning = camGO1forvard + camGO2forvard;    // итоговое направление движения

        Debug.Log(StateMachine.state);
        if (StateMachine.state.HasFlag(PlayerStatus.isGrounded) & jampStart == false)
        {
            // Проецируем направление на плоскость поверхности, чтобы двигаться по склонам
            Vector3 directionAlongSurface = Project(VectorRaning.normalized, _normal);
            Vector3 sped = directionAlongSurface * moveSpeed;

            offset = sped + cerVel;
            Debug.Log(cerVel);

            Debug.DrawLine(transform.position, transform.position + _normal * 20.0f, Color.red);
            // Смещаем Rigidbody


            //Debug.Log("Зашли");
        }
        else if (StateMachine.state.HasFlag(PlayerStatus.Fall) || (jampStart & StateMachine.state.HasFlag(PlayerStatus.isGrounded)))
        {
            Vector3 vectorY = new Vector3(0, moveY, 0);
            offset = offset + VectorRaning * moveSpeedFlay + vectorY;
            moveY -= strongGraviti;
            jampStart = false;
        }
        /*
        if (StateMachine.state.HasFlag(PlayerStatus.Sliding))
        {
            Vector3 slising = Project(offset, _normalSliding);
            offset = slising;
        }
        */
    }

    public void Jamp()
    {
        if (StateMachine.state.HasFlag(PlayerStatus.isGrounded))
        {
            moveY = strongJamp;
            jampStart = true;
        }
    }

}