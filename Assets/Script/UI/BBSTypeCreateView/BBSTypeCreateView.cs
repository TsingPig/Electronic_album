using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class BBSTypeCreateView : ViewBase<IBBSTypeCreatePresenter>, IBBSTypeCreateView
{
    private ButtonManager _btnQuit;

    private TMP_InputField _inptBBSTypeName;

    private ButtonManager _btnCreate;

    public ButtonManager BtnQuit => _btnQuit;
    public TMP_InputField InptBBSTypeName => _inptBBSTypeName;
    public ButtonManager BtnCreate => _btnCreate;


    protected override void OnCreate()
    {
        _btnQuit = _root.Find<ButtonManager>("MainPanel/btnQuit");
        _inptBBSTypeName = _root.Find<TMP_InputField>("MainPanel/inptBBSTypeName");
        _btnCreate = _root.Find<ButtonManager>("MainPanel/btnCreate");
        _btnQuit.onClick.AddListener(_presenter.Quit);
        _btnCreate.onClick.AddListener(_presenter.Create);
    }
}