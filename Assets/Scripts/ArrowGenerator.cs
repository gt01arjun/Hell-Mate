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

    private void Start()
    {
        InvokeRepeating("SpawnArrow", 1f, _arrowSpawnRate);
    }

    private void SpawnArrow()
    {
        int r = Random.Range(0, 2);

        if (r == 0)
        {
            GameObject arrowInstance = Instantiate(_arrowPrefab, new Vector3(_leftArrowX, _player.transform.position.y + Random.Range(_arrowYMin, _arrowYMax), _arrowPrefab.transform.position.z), _arrowPrefab.transform.rotation);
            arrowInstance.GetComponent<Arrow>().ArrowVelocityX = -arrowInstance.GetComponent<Arrow>().ArrowVelocityX;
        }
        else if (r == 1)
        {
            Instantiate(_arrowPrefab, new Vector3(_rightArrowX, _player.transform.position.y + Random.Range(_arrowYMin, _arrowYMax), _arrowPrefab.transform.position.z), _arrowPrefab.transform.rotation);
        }
    }
}