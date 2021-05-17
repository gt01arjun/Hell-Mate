using UnityEngine;

public class BoyHead : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MeshDestroy>())
        {
            Debug.Log(collision.gameObject.name);
            collision.gameObject.GetComponent<MeshDestroy>().DestroyMesh();
        }
    }
}