using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;
using UnityEngine.UI;

public interface IBBSView : IView
{

    VerticalLayoutGroup BBSPostItemRoot { get; }

    public ButtonManager BtnQuit { get; }

    public ButtonManager BtnDeleteSection { get; }

    public ButtonManager BtnEnterCreatePostItemView { get; }

    public NotificationManager Notification { get; }
}