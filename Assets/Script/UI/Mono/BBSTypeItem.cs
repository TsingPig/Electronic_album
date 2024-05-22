using Michsky.MUIP;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

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
            
            bbs.SetModel(new IBBSModel.BBS() { 
                sectionname = TxtBBSTypeName.text,
            });

            MVPFrameWork.UIManager.Instance.Enter(ViewId.BBSView, bbs);
        });
    }
}