using Michsky.MUIP;
using MVPFrameWork;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class PostView : ViewBase<IPostPresenter>, IPostView
{
    private ButtonManager _btnQuit;

    public ButtonManager BtnQuit => _btnQuit;

    protected override void OnCreate()
    {
        _btnQuit = _root.Find<ButtonManager>("GroupPanel/btnQuit");
        _btnQuit.onClick.AddListener(_presenter.Quit);
    }
}