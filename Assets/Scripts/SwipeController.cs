using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwipeController : MonoBehaviour
{
    private Vector2 _startPosition;
    private float _direction;

    public static event UnityAction<float> OnTouch;

    private void Update()
    {
#if UNITY_EDITOR
        OnTouch.Invoke(Input.GetAxis("Horizontal"));
#endif

#if UNITY_ANDROID
        GetTouchInput();
#endif
    }
    private void GetTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    _direction = touch.position.x > _startPosition.x ? 1f : -1f;
                    break;
                default:
                    _startPosition = touch.position;
                    _direction = 0;
                    break;
            }

            OnTouch.Invoke(_direction);
        }
    }
}
