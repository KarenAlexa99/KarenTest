using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private GameObject[] keysImg;
    private int collectedKeys = 0;

    private void Awake()
    {
        DisableAllKeys();
    }

    private void DisableAllKeys()
    {
        for (int i = 0; i < keysImg.Length; i++)
        {
            keysImg[i].SetActive(false);
        }
    }

    public void GetKey()
    {
        if(collectedKeys < keysImg.Length)
        {
            LeanTween.scale(keysImg[collectedKeys].gameObject, Vector3.one, 1).setEaseInOutElastic();
            keysImg[collectedKeys].SetActive(true);
            collectedKeys += 1;
        }
    }
}
