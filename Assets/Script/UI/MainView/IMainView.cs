using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IMainView : IView
{
    WindowManager WindowsManager { get; set; }

    #region TopPanel

    ButtonManager BtnSetting { get; set; }

    ButtonManager BtnCreatePhotoWallItem { get; }

    #endregion TopPanel

    #region BBSTypeView
    ButtonManager BtnCreateBBSType { get; set; }

    GridLayoutGroup BBSTypeItemRoot { get; set; }

    #endregion

    #region PhotoWallView

    VerticalLayoutGroup PhotoWallItemRoot { get; set; }

    #endregion PhotoWallView

    #region UserInformationView

    TMP_Text TxtUserName { get; set; }

    Button BtnUserIcon { get; set; }

    Button BtnUpdateUserIcon { get; set; }

    TMP_Text TxtNickName { get; set; }

    Button BtnUpdateNickName { get; set; }

    Button BtnEnterPhotoWall { get; set; }

    /// <summary>
    /// 输入更新后的昵称
    /// </summary>
    TMP_InputField InptNickName { get; set; }

    /// <summary>
    /// 确认更新昵称
    /// </summary>
    Button BtnSureUpdateNickName { get; set; }

    #endregion UserInformationView

    #region AlbumView

    ButtonManager BtnCreateAlbum { get; }

    GridLayoutGroup GridAlbumContent { get; }

    #endregion AlbumView
}