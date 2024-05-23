using Michsky.MUIP;
using MVPFrameWork;

public interface IPostView : IView
{


    public ButtonManager BtnQuit { get; }

    public ButtonManager BtnDeletePost { get; }

    // public NotificationManager Notification { get; }

}