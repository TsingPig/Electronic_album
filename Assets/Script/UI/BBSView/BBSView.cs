using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class BBSView : ViewBase<IBBSPresenter>, IBBSView
{
    private VerticalLayoutGroup _bBSPostItemRoot;

    private ButtonManager _btnQuit;

    private ButtonManager _btnDeleteSection;

    public VerticalLayoutGroup BBSPostItemRoot => _bBSPostItemRoot;

    public ButtonManager BtnQuit => _btnQuit;

    public ButtonManager BtnDeleteSection => _btnDeleteSection;

    protected override void OnCreate()
    {
        _bBSPostItemRoot = _root.Find<VerticalLayoutGroup>("GroupPanel/MainPanel/ScrollbarView/Viewport/BBSPostItemRoot");
        _btnQuit = _root.Find<ButtonManager>("GroupPanel/btnQuit");
        _btnDeleteSection = _root.Find<ButtonManager>("GroupPanel/btnDeleteSection");
        _btnQuit.onClick.AddListener(() =>
        {
            MVPFrameWork.UIManager.Instance.Quit(ViewId.BBSView);
        });

        _btnDeleteSection.onClick.AddListener(_presenter.DeleteSection);

    }
}