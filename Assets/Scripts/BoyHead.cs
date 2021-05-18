using UnityEngine;

public class BoyHead : MonoBehaviour
{
    public static bool DestroyLog = false;

    private GameObject _log;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MeshDestroy>() && DestroyLog == true)
        {
            //Debug.Log(collision.gameObject.name);
            //collision.gameObject.GetComponent<MeshDestroy>().DestroyMesh();
            //DestroyLog = false;
        }
    }

    private void Update()
    {
        if (DestroyLog == true)
        {
            _log.GetComponent<MeshDestroy>().DestroyMesh();
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