using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : MonoBehaviour
{
    [SerializeField] List<PC> pCs;

    int count;
    private void Awake()
    {
        //FindObjectOfType<PC>().PCActivated += UnLockProcess;
    }

    private void UnLockProcess()
    {
        if (count == pCs.Count)
        {
            Debug.Log("Door is unlocked");
        }
        //light open
        Debug.Log(count);
        count++;  
    }
}
