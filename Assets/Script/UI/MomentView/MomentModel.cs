using System.Collections.Generic;
using UnityEngine;

public class MomentModel : IMomentModel
{
    private GameObject _photoWallItemObj;

    public GameObject PhotoWallItemObj { get => _photoWallItemObj; set => _photoWallItemObj = value; }
    public void SetModel(GameObject photoWallItemObj)
    {
        _photoWallItemObj = photoWallItemObj;
    }
}