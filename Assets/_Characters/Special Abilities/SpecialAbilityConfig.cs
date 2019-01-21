using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{    
    public abstract class SpecialAbilityConfig : ScriptableObject
    {
        [Header("Speical Ability General")]
        [SerializeField] float energyCost = 0f;

        abstract public ISpecialAbility AddComponent(GameObject gameObjectToAttachTo);
    }
}
