using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class PostView : ViewBase<IPostPresenter>, IPostView
{
    private VerticalLayoutGroup _postItemRoot;
    private VerticalLayoutGroup _commentItemRoot;
    private RectTransform _postRoot;
    private RectTransform _scrollbarView;
    private RectTransform _createCommentPanel;
    private ButtonManager _btnQuit;
    private ButtonManager _btnDeletePost;
    private ButtonManager _btnSureCreateComment;
    private ButtonManager _btnCreateComment;
    private ButtonManager _btnDisactivateCreateCommentPanel;
    private TMP_InputField _inptComment;
    // private NotificationManager _notification;

    public VerticalLayoutGroup PostItemRoot => _postItemRoot;

    public VerticalLayoutGroup CommentItemRoot => _commentItemRoot;

    public RectTransform PostRoot => _postRoot;

    public RectTransform ScrollbarView => _scrollbarView;

    public RectTransform CreateCommentPanel => _createCommentPanel;

    public ButtonManager BtnQuit => _btnQuit;

    public ButtonManager BtnDeletePost => _btnDeletePost;

    public ButtonManager BtnCreateComment => _btnCreateComment;

    // public NotificationManager Notification => _notification;

    public ButtonManager BtnSureCreateComment => _btnSureCreateComment;

    public ButtonManager BtnDisactivateCreateCommentPanel => _btnDisactivateCreateCommentPanel;

    public TMP_InputField InptComment => _inptComment;

    protected override void OnCreate()
    {
        _postItemRoot = _root.Find<VerticalLayoutGroup>("GroupPanel/PostRoot/PostItemRoot");
        _postRoot = _root.Find<RectTransform>("GroupPanel/PostRoot");
        _commentItemRoot = _root.Find<VerticalLayoutGroup>("GroupPanel/PostRoot/ScrollbarView/Viewport/CommentItemRoot");
        _scrollbarView = _root.Find<RectTransform>("GroupPanel/PostRoot/ScrollbarView");
        _createCommentPanel = _root.Find<RectTransform>("CreateCommentPanel");
        _btnQuit = _root.Find<ButtonManager>("GroupPanel/btnQuit");
        _btnCreateComment = _root.Find<ButtonManager>("GroupPanel/btnCreateComment");
        _btnDeletePost = _root.Find<ButtonManager>("GroupPanel/btnDeletePost");
        _btnSureCreateComment = _root.Find<ButtonManager>("CreateCommentPanel/btnSureCreateComment");
        _btnDisactivateCreateCommentPanel = _root.Find<ButtonManager>("CreateCommentPanel/btnDisactivateCreateCommentPanel");
        _inptComment = _root.Find<TMP_InputField>("CreateCommentPanel/inptComment");

        // _notification = _root.Find<NotificationManager>("Notification");


        _btnQuit.onClick.AddListener(_presenter.Quit);
        _btnDeletePost.onClick.AddListener(_presenter.TryDeletePost);
        _btnSureCreateComment.onClick.AddListener(_presenter.SureCreateComment);
        
        _btnCreateComment.onClick.AddListener(() => _createCommentPanel.gameObject.SetActive(true));
        _btnDisactivateCreateCommentPanel.onClick.AddListener(() => _createCommentPanel.gameObject.SetActive(false));
    }
}