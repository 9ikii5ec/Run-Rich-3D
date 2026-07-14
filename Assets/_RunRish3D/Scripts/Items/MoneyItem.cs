using UnityEngine;

namespace ButchersGames
{
    public class MoneyItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameEvents gameEvents;
        [SerializeField] private int value = 5;
        [SerializeField] private GameObject collectEffect;
        [SerializeField] private AudioClip collectSound;

        public void Interact(GameObject player)
        {
            gameEvents?.MoneyCollected(value);

            if (collectEffect != null)
            {
                GameObject fx = Instantiate(collectEffect, transform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
