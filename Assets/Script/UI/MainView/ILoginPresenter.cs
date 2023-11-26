using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;

public interface IMainPresenter : IPresenter
{
    #region UserInformation
    void SaveUserInformation(string account, string nickName, Texture2D icon);
    
    
    
    
    #endregion

}
