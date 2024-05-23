using Michsky.MUIP;
using MVPFrameWork;
using System;
using TMPro;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS_2)]
public class NotificationView : ViewBase<INotificationPresenter>, INotificationView
{
    private TMP_Text _title;
    private NotificationManager _notification;

    public TMP_Text Title { get => _title; set => _title = value; }

    public NotificationManager Notification => _notification;

    protected override void OnCreate()
    {
        _title = _root.Find<TMP_Text>("Title");
        _notification = _root.GetComponent<NotificationManager>();
    }
}