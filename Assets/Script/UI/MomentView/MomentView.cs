using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS_1)]
public class MomentView : ViewBase<IMomentPresenter>, IMomentView
{
    private ButtonManager _btnQuit;
    private Transform _photoWallItemRoot;

    public Transform PhotoWallItemRoot
    {
        get => _photoWallItemRoot; set => _photoWallItemRoot = value;
    }

    public ButtonManager BtnQuit { get => _btnQuit; set => _btnQuit = value; }

    protected override void OnCreate()
    {
        _btnQuit = _root.Find<ButtonManager>("GroupPanel/btnQuit");
    }
}