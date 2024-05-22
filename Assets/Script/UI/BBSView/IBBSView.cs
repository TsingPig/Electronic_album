using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;
using UnityEngine.UI;

public interface IBBSView : IView
{

    VerticalLayoutGroup BBSPostItemRoot { get; }

    NotificationManager Notification { get; }
}