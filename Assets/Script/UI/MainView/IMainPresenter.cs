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

    #endregion UserInformation

    #region AlbumView

    void EnterAlbumCreateView();

    #endregion AlbumView
}