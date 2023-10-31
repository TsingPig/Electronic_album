using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;
using System;

public interface ILoginView : IView
{
    string account { get; set; }

    string password { get; set; }

    bool isHidden { get; set; }
}
