using UnityEngine;

namespace ButchersGames
{
    [CreateAssetMenu(menuName = "Data/Status Settings")]
    public class StatusSettings : ScriptableObject
    {
        [System.Serializable]
        public struct StatusThreshold
        {
            public StatusType status;
            public int minMoney;
            public string displayName;
        }

        public StatusThreshold[] thresholds = new StatusThreshold[]
        {
            new StatusThreshold { status = StatusType.Poor, minMoney = 0, displayName = "Poor" },
            new StatusThreshold { status = StatusType.Worker, minMoney = 20, displayName = "Worker" },
            new StatusThreshold { status = StatusType.Middle, minMoney = 50, displayName = "Middle" },
            new StatusThreshold { status = StatusType.Rich, minMoney = 100, displayName = "Rich" },
            new StatusThreshold { status = StatusType.Millionaire, minMoney = 200, displayName = "Millionaire" }
        };

        public StatusType GetStatusForMoney(int money)
        {
            StatusType result = StatusType.Poor;
            for (int i = thresholds.Length - 1; i >= 0; i--)
            {
                if (money >= thresholds[i].minMoney)
                {
                    result = thresholds[i].status;
                    break;
                }
            }
            return result;
        }
    }
}
