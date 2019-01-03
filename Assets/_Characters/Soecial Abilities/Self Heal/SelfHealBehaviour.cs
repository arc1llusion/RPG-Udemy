using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBeheviour : AbilityBeheviour
    {
        public override void Use(AbilityUseParams abilityParams)
        {
            PlayAbilitySound();
            //abilityParams.caster.Heal((config as SelfHealConfig).GetHealAmount());

            var playerHealth = GetComponent<Player>().GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetHealAmount());

            PlayParticleEffect();
        }
    }
}