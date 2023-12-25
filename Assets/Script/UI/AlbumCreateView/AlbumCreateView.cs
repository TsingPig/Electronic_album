using Michsky.MUIP;
using MVPFrameWork;
using System;
using System.Diagnostics;
using TMPro;
using TsingPigSDK;
using UnityEngine.Events;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class AlbumCreateView : ViewBase<IAlbumCreatePresenter>, IAlbumCreateView
{
   
    protected override void OnCreate()
    {

      //  _txtRegisterInputAccount = _root.Find<TMP_InputField>("Window Manager/Windows/Register/Content/inptAccount");
     
    }
}
