using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;

public interface IPostView : IView
{
    public Transform PostItemRoot { get;  }

    public ButtonManager BtnQuit { get; }

    public ButtonManager BtnDeletePost { get; }

    // public NotificationManager Notification { get; }

}