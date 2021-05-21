using UnityEngine;

public class DeathBed : MonoBehaviour
{
    private bool _hasHit;

    private void Start()
    {
        _hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root != null && other.transform.root.GetComponent<Boy>() && _hasHit == false && GameManager.GameOver == false)
        {
            _hasHit = true;
            GameManager.GameOverEvent.Invoke();
        }
    }
}