using UnityEngine;
using DG.Tweening;

namespace ButchersGames
{
    public class CheckPointTrigger : MonoBehaviour, IInteractable
    {
        private bool _isActivated;
        private Animator animator;

        private void Awake()
        {
            animator = transform.parent.GetComponent<Animator>();
        }

        public void Interact(GameObject player)
        {
            if (_isActivated) return;
            _isActivated = true;
            animator.SetTrigger("Flag");
        }
    }
}
