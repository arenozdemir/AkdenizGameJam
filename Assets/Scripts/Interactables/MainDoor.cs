using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainDoor : MonoBehaviour
{
    [SerializeField] List<PC> pCs;

    int count;
    private void Awake()
    {
        PC.pcActivated += UnLockProcess;
    }

    private void UnLockProcess()
    {
        if (count == pCs.Count)
        {
            Debug.Log("Door is unlocked");
            GetComponent<Animator>().SetBool("isDoorOpen", true);
        }
        count++;  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            
        }
    }
}
