using UnityEngine;
using DG.Tweening;

public class FloatRagdoll : MonoBehaviour
{
    [SerializeField]
    private Ease _easeType;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private float _moveAmount;

    private void Start()
    {
        transform.DOMoveY(_moveAmount, _duration).SetEase(_easeType).SetLoops(-1, LoopType.Yoyo);
    }
}