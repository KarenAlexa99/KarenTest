using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3;
    [SerializeField] private int damage;

    public int Damage { get => damage; }

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
