using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPopUIModule
{

}
namespace MVPFrameWork
{
    public interface IPopUIModule

    {
        void Enter(int viewId, Action callback = null);

        bool Pop(bool stayLast = true, Action callback = null);

        void Quit(int viewId, Action callback = null);

        void ResetStack();

        void Preload(int viewId, bool instantiate = true);

        void UnFocus(int viewId);

    }
}
