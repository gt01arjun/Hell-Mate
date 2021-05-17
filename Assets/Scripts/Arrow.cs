using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rb;

    private bool _hasHit;

    [SerializeField]
    private Vector2 _direction;

    [SerializeField]
    private float _launchForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(_direction * _launchForce);
    }

    private void Update()
    {
        if (_hasHit == false)
        {
            TrackMovement();
        }

        if (transform.position.y < -50f)
        {
            Destroy(gameObject);
        }
    }

    private void TrackMovement()
    {
        Vector2 direction = _rb.velocity;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
            Destroy(gameObject.GetComponent<Collider>());
            GameManager.ArrowsHit++;
        }
        else if(_hasHit == false)
        {
            _rb.isKinematic = true;
        }

        _hasHit = true;
    }
}