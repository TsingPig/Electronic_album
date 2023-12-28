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
    /// �����ǳ�
    /// </summary>
    void UpdateNickName();

    /// <summary>
    /// ȷ�ϸ���
    /// </summary>
    void SureUpdateNickName();

    /// <summary>
    /// �����û�ͷ��
    /// </summary>
    /// <param name="icon"></param>
    void UpdateUserIcon();

    #endregion

    #region AlbumView

    void EnterAlbumCreateView();

    #endregion

}
