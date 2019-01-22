using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{   
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;
        public Vector3 location;

        public AbilityUseParams(IDamageable target, float baseDamage, Vector3 location)
        {
            this.target = target;
            this.baseDamage = baseDamage;
            this.location = location;
        }
    }
    public abstract class SpecialAbility : ScriptableObject
    {
        [Header("Speical Ability General")]
        [SerializeField] float manaCost = 0f;
        [SerializeField] GameObject particalPrefab = null;

        protected ISpecialAbility behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams useParams)
        {
            behaviour.Use(useParams);
        }

        public float ManaCost()
        {
            return manaCost;
        }

        public GameObject GetParticalPrefab()
        {
            return particalPrefab;
        }
    }

    public interface ISpecialAbility {
        void Use(AbilityUseParams useParams);
        float ManaCost(); 
    }
}
