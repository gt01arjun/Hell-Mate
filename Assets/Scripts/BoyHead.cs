using System.Collections;
using UnityEngine;

public class BoyHead : MonoBehaviour
{
    public static bool DestroyLog = false;

    private GameObject _log;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _arrowGenerator;

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
            _arrowGenerator.GetComponent<ArrowGenerator>().StopSpawn();
            _arrowGenerator.SetActive(false);
            DestroyLog = false;

            StartCoroutine("EnableSpawn");
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

    private IEnumerator EnableSpawn()
    {
        yield return new WaitForSeconds(1.5f);
        _arrowGenerator.SetActive(true);
    }
}