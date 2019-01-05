using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBeheviour : AbilityBeheviour
    {
        public override void Use(GameObject target)
        {
            PlayAbilitySound();

            var playerHealth = GetComponent<PlayerControl>().GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetHealAmount());

            PlayParticleEffect();
            PlayAbilityAnimation();
        }
    }
}