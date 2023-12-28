using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;
using TMPro;
using System;

public interface IMainPresenter : IPresenter
{
    #region UserInformation

    UserInformation LoadUserInformation();

    void ClearUserInformationCache();

    /// <summary>
    /// 更新昵称
    /// </summary>
    void UpdateNickName();

    /// <summary>
    /// 确认更新
    /// </summary>
    void SureUpdateNickName();

    /// <summary>
    /// 更新用户头像
    /// </summary>
    /// <param name="icon"></param>
    void UpdateUserIcon();

    #endregion

    #region AlbumView

    void EnterAlbumCreateView();

    #endregion

}
