using System;
using System.Collections;
using UnityEngine;

public class PC : MonoBehaviour, InterfaceInteractable
{
    float timer;

    public bool isActivated;
    public static event Action makeNoise; // makeNoise event'ini static yaparak t�m PC'lerin payla�mas�n� sa�l�yoruz.

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
        makeNoise?.Invoke(); // makeNoise event'ini tetikle
        timer = 0f; // Timer'� s�f�rla

        while (timer <= 20)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }
        isActivated = true; // Aktivasyon tamamland�
    }
}

public interface InterfaceInteractable
{
    void Interact();
    void OutInteract();
}
