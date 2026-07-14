using UnityEngine;

namespace ButchersGames
{
    public class GateItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameEvents gameEvents;
        [SerializeField] private GateType gateType;
        [SerializeField] private GateItem pairedGate;
        [SerializeField] private GameObject passEffect;

        private Collider _collider;

        public GateType GateType => gateType;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void Interact(GameObject player)
        {
            gameEvents?.GatePassed(gateType);

            if (pairedGate != null && pairedGate._collider != null)
            {
                Destroy(pairedGate);
            }

            if (passEffect != null)
            {
                GameObject fx = Instantiate(passEffect, transform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }

            Destroy(gameObject);
        }
    }
}
