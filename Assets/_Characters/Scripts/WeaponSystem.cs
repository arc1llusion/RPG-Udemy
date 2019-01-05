using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;

        private const string ATTACK_TRIGGER = "Attack";
        private const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        Character character;
        Animator animator;

        GameObject target;
        GameObject weaponObject;
        float lastHitTime = 0f;

        void Start()
        {
            character = GetComponent<Character>();
            animator = GetComponent<Animator>();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }

        void Update()
        {
            bool targetIsDead = false;
            bool targetIsOutOfRange = false;

            if(target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                targetIsDead = target.GetComponent<HealthSystem>().healthAsPercentage <= Mathf.Epsilon;
                targetIsOutOfRange = Vector3.Distance(target.transform.position, transform.position) >= currentWeaponConfig.GetMaxAttackRange();
            }

            float characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = characterHealth <= Mathf.Epsilon;

            if(characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }
        }

        public void AttackTarget(GameObject target)
        {
            this.target = target;
            StartCoroutine(AttackTargetRepeatedly());
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public void StopAttacking()
        {
            StopAllCoroutines();
        }

        IEnumerator AttackTargetRepeatedly()
        {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            while(attackerStillAlive && targetStillAlive)
            {
                float weaponHitPeriod = currentWeaponConfig.GetMinTimeBetweenHits();
                float timeToWait = character.GetAnimationSpeedMultiplier();

                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if(isTimeToHitAgain)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }

                yield return new WaitForSeconds(timeToWait);
            }
        }

        private void AttackTargetOnce()
        {
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = currentWeaponConfig.GetDamageDelay();

            SetAttackAnimation();
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float damageDelay)
        {
            yield return new WaitForSecondsRealtime(damageDelay);

            var hs = target.GetComponent<HealthSystem>();
            hs.TakeDamage(CalculateDamage());
        }

        private void SetAttackAnimation()
        {
            if (!character.GetOverrideController())
            {
                Debug.Break();
                Debug.LogAssertion("Please provide " + gameObject + " with an animator override controller");
            }
            else
            {
                var animatorOverrideController = character.GetOverrideController();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
            }
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = currentWeaponConfig.GetWeaponPrefab();

            GameObject dominantHand = RequestDominantHand();
            if (weaponObject != null)
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
        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            float damageBeforeCritical = (baseDamage + currentWeaponConfig.GetAdditionalDamage());
            return damageBeforeCritical;
        }

    }

}