using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGroup : MonoBehaviour
{
    [SerializeField] private bool _isFood;

    public bool IsFood => _isFood;

    private void Awake()
    {
        int randomStatus = Random.Range(0, 2);

        if (randomStatus == 0)
        {
            _isFood = true;
        }
        else
        {
            _isFood = false;
        }
    }
}
