using UnityEngine;

namespace ButchersGames
{
    public class PlayerTriggerDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(transform.root.gameObject);
            }
        }
    }
}
