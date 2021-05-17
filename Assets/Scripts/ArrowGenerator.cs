using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _arrow;
    [SerializeField]
    private GameObject _player;

    private void Start()
    {
        InvokeRepeating("SpawnArrow", 1f, 1f);
    }

    private void SpawnArrow()
    {
        Instantiate(_arrow, new Vector3(_arrow.transform.position.x, _player.transform.position.y + Random.Range(-2, 3), _arrow.transform.position.z), _arrow.transform.rotation);
    }
}