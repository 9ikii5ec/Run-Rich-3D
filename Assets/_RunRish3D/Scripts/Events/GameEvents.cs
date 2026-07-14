using System;
using UnityEngine;

namespace ButchersGames
{
    [CreateAssetMenu(menuName = "Data/Game Events")]
    public class GameEvents : ScriptableObject
    {
        public event Action OnGameStarted;
        public event Action<int> OnMoneyCollected;
        public event Action<StatusType> OnStatusChanged;
        public event Action<int> OnMoneyCountChanged;
        public event Action<int> OnDistanceChanged;
        public event Action<GateType> OnGatePassed;

        public void GameStarted()
        {
            OnGameStarted?.Invoke();
        }

        public void MoneyCollected(int amount)
        {
            OnMoneyCollected?.Invoke(amount);
        }

        public void StatusChanged(StatusType newStatus)
        {
            OnStatusChanged?.Invoke(newStatus);
        }

        public void MoneyCountChanged(int currentMoney)
        {
            OnMoneyCountChanged?.Invoke(currentMoney);
        }

        public void DistanceChanged(int distance)
        {
            OnDistanceChanged?.Invoke(distance);
        }

        public void GatePassed(GateType gateType)
        {
            OnGatePassed?.Invoke(gateType);
        }
    }
}
