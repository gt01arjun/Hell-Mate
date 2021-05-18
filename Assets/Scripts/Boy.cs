using UnityEngine;

public class Boy : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private float _playerMaxVelocity;

    private bool _jump = false;
    private bool _powerJump = false;

    private Rigidbody[] _rigidbodies;

    public static bool CanPowerJump;

    private void Start()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.GameOver == false && CanPowerJump)
        {
            _powerJump = true;
            CanPowerJump = false;
            BoyHead.DestroyLog = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && GameManager.GameOver == false)
        {
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (_powerJump == true)
        {
            foreach (Rigidbody r in _rigidbodies)
            {
                r.AddForce(Vector3.up * _jumpForce * 3, ForceMode.Impulse);
            }
            _powerJump = false;
        }
        else if (_jump == true)
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