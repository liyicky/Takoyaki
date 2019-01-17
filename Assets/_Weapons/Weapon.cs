using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapon
{
  [CreateAssetMenu(menuName=("RPG/Weapon"))]

  public class Weapon : ScriptableObject
  {
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] AnimationClip attackAnimation;

    public Transform girpTransform;

    public GameObject GetWeaponPrefab()
    {
      return weaponPrefab;
    }

    public AnimationClip GetAttackAnimClip()
    {
      return attackAnimation;
    }
  }
}
