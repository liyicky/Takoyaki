using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable
{

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float aggroRadius = 10f;
    [SerializeField] float attackRadius = 5f;
    [SerializeField] GameObject projectileToUse;

    public float healthAsPercentage { get { return currentHealthPoint / maxHealthPoints; } }

    float currentHealthPoint = 100f;
    AICharacterControl aiController = null;
    GameObject player = null;

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

        if (distanceToPlayer <= attackRadius)
        {
            print(gameObject.name + "attacking player");
            // TODO: spawn projectile
        }

        if (distanceToPlayer <= aggroRadius)
        {
            aiController.SetTarget(player.transform);
        }
        else
        {
            aiController.SetTarget(transform);
        }

    }
        void OnDrawGizmos()
    {
        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, aggroRadius);

        Gizmos.color = new Color(66f, 134f, 244f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
