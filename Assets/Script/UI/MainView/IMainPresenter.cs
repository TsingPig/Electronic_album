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
    /// 更新昵称
    /// </summary>
    void UpdateNickName();

    /// <summary>
    /// 确认更新
    /// </summary>
    void SureUpdateNickName();

    #endregion

}
