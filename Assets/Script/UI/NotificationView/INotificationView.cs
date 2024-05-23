using Michsky.MUIP;
using MVPFrameWork;
using TMPro;

public interface INotificationView : IView
{
    public TMP_Text Title { get; set; }

    public NotificationManager Notification { get;  }
}