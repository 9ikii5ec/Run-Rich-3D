using UnityEngine;

namespace ButchersGames
{
    public class PlayerVisuals : MonoBehaviour
    {
        [SerializeField] private GameEvents gameEvents;
        [SerializeField] private GameObject[] statusModels;
        [SerializeField] private AudioClip[] statusSounds;

        private int _currentModelIndex = -1;

        private void OnEnable()
        {
            if (gameEvents != null)
            {
                gameEvents.OnStatusChanged += HandleStatusChanged;
            }
        }

        private void OnDisable()
        {
            if (gameEvents != null)
            {
                gameEvents.OnStatusChanged -= HandleStatusChanged;
            }
        }

        private void HandleStatusChanged(StatusType newStatus)
        {
            int modelIndex = (int)newStatus;
            if (modelIndex < 0 || modelIndex >= statusModels.Length) return;
            if (modelIndex == _currentModelIndex) return;

            if (_currentModelIndex >= 0 && _currentModelIndex < statusModels.Length)
            {
                statusModels[_currentModelIndex].SetActive(false);
            }

            statusModels[modelIndex].SetActive(true);
            _currentModelIndex = modelIndex;

            if (statusSounds != null && modelIndex < statusSounds.Length && statusSounds[modelIndex] != null)
            {
                AudioSource.PlayClipAtPoint(statusSounds[modelIndex], transform.position);
            }
        }
    }
}
