using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected SpecialAbility ability;

        const float PARTICLE_CLEANUP_DELAY = 20f;

        public abstract void Use(GameObject target = null);

        public void SetAbility(SpecialAbility ability)
        {
            this.ability = ability;
        }

        protected void PlayParticleEffect()
        {
            GameObject particlePrefab = ability.GetParticalPrefab();
            GameObject particleObject = Instantiate(
                particlePrefab,
                transform.position,
                particlePrefab.transform.rotation
            );

            particleObject.transform.parent = transform;
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

        // TODO: AOE particles are not being destroyed
        IEnumerator DestroyParticleWhenFinished(GameObject target)
        {
            while (target.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEANUP_DELAY);
            }

            Destroy(target);
            yield return new WaitForEndOfFrame();
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = ability.RandomAudioClip();
            var audoSource = GetComponent<AudioSource>();
            audoSource.PlayOneShot(abilitySound);
        }
    }
}
