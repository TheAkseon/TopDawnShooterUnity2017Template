using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField] private float _speed = 6f;//скорость передвижения игрока

    private Vector3 _moveDirection;//направление движения игрока
    private Animator _animator;

}
