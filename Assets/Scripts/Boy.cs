using UnityEngine;

public class Boy : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private float _playerMaxVelocity;

    private bool _jump = false;

    private Rigidbody[] _rigidbodies;

    private void Start()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.GameOver == false)
        {
            //Vector3 newVelocity = new Vector3(0, _thrust, 0);
            //newVelocity.y = Mathf.Clamp(newVelocity.y, _playerMinVelocity, _playerMaxVelocity);
            //_rb.velocity = newVelocity;

            //_rb.AddRelativeForce(Vector3.up * _thrust, ForceMode.VelocityChange);
            //_rb.AddForceAtPosition(Vector3.up * _thrust, transform.position, ForceMode.Impulse);

            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (_jump == true)
        {
            foreach (Rigidbody r in _rigidbodies)
            {
                r.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                r.velocity = Vector3.ClampMagnitude(r.velocity, _playerMaxVelocity);
            }
            _jump = false;
        }
    }
}