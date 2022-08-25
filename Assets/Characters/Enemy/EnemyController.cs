using Characters.Animation;
using Characters.Components;
using Characters.Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Characters.Enemy
{
    public sealed class EnemyController : EntityController
    {
        public static EntityNotify OnKill;
        public event EntityNotify OnAttack;
        public event EntityNotify OnDeath;

        [Header("References")]
        [SerializeField] private HitAreaComponent hitArea;
        [SerializeField] private AnimationEventListener animationEvent;

        [Header("Layers")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask targetLayer;

        [Header("Stats")]
        [SerializeField] private float walkPointRange;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float sightRange;
        [SerializeField] private float attackRange;

        public float Velocity => _agent.velocity.z;

        private NavMeshAgent _agent;
        private Transform _target;

        // Patrolling
        private Vector3 _walkPoint;
        private bool _walkPointSet;

        // Attacking
        private bool _alreadyAttacked;

        // States
        private bool _isTargetDead;
        private bool _targetInSightRange;
        private bool _targetInAttackRange;

        private void Awake()
        {
            Health = GetComponent<HealthComponent>();

            var player = FindObjectOfType<PlayerController>();
            player.OnDeath += () => { _isTargetDead = true; };

            _target = player.transform;
            _agent = GetComponent<NavMeshAgent>();

            animationEvent.OnAttackAnimationEnd += () =>
            {
                hitArea.gameObject.SetActive(false);
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            };
            animationEvent.OnDeathAnimationEnd += () => Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (_isTargetDead) return;

            var position = transform.position;
            _targetInSightRange = Physics.CheckSphere(position, sightRange, targetLayer);
            _targetInAttackRange = Physics.CheckSphere(position, attackRange, targetLayer);

            if (_targetInAttackRange)
                _agent.SetDestination(transform.position);
        }

        private void Update()
        {
            if (!_targetInSightRange && !_targetInAttackRange) Patrol();

            if (_isTargetDead) return;

            if (_targetInSightRange && !_targetInAttackRange) Chase();
            if (_targetInAttackRange && _targetInSightRange) Attack();
        }

        public override void TakeDamage(uint damageAmount)
        {
            base.TakeDamage(damageAmount);
            if (Health.IsAlive || !enabled) return;

            OnDeath?.Invoke();
            OnKill?.Invoke();

            enabled = false;
        }

        private void Patrol()
        {
            if (!_walkPointSet) SearchWalkPoint();

            _agent.SetDestination(_walkPoint);

            var distanceToWalkPoint = transform.position - _walkPoint;
            if (distanceToWalkPoint.magnitude < 1f)
                _walkPointSet = false;
        }

        private void SearchWalkPoint()
        {
            var currentTransform = transform;
            var position = currentTransform.position;

            var randomZ = Random.Range(-walkPointRange, walkPointRange);
            var randomX = Random.Range(-walkPointRange, walkPointRange);

            _walkPoint = new Vector3(position.x + randomX, position.y,
                position.z + randomZ);

            var hitResult = Physics.Raycast(_walkPoint, -currentTransform.up, 2f, groundLayer);
            if (hitResult)
                _walkPointSet = true;
        }

        private void Chase()
        {
            _alreadyAttacked = false;
            _agent.SetDestination(_target.position);
        }

        private void Attack()
        {
            var currentTransform = transform;
            var currentRotation = currentTransform.rotation;

            currentTransform.LookAt(_target);

            var newRotation = currentTransform.rotation;
            currentTransform.rotation = currentRotation;

            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, moveSpeed * 2.0f * Time.deltaTime);

            if (_alreadyAttacked) return;

            hitArea.gameObject.SetActive(true);
            OnAttack?.Invoke();
            _alreadyAttacked = true;
        }

        private void ResetAttack()
        {
            _alreadyAttacked = false;
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, sightRange);
        }
    }
}