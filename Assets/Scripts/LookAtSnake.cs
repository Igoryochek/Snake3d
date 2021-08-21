using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSnake : MonoBehaviour
{
    [SerializeField] private Transform _snake;
    [SerializeField]private Vector3 _offset;

    private void Update()
    {
        Vector3 camPosition = new Vector3(0, _snake.transform.position.y, _snake.transform.position.z);
        transform.position = camPosition + _offset;
    }
}
