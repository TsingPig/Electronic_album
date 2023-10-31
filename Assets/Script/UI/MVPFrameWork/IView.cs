using System;

namespace MVPFrameWork
{
    public interface IView
    {
        IPresenter Presenter { get; set; }

        bool Active { get; set; }

        void Create(Action callback = null);

        void Show(Action callback = null);

        void Hide(Action callback = null);

        void Destroy();
    }
}