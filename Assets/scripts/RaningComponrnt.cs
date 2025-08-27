using UnityEngine;

public class RaningComponrnt : MonoBehaviour
{
    // Ссылка на трансформ камеры, используется для направления движения
    public Transform camera;
    
    // Скорость вращения (не используется в текущем коде)
    [SerializeField] private float RotationSpeed = 5.0f;

    // Скорость движения персонажа
    [SerializeField] private float moveSpeed = 10.0f;

    [SerializeField] private float moveSpeedFlay = 5.0f;
    [SerializeField] private bool isGrounded = false;

    [SerializeField] private KeyCode JampKey = KeyCode.Space;

    [SerializeField] private Rigidbody car;

    private Vector3 carSpead = Vector3.zero;


    private Rigidbody rb; // Физическое тело объекта
    private float moveHorizontal; // Горизонтальный ввод игрока (A/D, стрелки)
    private float moveVertical;   // Вертикальный ввод игрока (W/S, стрелки)
    private int flag; // Переменная не используется, возможно заготовка для логики
    private Vector3 camRight;     // Правая сторона камеры (не используется)
    private Vector3 camForward;   // Перед камеры (не используется)
    private Vector3 lastMove = Vector3.zero;
    private bool tim;
    Vector3 VectorRaning;
    [SerializeField] private bool space = false;
    

    // Имена осей ввода
    const string controlHorizontal = "Horizontal";
    const string controlVertical = "Vertical";
    
    private Vector3 _normal; // Нормаль поверхности, на которой стоит персонаж

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Получаем Rigidbody объекта
        rb.freezeRotation = true;       // Запрещаем физическую ротацию объекта
    }

    void Update()
    {
        //Debug.Log("111111111111111111111111");
        // Считываем ввод от игрока по осям
        moveHorizontal = Input.GetAxisRaw(controlHorizontal);
        moveVertical = Input.GetAxisRaw(controlVertical);
        
        if (Input.GetKeyDown(JampKey))
        {
            space = true;
        }
        
        
        
    }

    void FixedUpdate()
    {

        
        if (isGrounded)
        {
            // Получаем направление движения на основе камеры и ввода
            Vector3 camGO1forvard = moveHorizontal * camera.right;   // движение вправо/влево относительно камеры
            Vector3 camGO2forvard = moveVertical * camera.forward;   // движение вперёд/назад относительно камеры
            VectorRaning = camGO1forvard + camGO2forvard;    // итоговое направление движения
            
            // Выполняем перемещение
            
            if (space)
            {
                return;
            }
            //Debug.Log("Не летим");
            Move(VectorRaning, carSpead);
            
            // Альтернатива с прямым смещением (закомментирована):                                   Мб весь этот блок в OnCollisionStay?????
            // rb.MovePosition(rb.position + VectorRaning * moveSpeed * Time.deltaTime);             Мб весь этот блок в OnCollisionStay?????      этот if из FixedUpdate
        }
        else
        {
            //Debug.Log("ЛЛЛЛЛЛЛЛЛетим");
            Vector3 camGO1forvard = moveHorizontal * camera.right;   // движение вправо/влево относительно камеры
            Vector3 camGO2forvard = moveVertical * camera.forward;   // движение вперёд/назад относительно камеры
            VectorRaning = camGO1forvard + camGO2forvard;    // итоговое направление движения
            MoveFlay(VectorRaning);
        }
    

        
        
    }

    // Проекция вектора на плоскость, перпендикулярную нормали
    private Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, _normal) * _normal;
    }

    private void OnCollisionStay(Collision collision)
    {
        
        // При столкновении сохраняем нормаль поверхности, на которой стоит персонаж
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.rigidbody != null)
            {
                //Debug.Log("Ква");
                car = collision.rigidbody;
                //Debug.Log("Все еще ква");
                carSpead = car.linearVelocity;
                Debug.Log(carSpead);
                
            }
            else
            {
                carSpead = Vector3.zero;
            }
            //Debug.Log("00000000000000000000000");
            // Проверяем, направлена ли нормаль вверх — значит, это "земля"
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                _normal = contact.normal; // сохраняем нормаль для корректной проекции движения
                
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Когда персонаж перестает касаться поверхности — снимаем флаг
        space = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        // Когда персонаж перестает касаться поверхности — снимаем флаг
        isGrounded = false;
    }
    
    // Рисуем вспомогательные линии в редакторе Unity
    private void OnDrawGizmos()
    {
        // Белая линия — нормаль поверхности
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + _normal * 3);

        // Красная линия — проекция направления движения на плоскость
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Project(transform.forward));
    }

    private void MoveFlay(Vector3 direction)
    {
        //Vector3 directionAlongSurface = Project(direction.normalized);
        Vector3 offset = direction * moveSpeedFlay;
        offset = offset - rb.linearVelocity;
        offset.y = 0;
        
        //Vector3 x = rb.linearVelocity;
        //x.y = 0;
        //Debug.Log("fdmdfgj");
        rb.AddForce(offset);
    }







    // Метод движения по поверхности
    private void Move(Vector3 direction, Vector3 carSp)
    {
        // Проецируем направление на плоскость поверхности, чтобы двигаться по склонам
        Vector3 directionAlongSurface = Project(direction.normalized);
        
        
        Vector3 offset = directionAlongSurface * moveSpeed;

        // if (carSp != Vector3.zero)
        // {
        offset = offset - rb.linearVelocity + carSp;
        // }
        // else
        // {
        //     offset = offset - rb.linearVelocity;
        // }
        //offset.y = 0;
        //Debug.Log(offset.y);
        Debug.Log(carSp);
        
        Debug.DrawLine(transform.position, transform.position + offset * 20.0f, Color.red);
        // Смещаем Rigidbody
        
        
        
        rb.AddForce(offset, ForceMode.VelocityChange);
        //lastMove = rb.linearVelocity;
    }
}