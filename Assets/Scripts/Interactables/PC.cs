using System;
using System.Collections;
using UnityEngine;

public class PC : MonoBehaviour, InterfaceInteractable
{
    float timer;
    private bool madeNoise;

    public bool isActivated;
    public static event Action makeNoise; // makeNoise event'ini static yaparak t�m PC'lerin payla�mas�n� sa�l�yoruz.
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
        // OutInteract �a�r�ld���nda coroutini durdurmak i�in kullan�l�r.
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
        isActivated = true; // Aktivasyon tamamland�
        light.SetActive(true);
        pcActivated?.Invoke();
    }
}

public interface InterfaceInteractable
{
    void Interact();
    void OutInteract();
}
