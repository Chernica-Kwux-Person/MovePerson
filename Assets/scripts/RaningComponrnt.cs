using UnityEngine;

public class RaningComponrnt : MonoBehaviour
{
    // Ссылка на трансформ камеры, используется для направления движения
    public Transform camera;
    
    // Скорость вращения (не используется в текущем коде)
    [SerializeField] private float RotationSpeed = 5.0f;

    // Скорость движения персонажа
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private bool isGrounded = false;

    private Rigidbody rb; // Физическое тело объекта
    private float moveHorizontal; // Горизонтальный ввод игрока (A/D, стрелки)
    private float moveVertical;   // Вертикальный ввод игрока (W/S, стрелки)
    private int flag; // Переменная не используется, возможно заготовка для логики
    private Vector3 camRight;     // Правая сторона камеры (не используется)
    private Vector3 camForward;   // Перед камеры (не используется)
    
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
        // Считываем ввод от игрока по осям
        moveHorizontal = Input.GetAxisRaw(controlHorizontal);
        moveVertical = Input.GetAxisRaw(controlVertical);
    }

    void FixedUpdate()
    {

        if (isGrounded)
        {
            // Получаем направление движения на основе камеры и ввода
            Vector3 camGO1forvard = moveHorizontal * camera.right;   // движение вправо/влево относительно камеры
            Vector3 camGO2forvard = moveVertical * camera.forward;   // движение вперёд/назад относительно камеры
            Vector3 VectorRaning = camGO1forvard + camGO2forvard;    // итоговое направление движения

            // Выполняем перемещение
            Move(VectorRaning);

            // Альтернатива с прямым смещением (закомментирована):
            // rb.MovePosition(rb.position + VectorRaning * moveSpeed * Time.deltaTime);
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
            // Проверяем, направлена ли нормаль вверх — значит, это "земля"
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                _normal = contact.normal; // сохраняем нормаль для корректной проекции движения
                
            }
            
        }
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

    // Метод движения по поверхности
    private void Move(Vector3 direction)
    {
        // Проецируем направление на плоскость поверхности, чтобы двигаться по склонам
        Vector3 directionAlongSurface = Project(direction.normalized);
        
        // Смещение на кадр с учётом скорости и времени
        Vector3 offset = directionAlongSurface * (moveSpeed * Time.deltaTime);

        // Смещаем Rigidbody
        rb.MovePosition(rb.position + offset);
    }
}