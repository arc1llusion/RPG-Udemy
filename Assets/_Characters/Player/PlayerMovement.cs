using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wire...
using RPG.CameraUI;
using System;

namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(.1f, 1.0f)]
        [SerializeField]
        float criticalHitChance = 0.1f;

        [SerializeField]
        float criticalHitMultiplier = 1.25f;

        [SerializeField]
        ParticleSystem criticalHitParticleSystem = null;


        Enemy currentEnemy = null;
        CameraRaycaster cameraRaycaster;
        SpecialAbilities energy;
        SpecialAbilities abilities;
        Character character;
        WeaponSystem weaponSystem;


        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();

            RegisterForMouseEvents();
        }

        void Update()
        {
            ScanForSpecialAbilityKeyDown();
        }

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        private void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            currentEnemy = enemy;

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0, currentEnemy.gameObject);
            }
        }

        private void ScanForSpecialAbilityKeyDown()
        {
            for (int i = 1; i < abilities.GetNumberOfAbilities(); ++i)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    abilities.AttemptSpecialAbility(i, currentEnemy == null ? null : currentEnemy.gameObject);
                }
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}