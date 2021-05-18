using UnityEngine;

public class LogGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _logPrefab;
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private float _logSpawnRate;

    [SerializeField]
    private int _logYMin;
    [SerializeField]
    private int _logYMax;

    private void Start()
    {
        InvokeRepeating("SpawnLog", 1f, _logSpawnRate);
    }

    private void SpawnLog()
    {
        Instantiate(_logPrefab, new Vector3(0, _player.transform.position.y + Random.Range(_logYMin, _logYMax), 0), _logPrefab.transform.rotation);
    }
}