
using System;

using UnityEngine;


namespace MVPFrameWork
{
    public abstract class ViewBase<TPresenter> : IView where TPresenter : class, IPresenter
    {
        protected RectTransform _root;

        protected CanvasGroup _rootCanvas;

        protected bool _rootCanvasIsActive;

        protected TPresenter _presenter;

        private bool _created = false;

        private string _resPath;

        public virtual bool Active
        {
            get
            {
                return _rootCanvasIsActive;
            }
            set
            {
                _rootCanvasIsActive = value;
                _rootCanvas.alpha =  value ? 1f: 0f;
                _rootCanvas.blocksRaycasts = value;
                _rootCanvas.interactable = value;
            }
        }

        public IPresenter Presenter
        {
            get
            {
                return _presenter;
            }
            set
            {
                if (_presenter != null)
                {
                    _presenter.Uninstall();
                }

                _presenter = value as TPresenter;
                if (_presenter != null)
                {
                    _presenter.View = this;
                    _presenter.Install();
                }
            }
        }

        public ViewBase()
        {
            // Presenter = Container.Resolve<TPresenter>();
        }

        
        public void Create(Action callback = null)
        {
            
        }
        

        public void Show(Action callback = null)
        {
            try
            {
                _presenter?.OnShowStart();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }

            try
            {
                OnShow(delegate
                {
                    try
                    {
                        _presenter?.OnShowCompleted();
                    }
                    catch (Exception exception3)
                    {
                        Debug.LogException(exception3);
                    }

                    callback?.Invoke();
                });
            }
            catch (Exception exception2)
            {
                Debug.LogException(exception2);
            }
        }

        public void Hide(Action callback = null)
        {
            try
            {
                _presenter?.OnHideStart();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }

            try
            {
                OnHide(delegate
                {
                    try
                    {
                        _presenter?.OnHideCompleted();
                    }
                    catch (Exception exception3)
                    {
                        Debug.LogException(exception3);
                    }

                    callback?.Invoke();
                });
            }
            catch (Exception exception2)
            {
                Debug.LogException(exception2);
            }
        }

        public void Destroy()
        {
            
        }

        protected abstract void OnCreate();

        protected virtual void OnShow(Action callback)
        {
            Active = true;
            callback?.Invoke();
        }

        protected virtual void OnHide(Action callback)
        {
            Active = false;
            callback?.Invoke();
        }

        protected virtual void OnDestroy()
        {
        }
    }
}

