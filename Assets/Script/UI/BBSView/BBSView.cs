using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class BBSView : ViewBase<IBBSPresenter>, IBBSView
{
    private VerticalLayoutGroup _bBSPostItemRoot;

    private ButtonManager _btnQuit;

    public VerticalLayoutGroup BBSPostItemRoot => _bBSPostItemRoot;

    public ButtonManager BtnQuit => _btnQuit;

    protected override void OnCreate()
    {
        _bBSPostItemRoot = _root.Find<VerticalLayoutGroup>("GroupPanel/MainPanel/ScrollbarView/Viewport/BBSPostItemRoot");
        _btnQuit = _root.Find<ButtonManager>("GroupPanel/btnQuit");
        
        _btnQuit.onClick.AddListener(() => {
            MVPFrameWork.UIManager.Instance.Quit(ViewId.BBSView);
        });
    }
}