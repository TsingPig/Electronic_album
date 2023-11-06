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

    }
}