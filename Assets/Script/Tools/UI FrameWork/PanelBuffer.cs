using System.Collections.Generic;
using UnityEngine;
namespace TsingPigSDK
{
    /// <summary>
    /// ������������ջ���洢UI
    /// </summary>
    public class PanelBuffer
    {
        public GameObject TopPanelObject
        {
            get { return GetSingleUI(_topPanel.UIType); }
        }

        /// <summary>
        /// ���ջ
        /// </summary>
        private Stack<BasePanel> _panelStack;

        /// <summary>
        /// ��ǰջ�������
        /// </summary>
        private BasePanel _topPanel;

        private Dictionary<UIType, GameObject> _dicUI;
        public PanelBuffer()
        {
            _panelStack = new Stack<BasePanel>();
            _dicUI = new Dictionary<UIType, GameObject>();
        }

        /// <summary>
        /// panel�����ջ����
        /// </summary>
        /// <param name="nextPanel">Ҫ��ʾ�����</param>
        public void Push(BasePanel nextPanel)
        {
            Debug.Log(nextPanel.UIType.Name);
            if (_panelStack.Count > 0)
            {
                _topPanel = _panelStack.Peek();
                _topPanel.OnPause();
            }

            _panelStack.Push(nextPanel);
            GetSingleUI(nextPanel.UIType);
            _topPanel = _panelStack.Peek();
            _topPanel.OnEntry();
        }
        public void Pop()
        {
            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().OnExit();
                DestroyUI(_panelStack.Peek().UIType);
                _panelStack.Pop();
            }
            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().OnResume();
            }
        }

        /// <summary>
        /// ��ʾһ��UI����
        /// </summary>
        /// <param name="type">ui��Ϣ</param>
        /// <returns></returns>
        public GameObject GetSingleUI(UIType type)
        {
            GameObject parent = GameObject.Find("Canvas");
            if (parent != null)
            {
                if (_dicUI.ContainsKey(type))
                {
                    return _dicUI[type];
                }
                else
                {
                    Log.Info("InstantiateSingleUI");
                    GameObject uiAsset = Res.Load<GameObject>(type.Name);
                    GameObject ui = GameObject.Instantiate(uiAsset, parent.transform);
                    ui.name = type.Name;
                    _dicUI.Add(type, ui);
                    return ui;
                }
            }
            else
            {

                Log.Error("��ʧCanvas���봴��Canvas����");
                return null;
            }
        }
        public void DestroyUI(UIType type)
        {
            if (_dicUI.ContainsKey(type))
            {
                GameObject.Destroy(_dicUI[type]);
                _dicUI.Remove(type);
            }
        }

    }
}