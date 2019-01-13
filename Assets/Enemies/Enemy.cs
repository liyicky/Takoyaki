using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float aggroRadius = 5f;

    float currentHealthPoint = 100f;
    AICharacterControl aiController = null;
    GameObject player = null;

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
        aiController = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= aggroRadius)
        {
            aiController.SetTarget(player.transform);
        }
        else
        {
            aiController.SetTarget(transform);
        }

        // Collider[] hitColliders = Physics.OverlapSphere(transform.position, aggroRadius);
        // foreach (var hit in hitColliders)
        // {
        //     if (hit.tag == "Player")
        //     {
        //         GetComponent<AICharacterControl>().SetTarget(hit.transform);
        //         target = hit.transform;
        //     }
        // }

        // if (target != null)
        // {
        //     var someting = target.position - transform.position;
        //     if (someting.magnitude >= 0.1f)
        //     {
        //     //    this.Move(Vector3.zero, false, false);
        //     }
        // }
    }

        void OnDrawGizmos()
    {
        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}
