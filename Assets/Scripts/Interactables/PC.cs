using System;
using System.Collections;
using UnityEngine;

public class PC : MonoBehaviour, InterfaceInteractable
{
    float timer;

    public bool isActivated;
    public static event Action makeNoise; // makeNoise event'ini static yaparak tüm PC'lerin paylaþmasýný saðlýyoruz.

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
        makeNoise?.Invoke(); // makeNoise event'ini tetikle
        timer = 0f; // Timer'ý sýfýrla

        while (timer <= 20)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }
        isActivated = true; // Aktivasyon tamamlandý
    }
}

public interface InterfaceInteractable
{
    void Interact();
    void OutInteract();
}
