using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{

    [SerializeField] float maxHealthPoints = 100f;

    public float healthAsPercentage { get { return currentHealthPoint / maxHealthPoints; } }

    float currentHealthPoint = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        // Mathf.Clamp - Only allows values between two floats (i.e. 0 and maxHealth)
        currentHealthPoint = Mathf.Clamp(currentHealthPoint - damage, 0f, maxHealthPoints);
    }
}
