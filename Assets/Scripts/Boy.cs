using UnityEngine;

public class Boy : MonoBehaviour
{
    [SerializeField]
    private float _thrust;

    [SerializeField]
    private float _thrustMultiplier;

    [SerializeField]
    private Rigidbody _rb;

    private float _startTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // key pressed: save the current time
            _startTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // key released: measure the time
            float timePressed = Time.time - _startTime;

            if (timePressed > 0.3f)
            {
                _rb.velocity = new Vector3(0, _thrust * _thrustMultiplier, 0);
            }
            else
            {
                _rb.velocity = new Vector3(0, _thrust, 0);
            }
        }
    }
}