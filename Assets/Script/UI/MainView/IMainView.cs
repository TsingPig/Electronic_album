using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine;
using UnityEngine.UI;

public interface IMainView : IView
{
    #region UserInformationView

    Text TxtUserName { get; set; }

    Button BtnUserIcon { get; set; }

    Button BtnUpdateUserIcon { get; set; }

    Text TxtNickName { get; set; }

    Button BtnUpdateNickName { get; set; }

    Button BtnEnterPhotoWall { get; set; }

    #endregion


}
