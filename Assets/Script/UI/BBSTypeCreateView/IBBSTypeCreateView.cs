using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine;

public interface IBBSTypeCreateView : IView
{
    ButtonManager BtnQuit { get; }
    TMP_InputField InptBBSTypeName { get; }
    ButtonManager BtnCreate{ get; }
}