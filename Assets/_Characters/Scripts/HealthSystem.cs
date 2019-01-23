using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{

    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] float deathVanishSeconds = 2f;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthPoolHolder;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        const string DEATH_TRIGGER = "Death";

        AudioSource audioSource;
        Animator animator;
        Character character;
        float currentHealthPoints;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            character = GetComponent<Character>();
            SetUpCurrentHealth();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateHealthPool();
        }

        void UpdateHealthPool()
        {
            healthPoolHolder.fillAmount = healthAsPercentage;
        }

        public void TakeDamage(float damage)
        {
            bool isDead = currentHealthPoints - damage <= 0;
            ReduceHealth(damage);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);

            if (isDead) StartCoroutine(KillCharacter());
        }

        private void ReduceHealth(float damage)
        {
            // Mathf.Clamp - Only allows values between two floats (i.e. 0 and maxHealth)
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        private void SetUpCurrentHealth()
        {
            currentHealthPoints = maxHealthPoints;
        
        }

        IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            character.Kill();

            animator.SetTrigger(DEATH_TRIGGER);

            var player = GetComponent<Player>();
            if (player && player.isActiveAndEnabled)
            {
                audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                audioSource.Play();
                yield return new WaitForSecondsRealtime(audioSource.clip.length); // TODO: use audio clip lenght
                SceneManager.LoadScene(0);
            }
            else
            {
                // is enemy, do something to kill them
                Destroy(gameObject, deathVanishSeconds);
            }
        }

    }
}