using UnityEngine;
using System.Linq;
public class QuestUINotificationManager : MonoBehaviour
{
    [SerializeField] QuestCompleteNotification[] _notifications;

    public QuestCompleteNotification GetNotification() => _notifications.FirstOrDefault(x => x.canUse);
}
