using UnityEngine;
using UnityEngine.SceneManagement;

public class SafePlaceGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            SceneManager.LoadScene("Maze");
        }
    }
}
