using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class BBSTypeView : ViewBase<IBBSTypePresenter>, IBBSTypeView
{
    private ButtonManager _btnCreateBBSType;
    private Transform _bBSTypeItemRoot;

    public ButtonManager BtnCreateBBSType { get => _btnCreateBBSType; set => _btnCreateBBSType = value; }
    public Transform BBSTypeItemRoot
    {
        get => _bBSTypeItemRoot; set => _bBSTypeItemRoot = value;
    }

    protected override void OnCreate()
    {
        _btnCreateBBSType = _root.Find<ButtonManager>("MainPanel/btnCreateBBSType");
        _bBSTypeItemRoot = _root.Find<Transform>("MainPanel/BBSTypeItemRoot");
    }
}