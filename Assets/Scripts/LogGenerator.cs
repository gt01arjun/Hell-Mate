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
    [SerializeField]
    private GameObject _initialBoosterText;

    private int c;

    private void Start()
    {
        c = 0;
        InvokeRepeating("SpawnLog", 1f, _logSpawnRate);
    }

    private void SpawnLog()
    {
        c++;
        GameObject currentLog = Instantiate(_logPrefab, new Vector3(_logPrefab.transform.position.x, _player.transform.position.y + Random.Range(_logYMin, _logYMax), 0), _logPrefab.transform.rotation);
        if (c == 1)
        {
            Instantiate(_initialBoosterText, currentLog.transform.position - new Vector3(-1.2f, 5f, 0), _initialBoosterText.transform.rotation);
        }
    }

    private void Update()
    {
        if (GameManager.GameOver == true)
        {
            CancelInvoke();
        }
    }
}