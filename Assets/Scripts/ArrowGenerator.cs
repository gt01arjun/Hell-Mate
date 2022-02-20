using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _arrowPrefab;
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private int _leftArrowX;
    [SerializeField]
    private int _rightArrowX;

    [SerializeField]
    private int _arrowYMin;
    [SerializeField]
    private int _arrowYMax;

    [SerializeField]
    private float _arrowSpawnRate;
    public float ArrowSpawnRate
    {
        get => _arrowSpawnRate;
        set
        {
            if (value > 0.2) _arrowSpawnRate = value;
        }
    }

    [SerializeField]
    private int _arrowSpawnDistance;

    private int lastArrowPosition;
    private int newArrowPosition;

    private void OnEnable()
    {
        InvokeRepeating("SpawnArrow", 1f, ArrowSpawnRate);
    }

    private void SpawnArrow()
    {
        int r = Random.Range(0, 2);
        newArrowPosition = Random.Range(_arrowYMin, _arrowYMax);

        while (Mathf.Abs(newArrowPosition - lastArrowPosition) < _arrowSpawnDistance)
        {
            newArrowPosition = Random.Range(_arrowYMin, _arrowYMax);
        }

        if (r == 0)
        {
            GameObject arrowInstance = Instantiate(_arrowPrefab, new Vector3(_leftArrowX, _player.transform.position.y + newArrowPosition, _arrowPrefab.transform.position.z), Quaternion.identity);
            arrowInstance.GetComponent<Arrow>().ArrowVelocityX = -arrowInstance.GetComponent<Arrow>().ArrowVelocityX;
            arrowInstance.transform.Rotate(0, 0, 180);
        }
        else if (r == 1)
        {
            GameObject arrowInstance = Instantiate(_arrowPrefab, new Vector3(_rightArrowX, _player.transform.position.y + newArrowPosition, _arrowPrefab.transform.position.z), Quaternion.identity);
        }

        lastArrowPosition = newArrowPosition;
    }

    private void Update()
    {
        if(GameManager.GameOver == true)
        {
            CancelInvoke();
        }
    }

    public void StopSpawn()
    {
        CancelInvoke();
    }
}