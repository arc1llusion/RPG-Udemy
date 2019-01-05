using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(HealthSystem))]
    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath = null;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] float waypointWaitTime = 0.5f;

        Character character;

        enum State { Attacking, Idling, Chasing, Patrolling }
        State state = State.Idling;

        PlayerControl player = null;
        float currentWeaponRange = 4f;

        float distanceToPlayer = 0f;
        int nextWaypointIndex;

        void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();

        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            var ws = GetComponent<WeaponSystem>();
            currentWeaponRange = ws.GetCurrentWeapon().GetMaxAttackRange();

            if (distanceToPlayer > chaseRadius && state != State.Patrolling)
            {
                StopAllCoroutines();
                ws.StopAttacking();
                StartCoroutine(Patrol());
            }
            if (distanceToPlayer <= chaseRadius && state != State.Chasing)
            {
                StopAllCoroutines();
                ws.StopAttacking();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.Attacking)
            {
                StopAllCoroutines();
                ws.AttackTarget(player.gameObject);
            }
        }

        IEnumerator Patrol()
        {
            state = State.Patrolling;

            while(patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(waypointWaitTime);
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.Chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
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