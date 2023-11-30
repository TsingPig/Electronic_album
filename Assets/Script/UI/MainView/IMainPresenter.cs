using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;
using TMPro;

public interface IMainPresenter : IPresenter
{
    #region UserInformation

    UserInformation LoadUserInformation();

    void ClearUserInformationCache();

    /// <summary>
    /// �����ǳ�
    /// </summary>
    void UpdateNickName();

    #endregion

}
