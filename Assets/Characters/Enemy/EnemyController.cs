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
        public event EntityNotify OnAttack;
        public event EntityNotify OnDeath; // Subscribe to the player and heal him?
        public static EntityNotify OnKill;

        [Header("References")] [SerializeField]
        private HitAreaComponent hitArea;

        [SerializeField] private AnimationEventListener animationEvent;

        // Layers
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask playerLayer;

        [Header("Stats")] [SerializeField] private float walkPointRange;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float sightRange;
        [SerializeField] private float attackRange;

        public float Velocity => _agent.velocity.z;

        private NavMeshAgent _agent;
        private Transform _player;

        // Patrolling
        private Vector3 _walkPoint;
        private bool _walkPointSet;

        // Attacking
        private bool _alreadyAttacked;

        // States
        private bool _playerInSightRange;
        private bool _playerInAttackRange;

        private void Awake()
        {
            Health = GetComponent<HealthComponent>();

            _player = FindObjectOfType<PlayerController>().transform;
            _agent = GetComponent<NavMeshAgent>();

            animationEvent.OnAttackAnimationEnd += () => hitArea.gameObject.SetActive(false);
            animationEvent.OnDeathAnimationEnd += () => Destroy(gameObject);
        }

        private void Update()
        {
            var position = transform.position;
            _playerInSightRange = Physics.CheckSphere(position, sightRange, playerLayer);
            _playerInAttackRange = Physics.CheckSphere(position, attackRange, playerLayer);

            if (!_playerInSightRange && !_playerInAttackRange) Patrolling();
            if (_playerInSightRange && !_playerInAttackRange) ChasePlayer();
            if (_playerInAttackRange && _playerInSightRange) AttackPlayer();
        }

        public override void TakeDamage(uint damageAmount)
        {
            base.TakeDamage(damageAmount);
            if (Health.IsAlive) return;

            OnDeath?.Invoke();
            OnKill?.Invoke();
            
            enabled = false;
        }

        private void Patrolling()
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

        private void ChasePlayer()
        {
            _agent.SetDestination(_player.position);
        }

        private void AttackPlayer()
        {
            _agent.SetDestination(transform.position);
            // transform.LookAt(_player); // TODO: Adopt it.

            if (_alreadyAttacked) return;

            hitArea.gameObject.SetActive(true);
            OnAttack?.Invoke();
            _alreadyAttacked = true;

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        private void ResetAttack()
        {
            _alreadyAttacked = false;
        }

        // TODO: Don't forget to remove.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
}