using UnityEngine;
using DG.Tweening;

namespace ButchersGames
{
    public class RotatingItem : MonoBehaviour
    {
        [SerializeField] private Vector3 rotateAxis = Vector3.up;
        [SerializeField] private float duration = 2f;

        private void Start()
        {
            transform.DORotate(rotateAxis * 360f, duration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }
    }
}
