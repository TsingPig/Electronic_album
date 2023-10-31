using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MVPFrameWork
{
    public abstract class PresenterBase<TView> : IPresenter where TView : class, IView
    {
        protected TView _view;

        public IView View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value as TView;
            }
        }

        public virtual void Install()
        {
        }

        public virtual void Uninstall()
        {
        }

        public virtual void OnCreateCompleted()
        {
        }

        
        public virtual void OnDestroy()
        {
        }

        public virtual void OnShowStart()
        {
        }


        public virtual void OnShowCompleted()
        {
        }



        public virtual void OnHideStart()
        {
        }


        public virtual void OnHideCompleted()
        {
        }

        

        
    }

}
