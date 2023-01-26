using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float turnSpeed = 0.5f;

    private CharacterController controller;
    private Animator anim;
    private Vector3 playerVelocity;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireTime;

    private void Start()
    {
        //poner en un initialize
        controller = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();

        //game manager con un check
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        PlayerShooting();
    }

    public void Move()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));
            anim.SetFloat("vertical", Input.GetAxis("Vertical"));
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        float mouseRotation = Input.GetAxis("Mouse X") * turnSpeed;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + mouseRotation, 0);
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
        bulletRb.AddForce(bulletSpawnPoint.up * bulletSpeed, ForceMode.Impulse);
    }

}
