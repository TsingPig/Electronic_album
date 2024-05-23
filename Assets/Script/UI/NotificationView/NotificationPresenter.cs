using MVPFrameWork;
using TMPro;
using UIManager = MVPFrameWork.UIManager;

public class NotificationPresenter : PresenterBase<INotificationView, INotificationModel>, INotificationPresenter
{
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        OnShowCompleted();
    }
    public override void OnShowCompleted()
    {
        base.OnShowCompleted();
        _view.Title.text = _model.Title;
        _view.Notification.CloseEvent = () => { UIManager.Instance.Quit(ViewId.NotificationView); };
        _view.Notification.title = _model.Title;
        _view.Notification.Open();
    }
}