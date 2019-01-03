using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI; // TODO consider re-wiring

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float animSpeedMultiplier = 1.5f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;

        Vector3 clickPoint;
        NavMeshAgent agent = null;
        Animator animator = null;
        Rigidbody rigidBody = null;
        float turnAmount;
        float forwardAmount;
        Vector3 groundNormal;

        void Start()
        {
            var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            agent = GetComponent<NavMeshAgent>();

            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            agent.updatePosition = true;
            agent.updateRotation = false;
            agent.stoppingDistance = stoppingDistance;

            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        public void Kill()
        {

        }

        private void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                agent.SetDestination(enemy.transform.position);
            }
        }


        private void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }
        }

        public void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);

            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        private void SetForwardAndTurn(Vector3 movement)
        {
            if (movement.magnitude > 1f)
            {
                movement.Normalize();
            }

            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }
}