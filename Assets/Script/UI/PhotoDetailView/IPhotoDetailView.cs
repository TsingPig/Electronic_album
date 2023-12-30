using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine.UI;

public interface IPhotoDetailView : IView
{
    ButtonManager BtnQuit { get; }

    ButtonManager BtnDeletePhoto { get; }

    Image ImgDetailPhoto { get; set; }
}