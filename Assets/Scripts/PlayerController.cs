using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float turnSpeed = 0.5f;
    [SerializeField] private Vector3 move;
    private Camera mainCamera;
    private CharacterController controller;
    private Animator anim;
    private Vector3 playerVelocity;
    private Rigidbody rb;
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireTime;
    private Vector3 position;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
        mainCamera = Camera.main;

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
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

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();  
        bulletRb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

}
