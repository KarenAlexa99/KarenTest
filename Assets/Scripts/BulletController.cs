using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
