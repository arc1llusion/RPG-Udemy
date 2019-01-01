using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class BaseBehaviour<T> : MonoBehaviour, ISpecialAbility where T: SpecialAbility
    {
        protected T config = default(T);

        public void SetConfig(T attackConfig)
        {
            config = attackConfig;
        }

        protected void PlayParticleEffect()
        {
            var ps = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            var psComponent = ps.GetComponent<ParticleSystem>();
            psComponent.Play();
            Destroy(psComponent, psComponent.main.duration);
        }

        public abstract void Use(AbilityUseParams abilityParams);
    }
}