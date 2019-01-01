using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class BaseBehaviour<T> : MonoBehaviour, ISpecialAbility where T: AbilityConfig
    {
        protected T config = default(T);

        private AudioSource audioSource = null;
        protected AudioSource AudioSource {  get { return audioSource; } }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig(T attackConfig)
        {
            config = attackConfig;
        }

        protected void PlayParticleEffect(bool parentToCaster = false)
        {
            if (config.GetParticlePrefab() != null)
            {
                var ps = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);

                if(parentToCaster)
                {
                    ps.transform.parent = transform;
                }

                var psComponent = ps.GetComponent<ParticleSystem>();
                psComponent.Play();
                Destroy(psComponent, psComponent.main.duration);
            }
        }

        protected void PlayAudioClip()
        {
            if(config.GetAudioClip() != null)
            {
                AudioSource.clip = config.GetAudioClip();
                AudioSource.Play();
            }
        }

        public abstract void Use(AbilityUseParams abilityParams);
    }
}