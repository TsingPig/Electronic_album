using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;
using UnityEngine.UI;

public interface IPostView : IView
{
    public VerticalLayoutGroup PostItemRoot { get;  }

    public ButtonManager BtnQuit { get; }

    public ButtonManager BtnDeletePost { get; }

    // public NotificationManager Notification { get; }

}