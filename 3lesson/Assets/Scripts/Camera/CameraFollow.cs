using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
    [SerializeField] private Player _player;//на это поле мы будем накидывать нашего игрока
    [SerializeField] private float _speedStartingPositionToTarget = 5f;//это поле отвечает за скорость передвижения от
                                                                       //начальной позиции к позиции игрока

    private Vector3 _offset;//смещение камеры от позиции игрока

    private void Start()//метод вызывается 1 раз со стартом игры, в нем мы находим игрока и задаем смещение
    {
        _player = FindObjectOfType<Player>();//находим игрока по типу
        _offset = transform.position - _player.transform.position;//задаем смещение камеры
    }

    private void FixedUpdate()//этот метод вызывается 50 раз в секунду
    {
        FollowPlayer();//вызываем метод следования за игроком
    }

    private void FollowPlayer()//метод следования за игроком
    {
        Vector3 playerCameraPosition = _player.transform.position + _offset;//задаем значение позиции камеры (скаладывается из позиции игрока + смещения)

        transform.position = Vector3.Lerp(transform.position, playerCameraPosition, _speedStartingPositionToTarget * Time.deltaTime);//передвигаем камеру с помощью Lerp
                                                                                                                                     //это позволяет нам плавно передвигть камеру с определенной скоростью из одной позиции в другую
                                                                                                                                     //в данном случае из позиции камеры в смещенную позицию игрока, также домнажаем скорость на время для достижения
                                                                                                                                     //плавного перемещения камеры
    }
}

