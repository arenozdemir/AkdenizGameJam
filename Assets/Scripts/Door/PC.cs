using System.Collections;
using UnityEngine;
using UnityEngine.WSA;

public class PC : MonoBehaviour
{
    public float activation;
    public bool isActivated;
    private IEnumerator enumerator;
    private void Awake()
    {
        FindObjectOfType<PlayerController>().OnInteract += Interact;
        FindObjectOfType<PlayerController>().OutInteract += OutInteract;

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
        while(activation <= 30)
        {
            if (activation >= 30)
            {
                isActivated = true;
                StopCoroutine(enumerator);
                break;
            }
            activation += 0.1f;
            Debug.Log(activation);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
