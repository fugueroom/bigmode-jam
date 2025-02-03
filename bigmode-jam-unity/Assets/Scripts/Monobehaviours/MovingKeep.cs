using UnityEngine;
using DG.Tweening;

public class MovingKeep : Tank
{
    public Transform EndingPosition;
    private Sequence moveSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOMove(new Vector3(EndingPosition.position.x, transform.position.y, EndingPosition.position.z), 6f)).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        moveSequence.Kill();
    }
}
