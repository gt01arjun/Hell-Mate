using UnityEngine;

public class BoyHead : MonoBehaviour
{
    public static bool DestroyLog = false;

    private GameObject _log;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (DestroyLog == true)
        {
            _log.GetComponent<MeshDestroy>().DestroyMesh();
            _audioSource.Stop();
            _audioSource.Play();
            DestroyLog = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PowerJump>())
        {
            Boy.CanPowerJump = true;
            _log = other.gameObject.transform.root.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PowerJump>())
        {
            Boy.CanPowerJump = false;
            _log = null;
        }
    }
}