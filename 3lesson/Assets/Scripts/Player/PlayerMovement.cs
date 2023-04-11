using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField] private float _speed = 6f;//скорость передвижения игрока

    private Vector3 _moveDirection;//направление движения игрока
    private Animator _animator;//аниматор, сюда мы положим наш аниматор игрока
    private Rigidbody _rigidbody;//поле физики, сюда навесим нашу физику, чтобы игрок подчинялся физ. законам и двигался
    private int _floorMask;//маска пола 
    private float _cameraRayLength = 100f;//длина луча камеры

    private void Awake()
    {
        _animator = GetComponent<Animator>();//получаем компонент Animator с объекта Player на сцене
        _rigidbody = GetComponent<Rigidbody>();//получаем компонент Rigidbody с объекта Player на сцене
        _floorMask = LayerMask.GetMask("Floor");//получаем маску Floor
    }

    private void FixedUpdate()//метод FixedUpdate вызывается 50 раз в секнду (фиксированное значение)
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");//переменная отвечает за ввод по горизонтальной оси (значения могут быть 1, -1 и 0)
        float verticalAxis = Input.GetAxisRaw("Vertical");//переменная отвечает за ввод по вертикальной оси (значения могут быть 1, -1 и 0)

        Move(horizontalAxis, verticalAxis);//вызываем метод Move
        Turn();//вызываем метод Turn
        Animate(horizontalAxis, verticalAxis);//вызываем метод Animate
    }

    private void Move(float horizontalAxis, float verticalAxis)//метод для передвижения персонажа, по кнопкам wasd
    {
        _moveDirection.Set(horizontalAxis, 0f, verticalAxis);//задаем направление движение по трем осям X, Y, Z (по Y задаем 0, так как по ней мы двигаться не будем) 
        _moveDirection = _moveDirection.normalized * _speed * Time.deltaTime;//нормализуем значение _moveDirection домнажаем на скорость и на Time.deltaTime
                                                                             //(если не домножить на время, то игрок будет передвигаться слишком быстро, а так он за 0.02 с
                                                                             //передвигается на определенный промежуток шагов по земле, чем больше скорость тем на больше он передвинется)
        _rigidbody.MovePosition(transform.position + _moveDirection);//задаем движение (обязательно сначала в аргументы вносим изначальную позицию transform.position, а потом
                                                                     //направление движения куда мы хотим передвинуться, иначе движения не будет (например если внести только _moveDirection, то персонаж двигаться не будет))
    }

    private void Turn()//метод для поворота персонажа, в зависимости от того куда направлена мышь
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);//луч камеры, это работает так, что мы берем нашу камеру, потом рисуем из камеры луч, а его позиция зависит от ввода мыши
        RaycastHit floorHit;//с помощью этой переменной мы будем обрабатывать соприкосновение луча с полом 

        if(Physics.Raycast (cameraRay, out floorHit, _cameraRayLength, _floorMask))//с помощью этого условия мы куда попал наш луч,
                                                                                   //и в зависимости от этого будем вращать персонажа в ту или иную сторону,
                                                                                   //также из параметров мы передаем сюда наш луч (cameraRay), наш обработчик соприкосновений
                                                                                   //(flooHit) - изначально у нас значение переменной не присвоено, поэтому используем модификатор параметра out
                                                                                   //(он позволит нам присвоить значение чуть позже),
                                                                                   //длину луча камеры (_cameraRayLength), маску пола (_floorMask)
        {
            Vector3 playerToMouse = floorHit.point - transform.position;//обрабатываем соприкосновение луча с полом
            playerToMouse.y = 0f;//обнуляем значение по оси Y, так как по ней мы поворачивать игрока не будем
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);//задаем направление вращения в ту сторону куда смотрит мышь,
                                                                            //используя тип кватеринион, так как этот тип используется в юнити для представления вращений
                                                                            //https://docs.unity3d.com/ScriptReference/Quaternion.html - подробнее можно почитать в доументации юнити
            _rigidbody.MoveRotation(newRotation);//задаем вращение по направлению
        }
    }

    private void Animate(float horizontalAxis, float verticalAxis)//метод для анимации персонажа 
    {
        bool isWalking = horizontalAxis != 0f || verticalAxis != 0f;//если хоть одна из осей не равна нулю, то значение переменной будет true
        _animator.SetBool("isWalking", isWalking);//задаем значений параметра isWalking (который есть в нашем аниматор контроллере), значение будет зависеть от значений
                                                  //переменной isWalking
    }
}
