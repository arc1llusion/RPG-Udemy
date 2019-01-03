using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wire...
using RPG.CameraUI;
using RPG.Core;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    [RequireComponent(typeof(AudioSource))]
    public class Player : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon currentWeaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;

        [Range(.1f, 1.0f)]
        [SerializeField]
        float criticalHitChance = 0.1f;

        [SerializeField]
        float criticalHitMultiplier = 1.25f;

        [SerializeField]
        ParticleSystem criticalHitParticleSystem = null;

        
        const string ATTACK_TRIGGER = "Attack";
        private const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        Enemy currentEnemy = null;
        Animator animator;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;
        SpecialAbilities energy;
        GameObject weaponObject;
        SpecialAbilities abilities;
        

        void Start()
        {
            abilities = GetComponent<SpecialAbilities>();

            RegisterForMouseClick();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }

        void Update()
        {
            var healthPercentage = GetComponent<HealthSystem>().healthAsPercentage;
            if(healthPercentage > Mathf.Epsilon)
            {
                ScanForSpecialAbilityKeyDown();
            }
        }

        private void ScanForSpecialAbilityKeyDown()
        {
            for(int i = 1; i < abilities.GetNumberOfAbilities(); ++i)
            {
                if(Input.GetKeyDown(i.ToString()))
                {
                    abilities.AttemptSpecialAbility(i);
                }
            }
        }

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = currentWeaponConfig.GetWeaponPrefab();

            GameObject dominantHand = RequestDominantHand();
            if(weaponObject != null)
            {
                Destroy(weaponObject);
            }

            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            currentEnemy = enemy;

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                if (IsTargetInRange(enemy.gameObject))
                {
                    AttackTarget();
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
            }
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                currentEnemy.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = (baseDamage + currentWeaponConfig.GetAdditionalDamage());
            if (isCriticalHit)
            {
                criticalHitParticleSystem.Play();

                return damageBeforeCritical * criticalHitMultiplier;
            }

            return damageBeforeCritical;
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeaponConfig.GetMaxAttackRange();
        }
    }
}