using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action OnTakeDamage;
    public Action OnGameOver;

    [Header("Movement")]
    [SerializeField] private float playerSpeed = 2.0f;
    private Vector3 move;
    private Camera mainCamera;
    private Animator anim;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100;

    [Header("VFX")]
    [SerializeField] private ParticleSystem shootPs;
    [SerializeField] private ParticleSystem Blood;

    [Header("SoundFX")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip walkClip;
    public float curHealth { get; set; }
    public float maxiHealth { get; set; }

    private void Awake()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
        maxiHealth = maxHealth;
        curHealth = maxHealth;
    }

    /// <summary>
    /// Shooting and movement is initialized for the game manager
    /// </summary>
    public void Initialized()
    {
        PlayerShooting();
        Movement();
    }

    private void Movement()
    {
        //Character movement
        move = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("vertical", Input.GetAxis("Vertical"));
        transform.Translate(move * Time.deltaTime * playerSpeed, Space.World);

        //Character rotation
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out var hitInfo, Mathf.Infinity);
        var pos = hitInfo.point;
        var direction = pos - transform.position;
        direction.y = 0;
        transform.forward = direction;
    }

    public void PlayerShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("isShooting", true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.SetBool("isShooting", false);
        }
    }

    /// <summary>
    /// Is use for animation events
    /// </summary>
    private void Shoot()
    {
        shootPs.Play();
        SoundManager.Instance.PlaySound(shootClip, 0.3f);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    /// <summary>
    /// Is use for animation events
    /// </summary>
    private void SetWalkSound()
    {
        SoundManager.Instance.PlaySound(walkClip, 0.05f);
    }

    /// <summary>
    /// Damage that received 
    /// </summary>

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            curHealth -= other.GetComponent<BulletController>().Damage;
            Blood.Play();

            if (curHealth <= 0)
            {
                if (OnGameOver != null)
                    OnGameOver?.Invoke();
            }

            if (OnTakeDamage != null)
                OnTakeDamage?.Invoke();
        }
    }
}
