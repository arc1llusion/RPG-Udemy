using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Characters
{
    public class AreaEffectBehavior : AbilityBeheviour
    {
        public override void Use(GameObject target)
        {
            DealingRadialDamage();
            PlayParticleEffect();
            PlayAbilitySound();
        }

        private void DealingRadialDamage()
        {
            var hits = Physics.SphereCastAll(transform.position, (config as AreaEffectConfig).GetRadius(), Vector3.up, (config as AreaEffectConfig).GetRadius());

            foreach (var hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
                var player = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && player == null)
                {
                    var enemy = hit.collider.gameObject.GetComponent<HealthSystem>();
                    enemy.TakeDamage((config as AreaEffectConfig).GetDamageToEachTarget());
                }
            }
        }
    }
}