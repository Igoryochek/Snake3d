using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrystalCountText : MonoBehaviour
{
    [SerializeField] private SnakeController _snake;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _snake.ChangedCristalCount += OnChangeCrystalCount; 
    }

    private void OnDisable()
    {
        _snake.ChangedCristalCount -= OnChangeCrystalCount;
    }

    private void OnChangeCrystalCount(int count)
    {
        _text.text ="Crystal:  "+ count;
    }
}
