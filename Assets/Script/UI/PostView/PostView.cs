using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine.Playables;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class PostView : ViewBase<IPostPresenter>, IPostView
{
    private ButtonManager _btnQuit;
    private ButtonManager _btnDeletePost;
    private NotificationManager _notification;


    public ButtonManager BtnQuit => _btnQuit;

    public ButtonManager BtnDeletePost => _btnDeletePost;

    public NotificationManager Notification => _notification;

    protected override void OnCreate()
    {
        _btnQuit = _root.Find<ButtonManager>("GroupPanel/btnQuit");
        _btnDeletePost = _root.Find<ButtonManager>("GroupPanel/btnDeletePost");
        _notification = _root.Find<NotificationManager>("Notification");
        _btnQuit.onClick.AddListener(_presenter.Quit);
        _btnDeletePost.onClick.AddListener(_presenter.TryDeletePost);
    }
}