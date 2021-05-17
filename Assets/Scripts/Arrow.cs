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
    }

    private void TrackMovement()
    {
        Vector2 direction = _rb.velocity;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _hasHit = true;
        _rb.velocity = Vector3.zero;
        //_rb.isKinematic = true;
    }
}