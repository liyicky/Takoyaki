using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable
{

    [SerializeField] float maxHealthPoints = 100f;

    [SerializeField] float aggroRadius = 10f;
    [SerializeField] float attackRadius = 5f;

    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float fireInterval = 1f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);

    public float healthAsPercentage { get { return currentHealthPoint / maxHealthPoints; } }

    float currentHealthPoint = 100f;
    AICharacterControl aiController = null;
    GameObject player = null;

    bool isAttacking = false;

    public void TakeDamage(float damage)
    {
        currentHealthPoint = Mathf.Clamp(currentHealthPoint - damage, 0f, maxHealthPoints);
    }

    // Start is called before the first frame update
    void Start()
    {
        aiController = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, fireInterval); //TODO: Switch to coroutines
        }

        if (distanceToPlayer > attackRadius)
        {
            isAttacking = false;
            CancelInvoke("SpawnProjectile");
        }

        if (distanceToPlayer <= aggroRadius)
        {   
            aiController.SetTarget(player.transform);
        }
        else
        {
            // isAttacking = false;
            aiController.SetTarget(transform);
        }
    }

    void SpawnProjectile()
    {
        GameObject bullet = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = bullet.GetComponent<Projectile>();
        projectileComponent.damageCaused = damagePerShot;

        // TODO: add arch aim via offset
        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized ;
        bullet.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.projectileSpeed;
    }
        void OnDrawGizmos()
    {
        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, aggroRadius);

        Gizmos.color = new Color(66f, 134f, 244f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
