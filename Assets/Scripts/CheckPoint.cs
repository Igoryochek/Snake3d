using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private FoodGroup[] _foodGroups;

    private void Start()
    {
        foreach (var food in _foodGroups)
        {
            if (food.IsFood)
            {
                for (int i = 0; i < food.transform.childCount; i++)
                {
                    food.transform.GetChild(i).GetComponentInChildren<Renderer>().material.color = GetComponent<Renderer>().material.color;
                }
            }
        }       
    }
}
