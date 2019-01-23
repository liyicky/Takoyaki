using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
  public class Enemy : MonoBehaviour, IDamageable
  {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float aggroRadius = 10f;
    [SerializeField] float attackRadius = 5f;
    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float fireInterval = 1f;
    [SerializeField] float fireVariation = 0.1f;
    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;
    [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    float currentHealthPoints;
    Player player = null;
    bool isAttacking = false;

    public void TakeDamage(float damage)
    {
      currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

    // Start is called before the first frame update
    void Start()
    {
      // TODO: create singlton system to keep track of player and camera at all times
      player = FindObjectOfType<Player>();
      currentHealthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
      if (player.healthAsPercentage <= Mathf.Epsilon)
      { 
        StopAllCoroutines();
        Destroy(this);
      }
      float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

      if (distanceToPlayer <= attackRadius && !isAttacking)
      {
        isAttacking = true;
        float randomizedDelay = fireInterval * Random.Range(fireInterval - fireVariation, fireInterval + fireVariation);
        InvokeRepeating("FireProjectile", 0f, randomizedDelay); //TODO: Switch to coroutines
      }

      if (distanceToPlayer > attackRadius)
      {
        isAttacking = false;
        CancelInvoke("FireProjectile");
      }

      if (distanceToPlayer <= aggroRadius)
      {   
        // aiController.SetTarget(player.transform);
      }
      else
      {
        // aiController.SetTarget(transform);
      }

      if (currentHealthPoints <= 0f)
      {
        Destroy(gameObject);
      }
    }

    // TODO: Refactor this method
    void FireProjectile()
    {
      GameObject bullet = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
      Projectile projectileComponent = bullet.GetComponent<Projectile>();
      projectileComponent.SetShooter(gameObject);
      projectileComponent.SetDamage(damagePerShot);

      // TODO: add arch aim via offset
      Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized ;
      bullet.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.GetDefaultLaunchSpeed();
    }

    void OnDrawGizmos()
    {
      Gizmos.color = new Color(255f, 0f, 0, 0.5f);
      Gizmos.DrawWireSphere(transform.position, aggroRadius);

      Gizmos.color = new Color(66f, 134f, 244f, 0.5f);
      Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
  }
}