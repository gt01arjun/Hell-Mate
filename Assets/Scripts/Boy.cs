using UnityEngine;

public class Boy : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private float _playerMaxVelocity;

    [SerializeField]
    private AudioClip[] _audioClips;

    [SerializeField]
    private AudioClip _powerJumpSoundClip;

    private bool _jump = false;
    private bool _powerJump = false;

    private Rigidbody[] _rigidbodies;

    private AudioSource _audioSource;

    public static bool CanPowerJump;

    private void Start()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();

        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.GameOver == false && CanPowerJump)
        {
            _powerJump = true;
            CanPowerJump = false;
            BoyHead.DestroyLog = true;
            _audioSource.PlayOneShot(_powerJumpSoundClip);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && GameManager.GameOver == false && GameManager.GameStarted == true)
        {
            _jump = true;

            int s = Random.Range(0, _audioClips.Length);
            _audioSource.PlayOneShot(_audioClips[s]);
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

    void OnBecameInvisible()
    {
        Debug.Log("Called");
        //GameManager.GameOverEvent.Invoke();
    }
}