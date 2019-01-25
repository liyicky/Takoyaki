using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickup : MonoBehaviour
    {

        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!Application.IsPlaying(this))
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other) {
            FindObjectOfType<WeaponSystem>().PutWeaponInHand(weaponConfig);
            GetComponent<AudioSource>().PlayOneShot(pickUpSFX);
        }
    }
}
