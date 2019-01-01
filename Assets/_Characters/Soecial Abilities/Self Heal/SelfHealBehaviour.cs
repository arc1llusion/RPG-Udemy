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
            abilityParams.caster.Heal((config as SelfHealConfig).GetHealAmount());
            PlayParticleEffect();
            PlayAbilitySound();
        }
    }
}