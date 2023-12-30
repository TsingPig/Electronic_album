using UnityEngine;

namespace TsingPigSDK
{
    public abstract class BasePanel
    {
        public UIType UIType { get; protected set; }

        public GameObject PanelObject
        {
            get
            {
                return UIManager.Instance.GetSingleUI(UIType);
            }
        }

        public BasePanel()
        {
            UIType = new UIType(this.GetType().Name);
        }

        public virtual void OnEntry()
        {
            Debug.Log($"��{UIType.Name}���");
        }

        public virtual void OnPause()
        {
            PanelObject.GetComponent<CanvasGroup>().interactable = false;
        }

        public virtual void OnResume()
        {
            PanelObject.GetComponent<CanvasGroup>().interactable = true;
        }

        public virtual void OnExit()
        {
        }
    }
}

//public class MainPanel : BasePanel
//{
//    static readonly string path = Str_Def.MainPanel;
//    public MainPanel() : base(new UIType(path))
//    {
//    }
//    /*
//     ����A a = new B();����a���鷽��ʱ��
//    new����A�ģ�override����B��
//    override�Ὣ���෽�����ǵ������ڣ�new�Ƿ�������
//     */

//}

//public class EnsurePanel : BasePanel
//{
//    static readonly string path = Path.EnsurePanel;

//    //ȷ�ϵ�����
//    public delegate void ContinueEvent();   //ί���¼�
//    private ContinueEvent _continueEvent;
//    private string ensureTipString;            //ȷ�ϵ�����ַ�����ʾ
//    public EnsurePanel(ContinueEvent continueEvent, string tipString = "�Ƿ�ȷ�ϣ�") : base(new UIType(path))
//    {
//        _continueEvent = continueEvent;
//        ensureTipString = tipString;
//    }
//    public override void OnEntry()
//    {
//        UIManager.Instance.GetOrAddComponentInChilden
//           <Button>("Cancel").onClick.AddListener(() =>
//           {
//               UIManager.panelManager.Pop();
//           });
//        UIManager.Instance.GetOrAddComponentInChilden
//           <Button>("Continue").onClick.AddListener(() =>
//           {
//               _continueEvent.Invoke();
//           });
//        UIManager.Instance.GetOrAddComponentInChilden
//           <Text>("EnsureTipText").text = ensureTipString;
//        //EnsureTipText
//    }
//}