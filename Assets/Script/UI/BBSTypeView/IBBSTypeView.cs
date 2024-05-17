using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;
using UnityEngine.UI;

public interface IBBSTypeView : IView
{
    ButtonManager BtnCreateBBSType { get; set; }

    Transform BBSTypeItemRoot { get; set; }
}