using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class BBSModel : IBBSModel
{

    private IBBSModel.BBS _section;

    public IBBSModel.BBS Section { get => _section; set => _section = value; }

    public void SetModel(IBBSModel.BBS section)
    {
        _section = section;
    }

}