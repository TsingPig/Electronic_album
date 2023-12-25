using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

public interface IAlbumCreateView : IView
{
    ButtonManager BtnCreateAlbum { get; }
    TMP_InputField InptAlbumName { get; }
    CustomDropdown DropDownAlbumType { get; }
    ButtonManager BtnQuit { get; }
}
