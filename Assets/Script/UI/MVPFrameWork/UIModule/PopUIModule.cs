using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVPFrameWork
{
    public sealed class PopUIModule : IPopUIModule
    {
        private struct ViewState
        {
            public bool active;
        }

        private IUIModule _uiModule;


        private SpecialStack<IntGroup> _viewStack = new SpecialStack<IntGroup>();

        private Dictionary<int, ViewState> _viewDic = new Dictionary<int, ViewState>();

        private List<int> _tempQuitList = new List<int>();


        public PopUIModule()
        {
            _uiModule = new UIModule();
        }

        public void Enter(int viewId, Action callback = null)
        {
            _viewDic.TryGetValue(viewId, out var value);
            if(!value.active)
            {
                value.active = true;
                _viewDic[viewId] = value;
                _uiModule?.Enter(viewId, delegate
                {
                    callback?.Invoke();
                    _uiModule?.Focus(viewId);
                    this.OnViewEnterCompletedEvent?.Invoke(viewId);
                });
            }
            else
            {
                _uiModule?.Focus(viewId);
                callback?.Invoke();
            }
        }


        public void Quit(int viewId,  Action callback = null)
        {
            if(_viewDic.TryGetValue(viewId, out var value))
            {
                if(value.active)
                {
                    value.active = false;
                    _viewDic[viewId] = value;
                    _uiModule?.Quit(viewId, delegate
                    {
                        callback?.Invoke();
                        _uiModule?.UnFocus(viewId);
                        this.OnViewQuitCompletedEvent?.Invoke(viewId);
                    });
                }
                else
                {
                    callback?.Invoke();
                }
            }
            else
            {
                callback?.Invoke();
            }

        }


        public void UnFocus(int viewId)
        {
            _uiModule?.UnFocus(viewId);
        }





        public bool Pop(bool stayLast = true, Action callback = null)
        {
            bool result = false;
            if(_viewStack.Count > 1)
            {
                if(_viewStack.Peek(out item))
                {
                    bool flag = false;
                    for(int i = 0; i < item.Count; i++)
                    {
                        if(_viewDic.GetValueAnyway(item[i]).active)
                        {
                            flag = true;
                            break;
                        }
                    }

                    if(flag)
                    {
                        if(_viewStack.Pop(out item) && _viewStack.Peek(out dstId))
                        {
                            result = true;
                            IntGroup viewGroup = item - dstId;
                            Quit(viewGroup, QuitOptions.None, delegate
                            {
                                Enter(dstId, EnterOptions.None, callback);
                            });
                        }
                    }
                    else
                    {
                        result = true;
                        Enter(item, EnterOptions.None, callback);
                    }
                }
            }
            else if(!stayLast && _viewStack.Count == 1)
            {
                _viewStack.Pop(out var item2);
                Quit(item2);
                result = true;
            }

            return result;
        }

        public void ResetStack()
        {
            _viewStack.Clear();
        }

        public void Preload(int viewId, bool instantiate = true)
        {
            if(!_viewDic.ContainsKey(viewId))
            {
                _uiModule.Preload(viewId, instantiate);
                _viewDic[viewId] = new ViewState
                {
                    active = false
                };
            }
        }

    }


}