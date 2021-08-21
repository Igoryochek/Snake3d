using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodCountText : MonoBehaviour
{
    [SerializeField] private SnakeController _snake;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _snake.ChangedFoodCount += OnChangeFoodCount;
    }

    private void OnDisable()
    {
        _snake.ChangedFoodCount -= OnChangeFoodCount;
    }

    private void OnChangeFoodCount(int count)
    {
        _text.text = "Food:  " + count;
    }
}
