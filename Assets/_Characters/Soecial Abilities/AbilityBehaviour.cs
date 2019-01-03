﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBeheviour : MonoBehaviour
    {
        protected AbilityConfig config = null;

        const float PARTICLE_CLEAN_UP_DELAY = 20F;

        void Start()
        {
            
        }

        public void SetConfig(AbilityConfig attackConfig)
        {
            config = attackConfig;
        }

        protected void PlayParticleEffect()
        {
            if (config.GetParticlePrefab() != null)
            {
                var particlePrefab = config.GetParticlePrefab();
                var particleObject = Instantiate(config.GetParticlePrefab(), transform.position, particlePrefab.transform.rotation);

                particleObject.transform.parent = transform;
                particleObject.GetComponent<ParticleSystem>().Play();
                StartCoroutine(DestroyParticleWhenFinished(particleObject));
            }
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while(particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }

            Destroy(particlePrefab.gameObject);

            yield return new WaitForEndOfFrame();
        }

        protected void PlayAbilitySound()
        {
            if (config.GetRandomAbilitySound() != null)
            {
                var abilitySound = config.GetRandomAbilitySound();
                var audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(abilitySound);
            }
        }

        public abstract void Use(GameObject target = null);
    }
}