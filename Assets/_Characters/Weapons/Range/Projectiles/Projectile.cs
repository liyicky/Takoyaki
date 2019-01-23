using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
  public class Projectile : MonoBehaviour
  {
    // Note: Other classes can set
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject shooter; // For inspecting while paused

    const float DESTROY_DELAY = 60f;
    float damageCaused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetShooter(GameObject shootingGameObject)
    {
      shooter = shootingGameObject;
    }

    public float GetDefaultLaunchSpeed()
    {
      return projectileSpeed;
    }

    public void SetDamage(float damage)
    {
      damageCaused = damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
      DamageIfDamageables(other);
    }

    private void DamageIfDamageables(Collision other)
    {
      Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
      if (shooter && damageableComponent && shooter.layer != other.gameObject.layer)
      {
        (damageableComponent as IDamageable).TakeDamage(damageCaused);
      }
      Destroy(gameObject, DESTROY_DELAY);
    }
  }
}