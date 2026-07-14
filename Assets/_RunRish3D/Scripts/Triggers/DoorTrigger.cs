using UnityEngine;

namespace ButchersGames
{
    public class DoorTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private int currentDoorIndex = 0;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Interact(GameObject player)
        {
            if (currentDoorIndex == 0)
            {
                animator.SetTrigger("Open");
            }
            else
            {
                string _index = "Open" + currentDoorIndex.ToString();
                Debug.Log(_index);
                animator.SetTrigger(_index);
            }

        }
    }
}
