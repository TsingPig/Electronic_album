using MVPFrameWork;

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

    #endregion UserInformation

    #region AlbumView

    void EnterAlbumCreateView();

    #endregion AlbumView
}