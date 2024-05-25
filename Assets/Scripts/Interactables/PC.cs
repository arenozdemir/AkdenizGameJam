using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PC : MonoBehaviour, InterfaceInteractable
{
    float timer;

    public bool isActivated;
    public event Action makeNoise;

    private IEnumerator enumerator;
    private void Awake()
    {
        enumerator = ActivatePC();
        timer = 0f;
    }
    public void Interact()
    {
        StartCoroutine(enumerator);
    }
    public void OutInteract()
    {
        StopCoroutine(enumerator);
    }
    private IEnumerator ActivatePC()
    {
        makeNoise?.Invoke();
        while (timer <= 20)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }
        isActivated = true;
        StopCoroutine(enumerator);
    }
}

public interface InterfaceInteractable
{
    public void Interact();
    public void OutInteract();
}