using Michsky.MUIP;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����б���ĵ���¼�
/// </summary>
public class PhotoItem : ItemBase
{
    public ButtonManager BtnEnterManager;
    public Image Cover;

    /// <summary>
    /// ͼƬ����
    /// </summary>
    //[HideInInspector]
    public int photoId;

    protected override void OnClick()
    {
        base.OnClick();
        Debug.Log($"PhotoItem OnClick");

        PhotoDetailModel model = new PhotoDetailModel();

        model.SetModel(Cover, photoId);

        MVPFrameWork.UIManager.Instance.Enter(ViewId.PhotoDetailView, model);
    }

    private void Start()
    {
        BtnItem = BtnEnterManager;
    }
}