using MVPFrameWork;
using UnityEngine;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class PhotoView : ViewBase<IPhotoPresenter>, IPhotoView
{
    protected override void OnCreate()
    {
        Debug.Log("OnCreate PhotoView");
    }
}