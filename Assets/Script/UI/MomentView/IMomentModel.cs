using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMomentModel : IModel
{

    public GameObject PhotoWallItemObj { get; set; }

    public void SetModel(GameObject photoWallItemObj);
}