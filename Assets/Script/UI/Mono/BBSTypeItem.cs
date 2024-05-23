using Michsky.MUIP;
using TMPro;
using UnityEngine;

/// <summary>
/// ÿ�������
/// </summary>
public class BBSTypeItem : ItemBase
{
    public ButtonManager BtnEnterBBS;

    public TMP_Text TxtBBSTypeName;

    private void Start()
    {
        BtnEnterBBS.onClick.AddListener(() =>
        {
            Debug.Log("Enter BBSTypeView");

            var bbs = new BBSModel();

            bbs.SetModel(new IBBSModel.BBS()
            {
                sectionname = TxtBBSTypeName.text,
            });

            MVPFrameWork.UIManager.Instance.Enter(ViewId.BBSView, bbs);
        });
    }
}