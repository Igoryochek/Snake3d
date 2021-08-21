using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Finish : MonoBehaviour
{
    [SerializeField] private SnakeController _snake;
    [SerializeField] private TextMeshProUGUI _finishText;

    private void OnEnable()
    {
        _snake.IsFinished += OnFinished;
    }

    private void OnDisable()
    {
        _snake.IsFinished -= OnFinished;

    }
    private void OnFinished(int count)
    {
        StartCoroutine(SetText(count));
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    private IEnumerator SetText(int count)
    {
        _finishText.text = "Mission Completed!";

        yield return new WaitForSeconds(1);

        _finishText.text = "Your score:" + count + "     Press";
        Time.timeScale = 0;
    }
}
