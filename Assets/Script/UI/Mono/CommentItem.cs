using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using UIManager = MVPFrameWork.UIManager;

/// <summary>
/// 每个评论的项
/// </summary>
public class CommentItem : MonoBehaviour
{

    public TMP_Text UserName;
    
    public TMP_Text Content;

    public TMP_Text CreateTime;

    public Image UserIcon;

    private void Start()
    {

    }


}