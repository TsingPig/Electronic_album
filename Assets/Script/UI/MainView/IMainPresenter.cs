using MVPFrameWork;

public interface IMainPresenter : IPresenter
{
    #region TopPanel

    void EnterCreatePhotoWallItemView();

    void ClearUserInformationCache();

    #endregion TopPanel

    #region BBSTypeView

    void EnterBBSTypeCreateView();

    #endregion

    #region PhotoWallView

    #endregion PhotoWallView

    #region UserInformation

    UserInformation LoadUserInformation();

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

    #endregion UserInformation

    #region AlbumView

    void EnterAlbumCreateView();

    #endregion AlbumView
}