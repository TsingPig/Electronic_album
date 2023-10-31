using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVPFrameWork
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoBuildAttribute : Attribute
    {
        public string name;

        public AutoBuildAttribute()
        {
        }

        public AutoBuildAttribute(string name)
        {
            this.name = name;
        }
    }
}

