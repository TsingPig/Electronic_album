using System;

namespace MVPFrameWork
{
    public interface IUIModule
    {
        void Enter(int viewId, Action callback = null);

        void Quit(int viewId, Action callback = null, bool destroy = false);

    }
}