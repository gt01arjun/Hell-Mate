using UnityEngine;

public class Boy : MonoBehaviour
{
    [SerializeField]
    private float _thrust;

    [SerializeField]
    private Rigidbody _rb;

    [SerializeField]
    private float _playerMinVelocity;
    [SerializeField]
    private float _playerMaxVelocity;

    private void Update()
    {
        //Debug.Log(_rb.velocity);

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.GameOver == false)
        {
            Vector3 newVelocity = new Vector3(0, _thrust, 0);
            newVelocity.y = Mathf.Clamp(newVelocity.y, _playerMinVelocity, _playerMaxVelocity);
            _rb.velocity = newVelocity;
        }
    }
}