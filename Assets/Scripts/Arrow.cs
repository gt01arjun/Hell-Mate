using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rb;

    private bool _hasHit;

    public float ArrowVelocityX;

    public AudioSource AudioSource;

    public GameObject BloodEffect;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = new Vector3(ArrowVelocityX, 0, 0);
    }

    private void Update()
    {
        if (transform.position.x > 16f || transform.position.x < -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rb.velocity = Vector3.zero;

        if (collision.transform.root.GetComponent<Boy>() && _hasHit == false)
        {
            //Debug.Log(collision.transform.name);
            AudioSource.Play();
            FixedJoint _fixedJoint = gameObject.AddComponent<FixedJoint>();
            _fixedJoint.connectedBody = collision.transform.GetComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            Destroy(gameObject.GetComponent<Collider>());
            GameObject _bloodEffect = Instantiate(BloodEffect, collision.contacts[0].point, Quaternion.identity);
            Destroy(_bloodEffect, 3f);
            GameManager.LastArrowDirection = ArrowVelocityX;
            GameManager.ArrowsHit++;
            if (GameManager.ArrowsHit >= 3)
            {
                GameManager.GameOverEvent.Invoke();
            }
        }
        else if (_hasHit == false)
        {
            _rb.isKinematic = true;
        }

        _hasHit = true;
    }
}