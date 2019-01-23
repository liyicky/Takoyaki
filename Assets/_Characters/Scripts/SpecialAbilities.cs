using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{    
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] SpecialAbility[] abilities;
        [SerializeField] float maxManaPoints = 100f;
        [SerializeField] float manaRegenRate = 1f;
        [SerializeField] Image manaPoolHolder;

        public float manaAsPercentage { get { return currentManaPoints / maxManaPoints; } }

        AudioSource audioSource;
        float currentManaPoints;

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentManaPoints = maxManaPoints;
            AttachInitialAbilities();
        }

        // Update is called once per frame
        void Update()
        {
            RegenMana();
            UpdateManaPoolImage();
        }

        private void RegenMana()
        {
            float calculatedManaRegen = (0.01f * manaRegenRate) * currentManaPoints;
            currentManaPoints = Mathf.Clamp(calculatedManaRegen + currentManaPoints, 0f, maxManaPoints);
        }

        void UpdateManaPoolImage()
        {
            manaPoolHolder.fillAmount = manaAsPercentage;
        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public void AttemptSpecialAbility(int abilityIndex)
        {
            float manaCost = abilities[abilityIndex].ManaCost();
            if (manaCost <= currentManaPoints)
            {
                ConsumeMana(manaCost);
            }
            else
            {
                //TODO: play no energy sound
            }
        }

        private void ConsumeMana(float mana)
        {
            currentManaPoints -= mana;
        }
    }
}
