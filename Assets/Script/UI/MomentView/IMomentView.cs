using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;

public interface IMomentView : IView
{
    ButtonManager BtnQuit { get; set; }

    Transform PhotoWallItemRoot { get; set; }

}