using TMPro;
using UnityEngine;

namespace ButchersGames
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameEvents gameEvents;
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private TextMeshProUGUI scoreText;

        private void OnEnable()
        {
            if (gameEvents != null)
            {
                gameEvents.OnGameStarted += HandleGameStarted;
                gameEvents.OnDistanceChanged += HandleDistanceChanged;
            }
        }

        private void OnDisable()
        {
            if (gameEvents != null)
            {
                gameEvents.OnGameStarted -= HandleGameStarted;
                gameEvents.OnDistanceChanged -= HandleDistanceChanged;
            }
        }

        private void Start()
        {
            if (startPanel != null) startPanel.SetActive(true);
            if (gamePanel != null) gamePanel.SetActive(false);
        }

        private void HandleGameStarted()
        {
            if (startPanel != null) startPanel.SetActive(false);
            if (gamePanel != null) gamePanel.SetActive(true);
        }

        private void HandleDistanceChanged(int distance)
        {
            if (scoreText != null)
            {
                scoreText.text = distance.ToString();
            }
        }
    }
}
