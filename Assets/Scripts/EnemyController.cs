using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkRange = 10.0f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerMask;
    private NavMeshAgent agent;

    [Header("Attack")]
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    private Animator anim;

    [Header("States")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float maxLife;
    private float life;
    private bool playerInSightRange, playerInAttackRange;
    private bool enemyIsDie;

    [Header("VFX")]
    [SerializeField] private ParticleSystem shootPs;
    [SerializeField] private ParticleSystem bloodPs;

    [Header("SoundFX")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip damageClip;

    public bool alreadyAttacked { get; set; }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        life = maxLife;
        enemyIsDie = false;
    }

    /// <summary>
    /// Initialized the enemys states
    /// </summary>

    public void Initialized()
    {
        if (!enemyIsDie)
            CheckEnemyStates();
    }

    public void CheckEnemyStates()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange)
            EnemyRandomRunning();
        if (playerInSightRange && !playerInAttackRange)
            FollowPlayer();
        if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
    }

    /// <summary>
    /// random points for movement
    /// </summary>

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public void EnemyRandomRunning()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;

            if (RandomPoint(transform.position, walkRange, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
    }

    private void FollowPlayer()
    {
        agent.SetDestination(player.position);
        anim.SetBool("isRunning", true);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player.localPosition);

        if (!alreadyAttacked)
        {
            anim.SetBool("isShooting", true);
            shootPs.Play();
            SoundManager.Instance.PlaySound(shootClip, 0.2f);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        anim.SetBool("isShooting", false);
        alreadyAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            TakeDamage(other.GetComponent<BulletController>().Damage);
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        bloodPs.Play();
        SoundManager.Instance.PlaySound(damageClip,0.08f);

        if (life <= 0)
        {
            enemyIsDie = true;
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
