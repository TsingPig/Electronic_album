using System;

namespace MVPFrameWork
{
    public interface IPopUIModule
    {
        void Enter(int viewId, Action callback = null);

        void Quit(int viewId, Action callback = null);

        void Preload(int viewId, bool instantiate = true);
    }
}