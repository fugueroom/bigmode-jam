using UnityEngine;
using DG.Tweening;

public class MovingKeep : Tank
{
    public Transform EndingPosition;
    private Sequence moveSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOMove(new Vector3(EndingPosition.position.x, transform.position.y, EndingPosition.position.z), 5f)).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        moveSequence.Kill();
    }
}
