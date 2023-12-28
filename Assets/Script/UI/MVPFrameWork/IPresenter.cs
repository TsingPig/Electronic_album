namespace MVPFrameWork
{
    public interface IPresenter
    {
        IView View { get; set; }

        void Install();

        void Uninstall();

        void OnCreateCompleted();

        void OnShowStart();

        void OnShowCompleted();

        void OnHideStart();

        void OnHideCompleted();
    }
}