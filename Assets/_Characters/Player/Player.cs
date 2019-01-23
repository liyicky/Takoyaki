using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.CameraUI;
using RPG.Core;
using System;

namespace RPG.Characters
{
  [RequireComponent(typeof(CameraRaycaster))]

  public class Player : MonoBehaviour, IDamageable
  {
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float maxManaPoints = 100f;
    [SerializeField] float manaRegenRate = 1f;
    [SerializeField] float baseDamage = 10f;
    [SerializeField] Weapon weaponInUse;
    [SerializeField] AnimatorOverrideController animatorOverrideController;
    [SerializeField] AudioClip[] damageSounds;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] SpecialAbility[] abilities;
    [Range(.01f, 1.0f)] [SerializeField] float criticalHitChance = .01f;
    [SerializeField] float criticalHitMultiplayer = 2f;
    [SerializeField] ParticleSystem critParticleSystem;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }
    public float manaAsPercentage { get { return currentManaPoints / maxManaPoints; } }

    const string DEATH_TRIGGER = "Death";
    const string ATTACK_TRIGGER = "Attack";

    CameraRaycaster cameraRaycaster;
    GameObject currentTarget;
    GameObject currentWeapon;
    float currentHealthPoints;
    float currentManaPoints;
    float lastHitTime = 1f;

    AudioSource audioSource;
    Animator animator;

    public void PutWeaponInHand(Weapon weaponConfig)
    {
      weaponInUse = weaponConfig;
      GameObject dominantHand = RequestDominantHand();
      Destroy(currentWeapon);
      currentWeapon = Instantiate(weaponInUse.GetWeaponPrefab(), dominantHand.transform) as GameObject;
      currentWeapon.transform.localPosition = weaponInUse.girpTransform.localPosition;
      currentWeapon.transform.localRotation = weaponInUse.girpTransform.localRotation;
    }

    public void TakeDamage(float damage)
    {
      bool isDead = currentHealthPoints - damage <= 0;
      ReduceHealth(damage);
      audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
      audioSource.Play();

      if (isDead) StartCoroutine(KillPlayer());
    }

    IEnumerator KillPlayer()
    {
      audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
      audioSource.Play();
      animator.SetTrigger(DEATH_TRIGGER);
      yield return new WaitForSecondsRealtime(audioSource.clip.length); // TODO: use audio clip lenght
      SceneManager.LoadScene(0);
    }

    // Start is called before the first frame update
    private void Start()
    {
      SetCurrentPoints();
      SetupMouseClick();
      PutWeaponInHand(weaponInUse);
      SetAttackAnimation();
      AttachInitialAbilities();
      audioSource = GetComponent<AudioSource>();
      animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    private void Update()
    {
      RegenMana();

      if (currentHealthPoints > Mathf.Epsilon)
      {
        ScanForAbilityKeyDown();
      }
    }

    private void AttachInitialAbilities()
    {
      for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
      {
        abilities[abilityIndex].AttachAbilityTo(gameObject);
      }
    }

    private void ReduceHealth(float damage)
    {
      // Mathf.Clamp - Only allows values between two floats (i.e. 0 and maxHealth)
      currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

    private void SetCurrentPoints()
    {
      currentHealthPoints = maxHealthPoints;
      currentManaPoints = maxManaPoints;
    }

    private void SetAttackAnimation()
    {
      var animator = GetComponent<Animator>();
      animator.runtimeAnimatorController = animatorOverrideController;
      animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO: remove const
    }

    private void SetupMouseClick()
    {
      cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      cameraRaycaster.onMouseOverEnemy += ProcessEnemyInteraction;
    }

    private void ProcessEnemyInteraction(Enemy enemy)
    {
      currentTarget = enemy.gameObject;
      if (Input.GetMouseButtonDown(0))
      {
        Attack();
      }
      else if (Input.GetMouseButtonDown(1))
      {
        AttemptSpecialAbility(2);
      }
    }

    private GameObject RequestDominantHand()
    {
      var dominantHands = GetComponentsInChildren<DominantHand>();
      int numberOfDominantHands = dominantHands.Length;
      Assert.AreNotEqual(numberOfDominantHands, 0, "No Dominant Hand");
      Assert.IsFalse(numberOfDominantHands > 1, "Multiple domainant hand scripts on player");
      return dominantHands[0].gameObject;
    }
    
    private void RegenMana()
    {
      float calculatedManaRegen = (0.01f * manaRegenRate) * currentManaPoints;
      currentManaPoints = Mathf.Clamp(calculatedManaRegen + currentManaPoints, 0f, maxManaPoints);
    }

    private float CalculateDamage()
    {
      float damageBeforeCrit = baseDamage + weaponInUse.WeaponDamage();
      bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
      if (isCriticalHit)
      {
        critParticleSystem.Play();
        return damageBeforeCrit * criticalHitMultiplayer;
      }
      return damageBeforeCrit;
    }

    private void Attack()
    {            
      var targetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
      if (targetDistance > weaponInUse.AttackRadius()) return;
      if (Time.time - lastHitTime > weaponInUse.AttackCooldown())
      {
        (currentTarget.GetComponent(typeof(IDamageable)) as IDamageable).TakeDamage(CalculateDamage());
        lastHitTime = Time.time;
        AttackAnimation();
      }
    }

    void ScanForAbilityKeyDown()
    {
      if (Input.GetKeyDown("1"))
      {
        AttemptSpecialAbility(0);
      }
      else if (Input.GetKeyDown("2"))
      {
        currentTarget = gameObject;
        AttemptSpecialAbility(1);
      }
    }
    
    private void AttemptSpecialAbility(int abilityIndex)
    {  
      float manaCost = abilities[abilityIndex].ManaCost();
      if (ManaAvailable(manaCost))
      {
        var tar = currentTarget.GetComponent(typeof(IDamageable)) as IDamageable;
        var abilityParams = new AbilityUseParams(tar, baseDamage, currentTarget.transform.position);
        abilities[abilityIndex].Use(abilityParams);
        currentManaPoints -= manaCost;      
      }
    }

    private void AttackAnimation()
    {
      SetAttackAnimation();
      animator.SetTrigger(ATTACK_TRIGGER);
    }

    private bool ManaAvailable(float manaCost)
    {
      return currentManaPoints >= manaCost;
    }
  }
}