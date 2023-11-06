using System;
using System.Collections.Generic;

namespace MVPFrameWork
{

    public class UIModule : IUIModule
    {
        private Dictionary<int, IView> _uiDic = new Dictionary<int, IView>();

        private IView this[int viewId]
        {
            get
            {
                IView value = null;
                _uiDic.TryGetValue(viewId, out value);
                return value;
            }
            set
            {
                _uiDic[viewId] = value;
            }
        }

        public void Enter(int viewId, Action callback = null)
        {
            IView view = this[viewId];
            if(view == null)
            {
                view = Container.Resolve<IView>(viewId);
                view?.Create(delegate
                {
                    this[viewId] = view;
                    view?.Show(callback);
                });
            }
            else
            {
                view.Show(callback);
            }
        }


        public void Quit(int viewId, Action callback = null, bool destroy = false)
        {
            IView view = this[viewId];
            if(view == null)
            {
                return;
            }

            view.Hide(delegate
            {
                if(destroy)
                {
                    view.Destroy();
                    _uiDic.Remove(viewId);
                }

                callback?.Invoke();
            });
        }


    }
}