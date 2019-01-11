using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float maxHealthPoints = 100f;

    [SerializeField] float currentHealthPoint = 100f;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoint / maxHealthPoints;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
