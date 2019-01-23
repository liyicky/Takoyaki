using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{   
    public abstract class SpecialAbility : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float manaCost = 0f;
        [SerializeField] GameObject particalPrefab = null;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehaviour behaviour;

        protected abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToAttachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToAttachTo);
            behaviourComponent.SetAbility(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        public float ManaCost()
        {
            return manaCost;
        }

        public GameObject GetParticalPrefab()
        {
            return particalPrefab;
        }

        public AudioClip RandomAudioClip()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}
