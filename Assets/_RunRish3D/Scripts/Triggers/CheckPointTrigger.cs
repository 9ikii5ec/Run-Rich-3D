using UnityEngine;
using DG.Tweening;

namespace ButchersGames
{
    public class CheckPointTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private AudioClip hitSound;

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

            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }
        }
    }
}
