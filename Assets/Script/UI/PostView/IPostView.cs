using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IPostView : IView
{
    public VerticalLayoutGroup PostItemRoot { get; }

    public VerticalLayoutGroup CommentItemRoot { get; }

    public RectTransform PostRoot { get; }

    public RectTransform ScrollbarView { get; }

    public RectTransform CreateCommentPanel { get; }

    public ButtonManager BtnQuit { get; }

    public ButtonManager BtnDeletePost { get; }

    public ButtonManager BtnCreateComment { get; }

    // public NotificationManager Notification { get; }

    public ButtonManager BtnSureCreateComment { get; }

    public ButtonManager BtnDisactivateCreateCommentPanel { get; }

    public TMP_InputField InptComment { get; }


}