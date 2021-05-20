using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rb;

    private bool _hasHit;

    public float ArrowVelocityX;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = new Vector3(ArrowVelocityX, 0, 0);
    }

    private void Update()
    {
        if (transform.position.x > 10f || transform.position.x < -10f)
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
            FixedJoint _fixedJoint = gameObject.AddComponent<FixedJoint>();
            _fixedJoint.connectedBody = collision.transform.GetComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            Destroy(gameObject.GetComponent<Collider>());
            GameManager.LastArrowDirection = ArrowVelocityX;
            GameManager.ArrowsHit++;
            if(GameManager.ArrowsHit >= 3)
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