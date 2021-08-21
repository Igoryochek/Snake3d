using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] private SnakeController _snake;

    private void OnEnable()
    {
        _snake.IsRestart += OnRestart;
    }

    private void OnDisable()
    {
        _snake.IsRestart -= OnRestart;
    }
    private void OnRestart()
    {
        Time.timeScale = 0;
    }

    public void LoadAgain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
