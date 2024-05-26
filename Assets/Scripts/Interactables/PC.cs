using System;
using System.Collections;
using UnityEngine;

public class PC : MonoBehaviour, InterfaceInteractable
{
    float timer;
    private bool madeNoise;

    public bool isActivated;
    public static event Action makeNoise; // makeNoise event'ini static yaparak tüm PC'lerin paylaþmasýný saðlýyoruz.
    public static event Action pcActivated;

    public GameObject light;
    private void Awake()
    {
        timer = 0f;
    }

    public void Interact()
    {
        StartCoroutine(ActivatePC());
    }

    public void OutInteract()
    {
        // OutInteract çaðrýldýðýnda coroutini durdurmak için kullanýlýr.
        StopAllCoroutines();
    }

    private IEnumerator ActivatePC()
    {
        while (timer <= 20)
        {
            timer += Time.deltaTime;
            if (timer >= 10 && !madeNoise)
            {
                makeNoise?.Invoke();
                madeNoise = true;
            }
            yield return null;
        }
        isActivated = true; // Aktivasyon tamamlandý
        light.SetActive(true);
        pcActivated?.Invoke();
    }
}

public interface InterfaceInteractable
{
    void Interact();
    void OutInteract();
}
