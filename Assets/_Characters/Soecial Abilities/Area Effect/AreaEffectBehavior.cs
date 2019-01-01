using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Characters
{
    public class AreaEffectBehavior : BaseBehaviour<AreaEffectConfig>
    {
        public override void Use(AbilityUseParams abilityParams)
        {
            DealingRadialDamage(abilityParams);
            PlayParticleEffect();
            PlayAudioClip();
        }

        private void DealingRadialDamage(AbilityUseParams abilityParams)
        {
            var hits = Physics.SphereCastAll(transform.position, config.GetRadius(), Vector3.up, config.GetRadius());

            var enemies = hits.Where(h =>
            {
                var component = h.collider.gameObject.GetComponent<IDamageable>();

                return component != null && component != abilityParams.caster;
            });

            foreach (var hit in enemies)
            {
                var enemy = hit.collider.gameObject.GetComponent<IDamageable>();
                enemy.TakeDamage(abilityParams.baseDamage + config.GetDamageToEachTarget());
            }
        }
    }
}