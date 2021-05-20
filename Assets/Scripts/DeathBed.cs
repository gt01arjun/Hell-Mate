using UnityEngine;

public class DeathBed : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root != null && other.transform.root.GetComponent<Boy>())
        {
            GameManager.GameOverEvent.Invoke();
        }
    }
}