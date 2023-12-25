using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IMainView : IView
{


    #region TopPanel
    ButtonManager BtnSetting { get; set; }

    #endregion

    #region UserInformationView

    TMP_Text TxtUserName { get; set; }

    Button BtnUserIcon { get; set; }

    Button BtnUpdateUserIcon { get; set; }

    TMP_Text TxtNickName { get; set; }

    Button BtnUpdateNickName { get; set; }

    Button BtnEnterPhotoWall { get; set; }

    /// <summary>
    /// ������º���ǳ�
    /// </summary>
    TMP_InputField InptNickName { get; set; }

    /// <summary>
    /// ȷ�ϸ����ǳ�
    /// </summary>
    Button BtnSureUpdateNickName { get; set; }

    #endregion

    #region AlbumView

    ButtonManager BtnCreateAlbum { get; }

    #endregion
}
