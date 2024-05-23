using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class BBSView : ViewBase<IBBSPresenter>, IBBSView
{
    private VerticalLayoutGroup _bBSPostItemRoot;

    private ButtonManager _btnQuit;

    private ButtonManager _btnDeleteSection;

    private ButtonManager _btnEnterCreatePostItemView;

    private NotificationManager _notification;

    public VerticalLayoutGroup BBSPostItemRoot => _bBSPostItemRoot;

    public ButtonManager BtnQuit => _btnQuit;

    public ButtonManager BtnDeleteSection => _btnDeleteSection;

    public ButtonManager BtnEnterCreatePostItemView => _btnEnterCreatePostItemView;

    public NotificationManager Notification => _notification;

    protected override void OnCreate()
    {
        _bBSPostItemRoot = _root.Find<VerticalLayoutGroup>("GroupPanel/MainPanel/ScrollbarView/Viewport/BBSPostItemRoot");
        _btnQuit = _root.Find<ButtonManager>("GroupPanel/btnQuit");
        _btnDeleteSection = _root.Find<ButtonManager>("GroupPanel/btnDeleteSection");
        _btnEnterCreatePostItemView = _root.Find<ButtonManager>("GroupPanel/btnEnterCreatePostItemView");

        _notification = _root.Find<NotificationManager>("GroupPanel/Notification");
        _btnQuit.onClick.AddListener(() =>
        {
            MVPFrameWork.UIManager.Instance.Quit(ViewId.BBSView);
        });

        _btnDeleteSection.onClick.AddListener(_presenter.DeleteSection);
        _btnEnterCreatePostItemView.onClick.AddListener(_presenter.EnterCreatePostItemView);
    }
}