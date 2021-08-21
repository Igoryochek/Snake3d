using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _snakeParts;
    [SerializeField] private GameObject _snakePartPrefab;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _finishButton;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _speed;
    [SerializeField] private float _feverDuration;
    [SerializeField] private float _speedX;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Material _snakeMaterial;

    private CharacterController _player;
    private float _moveX;
    private Vector3 _target;
    private Vector3 _direction;
    private float _distance;
    private GameObject _currentPart;
    private GameObject _previousPart;
    private int _crystalCount;
    private int _foodCount;
    private bool _isFever = false;
    private float _feverSpeed = 3;
    private float _rotationAngle = 90;

    public event UnityAction<int> ChangedCristalCount;
    public event UnityAction<int> ChangedFoodCount;
    public event UnityAction<int> IsFinished;
    public event UnityAction IsRestart;

    private void OnEnable()
    {
        SwipeController.OnTouch += OnTouch;
    }

    private void OnDisable()
    {
        SwipeController.OnTouch += OnTouch;
    }

    private void Awake()
    {
        _player = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        MoveBody();
        if (_isFever == false)
        {
            Move();
        }
    }

    private void MoveBody()
    {
        _direction.z = _speed;
        _player.Move(_direction * Time.fixedDeltaTime);

        for (int i = 1; i < _snakeParts.Count; i++)
        {
            _currentPart = _snakeParts[i];
            _previousPart = _snakeParts[i - 1];
            _distance = Vector3.Distance(_previousPart.gameObject.transform.position, _currentPart.gameObject.transform.position);

            Vector3 newPosition = _previousPart.gameObject.transform.position;
            newPosition.y = transform.position.y;
            float time = Time.deltaTime * _distance / _minDistance * _speed;

            if (time > 0.5f)
            {
                time = 0.5f;
            }

            _currentPart.gameObject.transform.position = Vector3.Lerp(_currentPart.gameObject.transform.position, newPosition, time);
            _currentPart.gameObject.transform.rotation = Quaternion.Lerp(_currentPart.gameObject.transform.rotation, _previousPart.gameObject.transform.rotation, time);
        }
    }

    private void OnTouch(float moveX)
    {
        _moveX = moveX;
    }

    private void Move()
    {
        float positionX = transform.position.x + _moveX * _speed * Time.fixedDeltaTime;
        _target = new Vector3(positionX, transform.position.y, transform.position.z);

        if (_target.x < transform.position.x)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -_rotationAngle, 0), _rotationSpeed * Time.fixedDeltaTime);
        }
        else if (_target.x > transform.position.x)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _rotationAngle, 0), _rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target, Time.fixedDeltaTime * _speedX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out FoodGroup foodGroup))
        {
            if (foodGroup.IsFood == false && _isFever == false)
            {
                Time.timeScale = 0;
                _crystalCount = 0;
                _foodCount = 0;
                _restartButton.SetActive(true);
                IsRestart.Invoke();
            }
        }

        if (other.gameObject.TryGetComponent(out CheckPoint checkPoint))
        {
            StartCoroutine(ChangeColor(checkPoint));
        }

        if (other.gameObject.TryGetComponent(out Crystal crystal))
        {
            _crystalCount++;
            Destroy(other.gameObject);
            ChangedCristalCount.Invoke(_crystalCount);
            if (_crystalCount % 3 == 0)
            {
                StartCoroutine(Fever());
            }
        }

        if (other.gameObject.TryGetComponent(out Food food))
        {
            _foodCount++;
            ChangedFoodCount.Invoke(_foodCount);
            GameObject newPart = Instantiate(_snakePartPrefab, transform.position, transform.rotation);
            _snakeParts.Add(newPart);
            Destroy(other.gameObject);
        }

        if (other.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            if (_isFever == false)
            {
                _crystalCount = 0;
                _foodCount = 0;
                ChangedCristalCount.Invoke(_crystalCount);
                ChangedFoodCount.Invoke(_foodCount);
                _restartButton.SetActive(true);
                IsRestart.Invoke();
            }
            else
            {
                Destroy(other.gameObject);
                _foodCount++;
                ChangedFoodCount.Invoke(_foodCount);
            }
        }

        if (other.gameObject.TryGetComponent(out Finish finish))
        {
            ChangedCristalCount.Invoke(_crystalCount);
            ChangedFoodCount.Invoke(_foodCount);
            _finishButton.SetActive(true);
            IsFinished.Invoke(_foodCount);
        }
    }

    private IEnumerator Fever()
    {
        _speed *= _feverSpeed;
        transform.rotation = Quaternion.identity;
        _isFever = true;
        yield return new WaitForSeconds(_feverDuration);

        _crystalCount = 0;
        ChangedCristalCount.Invoke(_crystalCount);
        _speed /= _feverSpeed;
        _isFever = false;
    }

    private IEnumerator ChangeColor(CheckPoint checkPoint)
    {
        float deltaTime = 0;

        while (_snakeMaterial.color != checkPoint.GetComponent<Renderer>().material.color)
        {

            deltaTime += Time.deltaTime * 2;
            _snakeMaterial.color = Color.Lerp(_snakeMaterial.color,
              checkPoint.gameObject.GetComponent<MeshRenderer>().material.color, 1 * Time.deltaTime);
            yield return null;
        }
    }
}
