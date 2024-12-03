using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSpaceController : MonoBehaviour
{
    [SerializeField] private float _thrust = 5f; // Ускорение
    [SerializeField] private float _maxSpeed = 10f; // Максимальная скорость
    [SerializeField] private float _rotationSpeed = 1f; // Скорость поворота
    [SerializeField] private float _rollSpeed = 1f; // Скорость вращения по часовой/против часовой стрелке
    [SerializeField] private float _maxAngularSpeed = 10f;
    [SerializeField] private float _angularDump = 10f;
    [SerializeField] private float _linearDump = 0.5f; // Сопротивление воздуха, для демонстрации, потом удалить


    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        //На всякий случай отключаем гравитацию, если в инспекторе это не сделали.
        _rb.useGravity = false;
        _rb.linearDamping = 0; //Отключаем затухание скорости
        _rb.angularDamping = 0;// и вращения. Чтобы все тестить в рантайме 
        _rb.maxAngularVelocity = _maxAngularSpeed;
    }

    private void FixedUpdate()
    {
        //Получаем данные о движении вперед-назад
        float vertical = Input.GetAxis("Vertical");

        //Вычисляем ускорение
        Vector3 acceleration = transform.forward * vertical * _thrust;

        //Добавляем ускорение
        _rb.AddForce(acceleration, ForceMode.Acceleration);

        //Ограничиваем скорость движения
        if (_rb.linearVelocity.magnitude > _maxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed;
        }

        //Уменьшаем ускорение
        _rb.linearVelocity *= (1 - _linearDump * Time.fixedDeltaTime);
        _rb.angularVelocity *= (1 - _angularDump * Time.fixedDeltaTime);
        
        if (Input.GetKey(KeyCode.A))
        {
            _rb.AddTorque(transform.up * -_rotationSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _rb.AddTorque(transform.up * _rotationSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        // Вращение вокруг оси Z (клавиши Q/E)
        if (Input.GetKey(KeyCode.Q))
        {
            _rb.AddTorque(transform.forward * -_rollSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            _rb.AddTorque(transform.forward * _rollSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        // Вращение вокруг оси X (клавиши R/F)
        if (Input.GetKey(KeyCode.R))
        {
            _rb.AddTorque(transform.right * -_rollSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.F))
        {
            _rb.AddTorque(transform.right * _rollSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}
