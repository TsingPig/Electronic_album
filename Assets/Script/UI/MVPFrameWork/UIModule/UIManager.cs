using System;


namespace MVPFrameWork
{

    public sealed class UIManager : Singleton<UIManager>, IPopUIModule
    {
        private IPopUIModule _module;
        public UIManager()
        {
            _module = new PopUIModule();
        }
        public void Enter(int viewId,  Action callback = null)
        {
            _module?.Enter(viewId,callback);
        }
        public void Quit(int viewId, Action callback = null)
        {
            _module?.Quit(viewId, callback);
        }
      
        public void UnFocus(int viewId)
        {
            _module?.UnFocus(viewId);
        }
        public bool Pop(bool stayLast = true, Action callback = null)
        {
            if(_module != null)
            {
                return _module.Pop(stayLast, callback);
            }

            return false;
        }
        public void ResetStack()
        {
            _module?.ResetStack();
        }

        public void Preload(int viewId, bool instantiate = true)
        {
            _module?.Preload(viewId, instantiate);
        }

    }
}