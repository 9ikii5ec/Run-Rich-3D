using UnityEngine;

namespace ButchersGames
{
    public class AlcoholItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameEvents gameEvents;
        [SerializeField] private int penalty = -10;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private AudioClip hitSound;

        public void Interact(GameObject player)
        {
            gameEvents?.MoneyCollected(penalty);

            Animator animator = player.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Stumble");
            }

            if (hitEffect != null)
            {
                GameObject fx = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }

            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
