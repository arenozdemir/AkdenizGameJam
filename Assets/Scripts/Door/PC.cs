using System;
using System.Collections;
using UnityEngine;

public class PC : MonoBehaviour
{
    public Action madeSound;
    public float activation;
    public bool isActivated;
    private IEnumerator enumerator;
    private void Awake()
    {
        enumerator = ActivatePC();
        activation = 0f;
    }
    public void Interact(PC pc)
    {
        if (pc == this)
        {
            StartCoroutine(enumerator);
        }
    }
    public void OutInteract(PC pc){
        if(pc == this){
            StopCoroutine(enumerator);
        }
    }
    private IEnumerator ActivatePC()
    {
        madeSound?.Invoke();
        while (activation <= 30)
        {
            if (activation >= 30)
            {
                isActivated = true;
                StopCoroutine(enumerator);
                break;
            }
            activation += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
