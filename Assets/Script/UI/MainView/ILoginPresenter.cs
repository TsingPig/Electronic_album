using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;

public interface IMainPresenter : IPresenter
{
    #region UserInformation

    UserInformation LoadUserInformation();

    void ClearUserInformationCache();

    //void PresentUserInformation(UserInformation userInformation);


    //void ClearUserInformationCache();


    #endregion

}
