using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]

public class Player : MonoBehaviour, IDamageable
{

    [SerializeField] int enemyLayer = 10;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] Weapon weaponInUse;
    [SerializeField] GameObject weaponSocket;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    CameraRaycaster cameraRaycaster;
    GameObject currentTarget;
    float currentHealthPoints;
    float lastHitTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        currentHealthPoints = maxHealthPoints;

        PutWeaponInHand();
    }

    void SetupMouseClick()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnTargetClicked;
    }

    void PutWeaponInHand()
    {
        var weaponPrefab = weaponInUse.GetWeaponPrefab();
        GameObject weapon = Instantiate(weaponPrefab, weaponSocket.transform) as GameObject;
        weapon.transform.localPosition = weaponInUse.girpTransform.localPosition;
        weapon.transform.localRotation = weaponInUse.girpTransform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        // Mathf.Clamp - Only allows values between two floats (i.e. 0 and maxHealth)
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

    void OnTargetClicked(RaycastHit hit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = hit.collider.gameObject;
            currentTarget = enemy;
            Attack();
        }
    }

    void Attack()
    {            
        var targetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
        if (targetDistance > attackRadius)
        {
            return;
        }

        if (Time.time - lastHitTime > attackCooldown)
        {
            print("attacked");
            (currentTarget.GetComponent(typeof(IDamageable)) as IDamageable).TakeDamage(attackDamage);
            lastHitTime = Time.time;
        }
    }
}
