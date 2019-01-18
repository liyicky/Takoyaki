using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Weapon;
using RPG.Core;
using System;

namespace RPG.Characters
{
  [RequireComponent(typeof(CameraRaycaster))]

  public class Player : MonoBehaviour, IDamageable
  {

    [SerializeField] int enemyLayer = 10;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] RPG.Weapon.Weapon weaponInUse;
    [SerializeField] AnimatorOverrideController animatorOverrideController;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    CameraRaycaster cameraRaycaster;
    GameObject currentTarget;
    float currentHealthPoints;
    float lastHitTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
      SetCurrentMaxHealth();
      SetupMouseClick();
      PutWeaponInHand();
      OverrideAnimatorController();
    }

    private void SetCurrentMaxHealth()
    {
      currentHealthPoints = maxHealthPoints;
    }

    private void OverrideAnimatorController()
    {
      var animator = GetComponent<Animator>();
      animator.runtimeAnimatorController = animatorOverrideController;
      animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO: remove const
    }

    void SetupMouseClick()
    {
      cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      cameraRaycaster.notifyMouseClickObservers += OnTargetClicked;
    }

    void PutWeaponInHand()
    {
      var weaponPrefab = weaponInUse.GetWeaponPrefab();
      GameObject dominantHand = RequestDominantHand();
      GameObject weapon = Instantiate(weaponPrefab, dominantHand.transform) as GameObject;
      weapon.transform.localPosition = weaponInUse.girpTransform.localPosition;
      weapon.transform.localRotation = weaponInUse.girpTransform.localRotation;
    }

    GameObject RequestDominantHand()
    {
      var dominantHands = GetComponentsInChildren<DominantHand>();
      int numberOfDominantHands = dominantHands.Length;
      Assert.AreNotEqual(numberOfDominantHands, 0, "No Dominant Hand");
      Assert.IsFalse(numberOfDominantHands > 1,   "Multiple domainant hand scripts on player");
      return dominantHands[0].gameObject;
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

    private void OnTargetClicked(RaycastHit hit, int layerHit) 
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
      print("attack");
      var targetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
      if (targetDistance > attackRadius)
      {
        print("too far to attack");
        return;
      }

      if (Time.time - lastHitTime > attackCooldown)
      {
        print("attacked");
        (currentTarget.GetComponent(typeof(IDamageable)) as IDamageable).TakeDamage(attackDamage);
        lastHitTime = Time.time;
        // Anima
      }
    }
  }
}