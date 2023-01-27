using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceController : MonoBehaviour
{
    [SerializeField] private CanvasGroup lockImg;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject roof;
    [SerializeField] private AudioClip doorClip;
    public bool isOpen { get; set; }
    public Places typeOfPlace;

    private void Awake()
    {
        door.SetActive(true);
        lockImg.alpha = 0;
        roof.SetActive(true);
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (isOpen)
            {
                SoundManager.Instance.PlaySound(doorClip, 0.3f);
                door.SetActive(false);
                roof.SetActive(false);
                lockImg.alpha = 0;
            }
            else
            {
                lockImg.alpha = 1;
                roof.SetActive(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            roof.SetActive(true);
            lockImg.alpha = 0;
        }
    }
}
