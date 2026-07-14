using UnityEngine;

namespace ButchersGames
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private GameEvents gameEvents;
        [SerializeField] private StatusSettings statusSettings;
        [SerializeField] private int startingMoney = 0;

        private int _currentMoney;
        private StatusType _currentStatus;

        public int CurrentMoney => _currentMoney;
        public StatusType CurrentStatus => _currentStatus;

        private void OnEnable()
        {
            if (gameEvents != null)
            {
                gameEvents.OnMoneyCollected += HandleMoneyCollected;
            }
        }

        private void OnDisable()
        {
            if (gameEvents != null)
            {
                gameEvents.OnMoneyCollected -= HandleMoneyCollected;
            }
        }

        private void Start()
        {
            _currentMoney = startingMoney;
            _currentStatus = statusSettings.GetStatusForMoney(_currentMoney);
        }

        private void HandleMoneyCollected(int amount)
        {
            _currentMoney += amount;
            if (_currentMoney < 0) _currentMoney = 0;

            gameEvents.MoneyCountChanged(_currentMoney);

            StatusType newStatus = statusSettings.GetStatusForMoney(_currentMoney);
            if (newStatus != _currentStatus)
            {
                _currentStatus = newStatus;
                gameEvents.StatusChanged(_currentStatus);
            }
        }
    }
}
