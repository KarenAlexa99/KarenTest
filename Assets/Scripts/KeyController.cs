using System.Collections;
using System;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public Action OnGetKey;
    public Places typeOfPlaceOpen;
    public bool isObtained { get; set; }

    private void Awake()
    {
        isObtained = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (OnGetKey != null)
            {
                OnGetKey?.Invoke();
                isObtained = true;
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
