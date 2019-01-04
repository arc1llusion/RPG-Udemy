using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 6f;

        bool isAttacking = false;
        PlayerMovement player = null;
        float currentWeaponRange = 4f;

        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();

        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            var ws = GetComponent<WeaponSystem>();
            currentWeaponRange = ws.GetCurrentWeapon().GetMaxAttackRange();
        }

        void OnDrawGizmos()
        {
            // Draw attack sphere 
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}