using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI; // TODO consider re-wiring

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {

        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Audio")]
        [Range(0f, 1f)]
        [SerializeField] float spatialBlend = 0.5f;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 capsuleCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float capsuleRadius = 0.2f;
        [SerializeField] float capsuleHeight = 2.03f;

        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float animSpeedMultiplier = 1.5f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;

        [Header("Nav Mesh Agent")]
        [SerializeField] float navMeshSteeringSpeed = 1.0f;
        [SerializeField] float navMeshStoppingDistance = 1.3f;


        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        Rigidbody rigidBody = null;
        float turnAmount;
        float forwardAmount;
        Vector3 groundNormal;

        bool isAlive = true;

        private void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = capsuleCenter;
            capsuleCollider.height = capsuleHeight;
            capsuleCollider.radius = capsuleRadius;

            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = spatialBlend;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();

            navMeshAgent.speed = navMeshSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshStoppingDistance;

            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = false;
            navMeshAgent.autoBraking = false;
        }

        public void Kill()
        {
            isAlive = false;
        }

        private void Update()
        {
            if (isAlive && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
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

        public void SetDestination(Vector3 worldPosition)
        {
            navMeshAgent.destination = worldPosition;
        }
        
        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        private void Move(Vector3 movement)
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